using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LableRect : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
	public RectTransform rectTransform;//

	public WindowPanel windowPanel;
	public bool isEnter { get;private set; }
    
    public void InitData()
	{
		rectTransform = GetComponent<RectTransform>();
		windowPanel = GetComponentInParent<WindowPanel>();
	}

	private void Start()
	{
		WindowPanelManager.Instance.OnBeginDragLabelCallEvent += OnBeginDragLabelCallEvent;
		WindowPanelManager.Instance.OnDragLabelCallEvent += OnDragLableCallEvent;
		WindowPanelManager.Instance.OnEndDragLabelCallEvent += OnEndDragLabelCallEvent;
	}

	private void OnDestroy()
	{
		WindowPanelManager.Instance.OnBeginDragLabelCallEvent -= OnBeginDragLabelCallEvent;
		WindowPanelManager.Instance.OnDragLabelCallEvent -= OnDragLableCallEvent;
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

	void OnBeginDragLabelCallEvent(Lable lable)
    {
		if (!isEnter)
		{
			return;
		}
	}
    void OnDragLableCallEvent(Lable lable)
    {
		if (!isEnter)
		{
			return;
		}
		
		lable.SetLableVisible(true);
		DefaultWindowPanel.Instance.Hide();

		lable.transform.position = new Vector3(UIUtil.GetScreenPointToWorldPointInRectangle(Input.mousePosition,UIManager.Instance.UICanvas).x, transform.position.y, transform.position.z);
		
		
	}
    void OnEndDragLabelCallEvent(Lable lable)
    {
		if (!isEnter)
		{
			return;
		}
		SetLableContentByLable(lable, windowPanel);
		//设置当前面板的内容
		if (this != WindowPanelManager.Instance.currentWindowPanel)
		{

			WindowPanelManager.Instance.currentWindowPanel.SetDefaultContent();
		}
	}

	public void SetLableContentByLableName(string lableName, WindowPanel windowPanel)
	{
		GameObject lableObj = Instantiate(Resources.Load<GameObject>("Lable"), transform);
		Lable lable = lableObj.GetComponent<Lable>();
		lable.InitData(lableName, windowPanel);
		SetLableContentByLable(lable, windowPanel);

	}
	public void SetLableContentByLable(Lable lable, WindowPanel windowPanel)
	{
			lable.transform.SetParent(transform);
			lable.inLableRect = true;
			lable.InitData(lable.lableName, windowPanel);
			HightLable(lable);
			windowPanel.SetContent(lable);
			UIUtil.ForceRebuildLayoutImmediate(rectTransform);

	}


	/// <summary>
	/// 添加标签设置当前标签内容
	/// </summary>
	/// <param name="lablesName"></param>
	public void AddLables(string [] lablesName) {

        for (int i = 0; i < lablesName.Length; i++)
        {

			GameObject lableObj = Instantiate(Resources.Load<GameObject>("Lable"), transform);
			Lable lable = lableObj.GetComponent<Lable>();
			lable.InitData(lablesName[i], windowPanel);
		}

	}

	public string[] GetLables() {
		Lable[] lables = GetComponentsInChildren<Lable>();

		string[] lablesName = new string[lables.Length];

        for (int i = 0; i < lables.Length; i++)
        {
			lablesName[i] = lables[i].lableName;
		}

		return lablesName;
	}


	public bool IsEmpty() {
		Lable[] lables = GetComponentsInChildren<Lable>();

		if (lables!=null&& lables.Length>0) {
			return false;
		}
		return true;
	}

	public bool IsLastLable() {
		Lable[] lables = GetComponentsInChildren<Lable>();

		if (lables != null && lables.Length ==1)
		{
			return true;
		}
		return false;
	}

	//public void RemoveLable(Lable lable) {
	//	if (lables.Contains(lable))
	//	{
	//		lables.Remove(lable);
	//	}
	//	else {
	//		Debug.LogError("移除标签失败，根本不存在这个标签");
	//	}

	//}


	/// <summary>
	/// 设置当前标签的颜色
	/// </summary>
	/// <param name="color"></param>
	public void HightLable(Lable lable )
    {
		lable.SetLableVisible(true);
		Lable[] lables = GetComponentsInChildren<Lable>();
        foreach (var item in lables)
        {
            item.SetLableColor(new Color(0.1568628f, 0.1568628f, 0.1568628f,1));
        }
        lable.SetLableColor(new Color(0.2352941f, 0.2352941f, 0.2352941f,1));
    }

	public Lable GetLastLable() {
		Lable[] lables = GetComponentsInChildren<Lable>();

		if (lables!=null&& lables.Length>0) {

			return lables[lables.Length - 1];


		}

		return null;
	}
	

}
