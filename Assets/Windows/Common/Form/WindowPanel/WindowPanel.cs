using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public bool isEmptyWindow
	{
		get
		{
			return panelTitle.lableRect.IsEmpty();
		}

		private set { isEmptyWindow = value; }
	}//标识当前面板是否是个空面板，里面标签为0即为空面板

	public bool isLastLable {
		get {
			return panelTitle.lableRect.IsLastLable();
		}
	}

	public RectTransform rectTransform { get; private set; }
	public PanelTitle panelTitle { get; private set; }
	public bool isEnter { get; private set; }

	public RectTransform contentRect { get; private set; }

	public void InitData()
	{
		contentRect = transform.Find("ContentRect") as RectTransform;
		rectTransform = GetComponent<RectTransform>();
		panelTitle = transform.Find("TitleRect").GetComponent<PanelTitle>();
		panelTitle.InitData();


		Lable lable = panelTitle.lableRect.GetLastLable();
		if (lable != null)
		{
			SetContent(lable);
		}
		
	}
	
	private void Start()
	{
		InitData();
		WindowPanelManager.Instance.OnBeginDragLabelCallEvent += OnBeginDragLabelCallEvent;
		WindowPanelManager.Instance.OnDragLabelCallEvent += OnDragLabelCallEvent;
		WindowPanelManager.Instance.OnEndDragLabelCallEvent += OnEndDragLabelCallEvent;
	}

	



	private void OnDestroy()
	{
		//Debug.LogError("隐藏标题栏");
		WindowPanelManager.Instance.OnBeginDragLabelCallEvent -= OnBeginDragLabelCallEvent;
		WindowPanelManager.Instance.OnDragLabelCallEvent -= OnDragLabelCallEvent;
		WindowPanelManager.Instance.OnEndDragLabelCallEvent -= OnEndDragLabelCallEvent;
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
		
		isEnter = true;

	}

	public void OnPointerExit(PointerEventData eventData)
	{
		isEnter = false;

	}
	#region 窗口拖动模块

	public RectTransform relationNode;
	private DragDirection currentDirection;

	void OnBeginDragLabelCallEvent(Lable lable)
	{
		

		if (!isEnter ||panelTitle.lableRect.isEnter )
		{
			return;
		}
		DefaultWindowPanel.Instance.Show();
		DefaultWindowPanel.Instance.SetFollow(true);
		currentDirection = DragDirection.None;
	}
	void OnDragLabelCallEvent(Lable lable)
	{
		if (!isEnter || panelTitle.lableRect.isEnter)
		{
			return;
		}

		DragDirection direction = GetStayEdgeDirection();
		lable.SetLableVisible(false);

		DefaultWindowPanel.Instance.Show();
		if (currentDirection != direction)
		{
			currentDirection = direction;
			if (isEmptyWindow || direction == DragDirection.None)
			{
				DefaultWindowPanel.Instance.SetFollow(true);
			}
			else
			{
				SetStayEdge(currentDirection);

				DefaultWindowPanel.Instance.SetFollow(false);

			}
		}
		else {
			if (currentDirection== DragDirection.None) { 
			
				DefaultWindowPanel.Instance.SetFollow(true);
			}

		}



	}
	void OnEndDragLabelCallEvent(Lable lable)
	{
		//鼠标没有进入||鼠标位于标签栏里面||现在窗口不是一个空窗口
		if (!isEnter || panelTitle.lableRect.isEnter)
		{
			return;
		}
		lable.SetLableVisible(true);

		DefaultWindowPanel.Instance.Hide();
		DragDirection direction = DragDirection.None;
		if (!isEmptyWindow) {
			direction = GetStayEdgeDirection();
		}
		if (direction != DragDirection.None)
		{
			DividePanel(direction,lable);
		}
		else
		{
			//创建一个悬浮窗口
			WindowPanelManager.Instance.CreateFloatWindow(lable);
		}
		isEnter = false;
		WindowPanelManager.Instance.currentWindowPanel.SetDefaultContent();
	}
	/// <summary>
	/// 划分面板
	/// </summary>
	private void DividePanel(DragDirection direction,Lable lable) {
		Vector3[] mousePoint = UIUtil.GetRectTransformToScreenPositionFromCorners(rectTransform, WindowPanelManager.Instance.UICanvas);

		Vector2 newRectSize = Vector2.zero;
		Vector3 newRectPos = Vector3.zero;

		Vector2 panelRectSize = Vector2.zero;
		Vector3 panelRectPos = Vector3.zero;

		switch (direction)
		{

			case DragDirection.Left:

				newRectSize = new Vector2(rectTransform.rect.size.x / 3, rectTransform.rect.size.y);
				newRectPos = ScreenPointToCanvasWorldPoint(new Vector2(mousePoint[0].x, mousePoint[0].y) + newRectSize / 2);

				panelRectSize = rectTransform.rect.size - Vector2.right * newRectSize.x;
				panelRectPos = GetNewPosByDeltaValue(rectTransform, Vector2.right * newRectSize.x / 2);

				//panelRectPos = new Vector2(rectTransform.position.x, rectTransform.position.y) + Vector2.right * newRectSize.x / 2;

				DefaultWindowPanel.Instance.SetRect(newRectSize, newRectPos);
				break;
			case DragDirection.Right:
				newRectSize = new Vector2(rectTransform.rect.size.x / 3, rectTransform.rect.size.y);

				newRectPos = new Vector2(mousePoint[3].x, mousePoint[3].y) + new Vector2(-(newRectSize / 2).x, (newRectSize / 2).y);
				newRectPos = ScreenPointToCanvasWorldPoint(newRectPos);

				panelRectSize = rectTransform.rect.size - Vector2.right * newRectSize.x;
				panelRectPos = GetNewPosByDeltaValue(rectTransform, -Vector2.right * newRectSize.x / 2);
				//panelRectSize = new Vector2(rectTransform.position.x, rectTransform.position.y) - Vector2.right * newRectSize.x / 2;
				DefaultWindowPanel.Instance.SetRect(newRectSize, newRectPos);

				break;
			case DragDirection.Top:
				newRectSize = new Vector2(rectTransform.rect.size.x, rectTransform.rect.size.y / 3);
				newRectPos = new Vector2(mousePoint[1].x, mousePoint[1].y) + new Vector2((newRectSize / 2).x, -(newRectSize / 2).y);
				newRectPos = ScreenPointToCanvasWorldPoint(newRectPos);

				panelRectSize = rectTransform.rect.size - Vector2.up * newRectSize.y;
				//newRectPos = new Vector2(rectTransform.position.x, rectTransform.position.y) - Vector2.up * newRectSize.y / 2;
				panelRectPos = GetNewPosByDeltaValue(rectTransform, -Vector2.up * newRectSize.y / 2);
				DefaultWindowPanel.Instance.SetRect(newRectSize, newRectPos);
				break;
			case DragDirection.Bottom:
				newRectSize = new Vector2(rectTransform.rect.size.x, rectTransform.rect.size.y / 3);

				newRectPos = new Vector2(mousePoint[0].x, mousePoint[0].y) + newRectSize / 2;
				newRectPos = ScreenPointToCanvasWorldPoint(newRectPos);


				panelRectSize = rectTransform.rect.size - Vector2.up * newRectSize.y;
				panelRectPos = GetNewPosByDeltaValue(rectTransform, Vector2.up * newRectSize.y / 2);

				//panelRectPos = new Vector2(rectTransform.position.x, rectTransform.position.y) + Vector2.up * newRectSize.y / 2;
				DefaultWindowPanel.Instance.SetRect(newRectSize, newRectPos);

				break;
			default:
				break;
		}


		//创建新的面板并设置新面板的位置和大小

		UIUtil.SetRectTransformSizeWithCurrentAnchors(rectTransform, panelRectSize, panelRectPos);

		WindowPanel windowPanel = WindowPanelManager.Instance.CreatePanelWindow(this, direction, lable, newRectSize, newRectPos);
	}
	/// <summary>
	/// 设置跟随面板为靠边面板
	/// </summary>
	/// <param name="direction"></param>
	/// <param name="mousePoint"></param>

	private void SetStayEdge(DragDirection direction) {
		Vector3[] mousePoint = rectTransform.GetRectTransformToScreenPositionFromCorners(WindowPanelManager.Instance.UICanvas);
		Vector2 newRectSize = Vector2.zero;
		Vector3 newRectPos = Vector3.zero;

		switch (direction)
		{

			case DragDirection.Left:
				newRectSize = new Vector2(rectTransform.rect.size.x / 3, rectTransform.rect.size.y);
				newRectPos = ScreenPointToCanvasWorldPoint(new Vector2(mousePoint[0].x, mousePoint[0].y) + newRectSize / 2);
				DefaultWindowPanel.Instance.SetRect(newRectSize, newRectPos);
				break;
			case DragDirection.Right:
				newRectSize = new Vector2(rectTransform.rect.size.x / 3, rectTransform.rect.size.y);

				newRectPos = new Vector2(mousePoint[3].x, mousePoint[3].y) + new Vector2(-(newRectSize / 2).x, (newRectSize / 2).y);
				newRectPos = ScreenPointToCanvasWorldPoint(newRectPos);
				DefaultWindowPanel.Instance.SetRect(newRectSize, newRectPos);
				break;
			case DragDirection.Top:
				newRectSize = new Vector2(rectTransform.rect.size.x, rectTransform.rect.size.y / 3);
				newRectPos = new Vector2(mousePoint[1].x, mousePoint[1].y) + new Vector2((newRectSize / 2).x, -(newRectSize / 2).y);
				newRectPos = ScreenPointToCanvasWorldPoint(newRectPos);
				DefaultWindowPanel.Instance.SetRect(newRectSize, newRectPos);
				break;
			case DragDirection.Bottom:
				newRectSize = new Vector2(rectTransform.rect.size.x, rectTransform.rect.size.y / 3);

				newRectPos = new Vector2(mousePoint[0].x, mousePoint[0].y) + newRectSize / 2;
				newRectPos = ScreenPointToCanvasWorldPoint(newRectPos);
				DefaultWindowPanel.Instance.SetRect(newRectSize, newRectPos);

				break;
			default:
				break;
		}
	}

	/// <summary>
	/// 将屏幕上的点转成Canvas 上的世界坐标
	/// </summary>
	/// <param name="screenPoint"></param>
	/// <returns></returns>
	public Vector3 ScreenPointToCanvasWorldPoint(Vector3 screenPoint) {

		return UIUtil.GetScreenPointToWorldPointInRectangle(screenPoint, WindowPanelManager.Instance.UICanvas);
	
	}
	/// <summary>
	/// 根据移动增量计算新的位置
	/// </summary>
	/// <param name="rect"></param>
	/// <param name="deltaValue"></param>
	/// <returns></returns>
	private Vector3 GetNewPosByDeltaValue(RectTransform rect, Vector3 deltaValue)
	{
		switch (WindowPanelManager.Instance.UICanvas.renderMode)
		{
			case RenderMode.ScreenSpaceOverlay:
				return rect.position + deltaValue;
		
			case RenderMode.ScreenSpaceCamera:
				Vector3 tranToScreenPos = UIUtil.GetRectTransformToScreenPosition(rect, WindowPanelManager.Instance.UICanvas);

				return  UIUtil.GetScreenPointToWorldPointInRectangle(tranToScreenPos + deltaValue, WindowPanelManager.Instance.UICanvas);//移动到的新位置;

			
			case RenderMode.WorldSpace:
				//Vector3 tranToScreenPos = UIUtil.GetRectTransformToScreenPosition(rectTransform, UIManager.Instance.UICanvas);

				//rectTransform.position = UIUtil.GetScreenPointToWorldPointInRectangle(tranToScreenPos + new Vector3(x, y), UIManager.Instance.UICanvas);//移动到的新位置;

				break;
			default:
				break;
		}

		return Vector3.zero;
	}
	private float directionDis = 100;

	/// <summary>
	///获取停靠方向
	/// </summary>
	/// <returns></returns>
	private DragDirection GetStayEdgeDirection()
	{
		DragDirection direction = DragDirection.None;


		Vector3[] mousePoint = UIUtil.GetRectTransformToScreenPositionFromCorners(rectTransform, WindowPanelManager.Instance.UICanvas);

		float leftDis = Input.mousePosition.x - mousePoint[0].x;
		float topDis = mousePoint[1].y - Input.mousePosition.y;
		float rightDis = mousePoint[3].x - Input.mousePosition.x;
		float bottomDis = Input.mousePosition.y - mousePoint[0].y;
		if (leftDis < directionDis && leftDis < topDis && leftDis < rightDis && leftDis < bottomDis)
		{
			direction = DragDirection.Left;
			//Debug.Log("停靠左边");


		}
		if (topDis < directionDis && topDis < leftDis && topDis < rightDis && topDis < bottomDis)
		{
			//Debug.Log("停靠上边");
			direction = DragDirection.Top;
		}
		if (rightDis < directionDis && rightDis < topDis && rightDis < leftDis && rightDis < bottomDis)
		{
			//Debug.Log("停靠右边");
			direction = DragDirection.Right;

		}

		if (bottomDis < directionDis && bottomDis < topDis && bottomDis < rightDis && bottomDis < leftDis)
		{
			//Debug.Log("停靠下边");
			direction = DragDirection.Bottom;

		}

		return direction;
	}



	#endregion

	/// <summary>
	/// 获取当前面板的矩形框
	/// </summary>
	/// <returns></returns>
	public Rect GetPanelRect()
	{
		return GetComponent<RectTransform>().rect;
	}
	/// <summary>
	/// 根据标签名设置面板内容
	/// </summary>
	/// <param name="lableName"></param>
	public void SetLableContentByLableName(string lableName)
	{
		
		panelTitle.lableRect.SetLableContentByLableName(lableName, this);
	}
	/// <summary>
	/// 根据标签设置面板内容
	/// </summary>
	/// <param name="lable"></param>
	public void SetLableContentByLable(Lable lable)
	{
		panelTitle.lableRect.SetLableContentByLable(lable, this);
	}

	/// <summary>
	/// 为面板添加标签，但是不设置内容
	/// </summary>
	/// <param name="lablesName"></param>
	public void AddLables(string[] lablesName) {
		panelTitle.lableRect.AddLables(lablesName);
	}


	/// <summary>
	/// 设置当前面板的大小和位置
	/// </summary>
	/// <param name="size"></param>
	/// <param name="pos"></param>
	internal void SetRectTransformSize(Vector2 size,Vector3 pos)
	{
		UIUtil.SetRectTransformSizeWithCurrentAnchors(rectTransform, size, pos);
	}


	/// <summary>
	/// 根据标签设置面板内容
	/// </summary>
	/// <param name="lable"></param>
	internal void SetContent(Lable lable)
	{
		RectTransform panelContent = lable.content;
		if (panelContent == null) {
			panelContent = WindowPanelManager.Instance.GetContentByLableName(lable.lableName);
			lable.content = panelContent;
		}
		

		panelTitle.lableRect.HightLable(lable);


		//for (int i = contentRect.childCount - 1; i >= 0; i--)
		//{
		//	Destroy(contentRect.GetChild(i).gameObject);
		//}

		panelContent.SetParent(contentRect);
		panelContent.gameObject.SetActive(true);
		panelContent.anchorMin = Vector2.zero;
		panelContent.anchorMax = Vector2.one;
		panelContent.offsetMin = Vector2.zero;
		panelContent.offsetMax = Vector2.zero;

		contentRect.offsetMax = -new Vector2(2, panelTitle.GetComponent<RectTransform>().rect.height);
		contentRect.offsetMin = new Vector2(2, 2);
		panelContent.localScale = Vector3.one;
		panelContent.localPosition = Vector3.zero;
		panelContent.SetRectTransformSizeWithCurrentAnchors( contentRect.rect.width, contentRect.rect.height);
	}

	/// <summary>
	/// 设置窗口的默认内容
	/// </summary>
	internal void SetDefaultContent()
	{

		if (isEmptyWindow)
		{
			FullParentNode();
		}
		else {
			Lable lable = panelTitle.lableRect.GetLastLable();

			if (lable != null)
			{
				SetContent(lable);

			}
			else {
				Debug.LogError("绝对不可能出现这种情况");
			}
		}
		
	}

	/// <summary>
	/// 让兄弟节点来填充父节点
	/// </summary>
	private void FullParentNode()
	{

		Node node = transform.parent.GetComponent<Node>();

		if (node.brotherNode != null)
		{
			node.brotherNode.FullParentNode();

		}
		DestroyImmediate(node.gameObject);
	}

}
