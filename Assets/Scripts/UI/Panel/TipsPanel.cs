using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TipsPanel : MonoBehaviour
{
	private Text tipText;
	private Button yesBt;
	private Button noBt;
	private Button okBt;
	UnityAction yesCallBack;
	UnityAction noCallBack;
	private void Awake()
	{
		
		
	}

	public void ShowTips(string tipContent,UnityAction yesCallBack,UnityAction noCallBack) {
		tipText.text = tipContent;
		this.yesCallBack = yesCallBack;
		this.noCallBack = noCallBack;
		yesBt.gameObject.SetActive(true);
		noBt.gameObject.SetActive(true);
		okBt.gameObject.SetActive(false);
	}
	public void ShowTips(string tipContent)
	{
		tipText.text = tipContent;
		yesBt.gameObject.SetActive(false);
		noBt.gameObject.SetActive(false);
		okBt.gameObject.SetActive(true);
	}

}
