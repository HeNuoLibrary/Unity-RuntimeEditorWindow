using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LablePanel : PanelBase
{
    private BtItem[] btItem;

	private Lable lable;
	private Transform lables;
	public void Show(Lable lable) {

		this.lable = lable;
		transform.SetAsLastSibling();
	
	}
	private void OnEnable()
	{
		btItem=transform.GetComponentsInChildren<BtItem>(true);
		lables = transform.Find("Lables");
		HidPanelLable();
		foreach (var item in btItem)
		{
			BtItem bt = item;

			item.onClick += BtItemCkick;

		}
		transform.SetAsLastSibling();
	}
	

	private void OnDisable()
	{
		foreach (var item in btItem)
		{
			BtItem bt = item;
			item.onClick -= BtItemCkick;

		}
	}


	private void Start()
	{
		Hide();
	}
	private void Update()
	{
		if (Input.GetMouseButtonDown(0)) {
			if (!((lables as RectTransform).RectangleContainsScreenPoint(Input.mousePosition, UIManager.Instance.UICanvas)||
				GetComponent<RectTransform>().RectangleContainsScreenPoint(Input.mousePosition, UIManager.Instance.UICanvas))) {
				Hide();
			}
			
		}
	}
	private void BtItemCkick(BtItem btItem) {
		if (btItem.name == "CloseLable")
		{
			WindowPanelManager.Instance.DeleteLable(lable);

			Hide();

		}		
		else if (btItem.name == "DefaultLayout")
		{			
			WindowPanelManager.Instance.LoadLayout(Persistence.DefaultLayoutName);
			Hide();

		}
		else if (btItem.name == "LoadLayout")
		{
			Hide();

		}
		else if (btItem.name == "SaveLayout")
		{
			Hide();
		}
		else if (btItem.name == "AddLable") {

		}
		else { 
			lable.windowPanel.SetLableContentByLableName(btItem.name);
			Hide();

		}
	}
	public void ShowPanelLable(Vector3 pos) {
		lables.position = pos;
		lables.gameObject.SetActive(true);
	}
	public void HidPanelLable()
	{
		lables.gameObject.SetActive(false);
	}

	public bool IsStayInLable() {
		return (lables as RectTransform).RectangleContainsScreenPoint(Input.mousePosition, UIManager.Instance.UICanvas);
	}

}
