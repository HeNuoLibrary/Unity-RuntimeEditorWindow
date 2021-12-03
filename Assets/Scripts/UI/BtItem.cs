using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class BtItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	public Image bc;
	public UnityAction<BtItem> onClick;
	
	public void OnPointerClick(PointerEventData eventData)
	{
		onClick?.Invoke(this);
	}
	private void OnEnable()
	{
		bc = transform.Find("bc").GetComponent<Image>();
		bc.color = Color.white;

	}



	public void OnPointerEnter(PointerEventData eventData)
	{
		bc.color = new Color(0.5518868f, 0.8857751f,1,1);
		LablePanel lablePanel = UIManager.Instance.Show<LablePanel>();
		if (this.name == "AddLable")
		{
			lablePanel.ShowPanelLable(transform.Find("Fold").position);
		}
		else {
			if (!lablePanel.IsStayInLable()) { 
				lablePanel.HidPanelLable();
			}
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		LablePanel lablePanel = UIManager.Instance.Show<LablePanel>();

		if (!(this.name == "AddLable" && lablePanel.IsStayInLable()))
		{
			bc.color = Color.white;

		}
	
	}
}
