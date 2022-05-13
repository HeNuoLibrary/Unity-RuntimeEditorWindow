using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class FloatWindow : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerDownHandler
{
	
	public Vector2 minSize = new Vector2(540, 280);
	private RectTransform rectTransform;
	public  bool isDraging = false;//标识当前是否正在缩放窗口
	public bool selectLable;//标识当前是否在拖动里面的标签
	private DragDirection dragDirection = DragDirection.None;
	private Vector3 lastPos;
	private Vector3 offset;

	private bool isEnter;//标识鼠标是否进入面板

	private bool dragTitle;//标识当前拖动的是否是标题
	private Canvas canvas;
	public Canvas UICanvas
	{
		get
		{

			return canvas;
		}
	}
	public void OnPointerDown(PointerEventData eventData)
	{
		transform.SetAsLastSibling();
	}
	public void OnBeginDrag(PointerEventData eventData)
	{
		if (InTitleRange()&&!isDraging && !selectLable)
		{
			dragTitle = true;
			offset=UIUtil.GetScreenPointToWorldPointInRectangle(eventData.position, UICanvas);
		    offset = new Vector3(transform.position.x, transform.position.y, offset.z) - offset;
		}
	}

	public void OnDrag(PointerEventData eventData)
	{

		
		//优先处理拖动边框，再处理窗口整体拖动，不然会出现同时检测到同时处理的情况
		if (dragTitle)
		{
		   transform.position = UIUtil.GetScreenPointToWorldPointInRectangle(eventData.position, UICanvas) + offset;
		}

	}

	public void OnEndDrag(PointerEventData eventData)
	{
		dragTitle = false;
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
		isEnter = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		isEnter = false;

	}
	public bool InTitleRange()
	{
		RectTransform rectTransform = transform.Find("TitleArea") as RectTransform;

		Vector2 screenPoint = rectTransform.GetScreenPointToLocalPointInRectangle( Input.mousePosition,UICanvas);

		//DefaultWindowPanel.Instance.Hide();

		return rectTransform.rect.Contains(screenPoint);
	}

	void Start()
	{
		canvas = FindObjectOfType<Canvas>();
		this.name = "FloatWindow" + transform.GetSiblingIndex();
		rectTransform = GetComponent<RectTransform>();
		

		transform.Find("TitleArea/Close").GetComponent<Button>().onClick.AddListener(()=> {
			Destroy(gameObject);
		});
		transform.Find("TitleArea/Close").parent.SetAsLastSibling();

	}
	void Update()
	{
		
		if (Input.GetMouseButtonDown(0))
		{
			
			dragDirection = DetectionDragDirection();
			if (isEnter && dragDirection != DragDirection.None)
			{

				isDraging = true;

				lastPos = Input.mousePosition;

			}
		}
		else
		{

			if (isDraging)
			{
				if (Input.GetMouseButton(0))
				{

					Vector2 offset = Input.mousePosition - lastPos;
					lastPos = Input.mousePosition;
					FloatingWindow(dragDirection, offset);
				}
				else
				{
					isDraging = false;
				}
			}
			else
			{
				if (!Input.anyKey)
				{
					DragDirection direction = DetectionDragDirection();
					if (direction != dragDirection)
					{
						dragDirection = direction;
						WindowsDrag.SetSystemCursor(dragDirection);
					}
				}
			}

		}

	}
	

	
	private void FloatingWindow(DragDirection dragDirection, Vector2 offsetValue)
	{
		Vector2 size = Vector2.zero;
		switch (dragDirection)
		{
			case DragDirection.None:
				return;
			case DragDirection.Left:
				offsetValue.y = 0;
				size = rectTransform.rect.size - new Vector2(offsetValue.x, offsetValue.y);//移动的新区域大小
				break;
			case DragDirection.Top:
				offsetValue.x = 0;
				size = rectTransform.rect.size + new Vector2(offsetValue.x, offsetValue.y);//移动的新区域大小
				break;
			case DragDirection.Right:
				offsetValue.y = 0;
				size = rectTransform.rect.size + new Vector2(offsetValue.x, offsetValue.y);//移动的新区域大小
				break;
			case DragDirection.Bottom:
				offsetValue.x = 0;
				size = rectTransform.rect.size - new Vector2(offsetValue.x, offsetValue.y);//移动的新区域大小
				break;
			case DragDirection.LeftTop:
				size = rectTransform.rect.size + new Vector2(-offsetValue.x, offsetValue.y);//移动的新区域大小
				break;
			case DragDirection.LeftBottom:
				size = rectTransform.rect.size - new Vector2(offsetValue.x, offsetValue.y);//移动的新区域大小
				break;
			case DragDirection.RightTop:
				size = rectTransform.rect.size + new Vector2(offsetValue.x, offsetValue.y);//移动的新区域大小
				break;
			case DragDirection.RightBottom:
				size = rectTransform.rect.size + new Vector2(offsetValue.x, -offsetValue.y);//移动的新区域大小
				break;
			default:
				break;
		}

		size.x = Mathf.Max(size.x, minSize.x);
		size.y = Mathf.Max(size.y, minSize.y);
		float x = Mathf.Sign(offsetValue.x) * Mathf.Abs((size.x - rectTransform.rect.size.x) / 2);
		float y = Mathf.Sign(offsetValue.y) * Mathf.Abs((size.y - rectTransform.rect.size.y) / 2);
		rectTransform.SetRectTransformSizeWithCurrentAnchors( size.x, size.y);



		switch (UICanvas.renderMode)
		{
			case RenderMode.ScreenSpaceOverlay:
				rectTransform.position += new Vector3(x, y);
				break;
			case RenderMode.ScreenSpaceCamera:
				Vector3 tranToScreenPos = UIUtil.GetRectTransformToScreenPosition(rectTransform, UICanvas);

				rectTransform.position = UIUtil.GetScreenPointToWorldPointInRectangle(tranToScreenPos + new Vector3(x, y), UICanvas);//移动到的新位置;

				break;
			case RenderMode.WorldSpace:
				
				break;
			default:
				break;
		}
		
	
	}

	/// <summary>
	/// 拖动的时候检测鼠标位于面板的方位
	/// </summary>
	/// 
	private float detectionDis=15;
	private DragDirection DetectionDragDirection()
	{
		DragDirection dragDir = DragDirection.None;

		if (isEnter)
		{
			Vector3[] corners = rectTransform.GetRectTransformToScreenPositionFromCorners(UICanvas);
			float leftDis = Input.mousePosition.x - corners[0].x;
			float topDis = corners[1].y - Input.mousePosition.y;
			float rightDis = corners[3].x - Input.mousePosition.x;
			float bottomDis = Input.mousePosition.y - corners[0].y;


			if (Vector2.Distance(Input.mousePosition, corners[0]) < detectionDis)
			{
				dragDir = DragDirection.LeftBottom;
				
			}else
			if (Vector2.Distance(Input.mousePosition, corners[1]) < detectionDis)
			{
				dragDir = DragDirection.LeftTop;
				
			}
			else
			if (Vector2.Distance(Input.mousePosition, corners[2]) < detectionDis)
			{
				dragDir = DragDirection.RightTop;
			}
			else

			if (Vector2.Distance(Input.mousePosition, corners[3]) < detectionDis)
			{
				dragDir = DragDirection.RightBottom;
			}
			else

			if (leftDis < detectionDis && leftDis < topDis && leftDis < rightDis && leftDis < bottomDis)
			{
				dragDir = DragDirection.Left;
			

				

			}
			else
			if (topDis < detectionDis && topDis < leftDis && topDis < rightDis && topDis < bottomDis)
			{
				dragDir = DragDirection.Top;
			
			

			}
			else
			if (rightDis < detectionDis && rightDis < topDis && rightDis < leftDis && rightDis < bottomDis)
			{
				dragDir = DragDirection.Right;
			}
			else

			if (bottomDis < detectionDis && bottomDis < topDis && bottomDis < rightDis && bottomDis < leftDis)
			{
				dragDir = DragDirection.Bottom;
			}

		}

	

		return dragDir;

	}

   
}
