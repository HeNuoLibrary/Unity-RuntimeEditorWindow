using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class UIManager : SingletonGameObject<UIManager>
{

    private Canvas canvas;
    public Canvas UICanvas
    {
        get
        {
            if (canvas == null)
            {
                canvas = FindObjectOfType<Canvas>();
            }
            return canvas;
        }
    }

    public RectTransform CanvasRectTransform
    {
        get
        {

            return UICanvas.GetComponent<RectTransform>();
        }
    }
    public T Show<T>() where T : PanelBase
	{
		string panelName = typeof(T).ToString();
		T t= UICanvas.gameObject.GetComponentInChildren<T>(true);
		if (t==null) {
			RectTransform panel =Instantiate(Resources.Load<GameObject>("UI/Panel/" + panelName).GetComponent<RectTransform>());
			panel.parent = FindObjectOfType<Canvas>().transform;
			panel.transform.localPosition = Vector3.zero;
			panel.localScale = Vector3.one;
			panel.offsetMin = new Vector2(0, 0);
			panel.offsetMax = new Vector2(0, 0);
			t = panel.GetComponent<T>();
		}

		t?.Show();
		return t;
	}
	public PanelBase Show(string panelName) {
		PanelBase panelBase=null;
		foreach (var item in UICanvas.GetComponentsInChildren<PanelBase>())
		{
			if (item.name== panelName) {
				panelBase = item;
			}
		}

		if (panelBase == null)
		{
			RectTransform panel = Instantiate(Resources.Load<GameObject>("UI/Panel/" + panelName).GetComponent<RectTransform>());
			panel.parent = FindObjectOfType<Canvas>().transform;
			panel.transform.localPosition = Vector3.zero;
			panel.localScale = Vector3.one;
			panel.offsetMin = new Vector2(0, 0);
			panel.offsetMax = new Vector2(0, 0);
			panelBase = panel.GetComponent<PanelBase>();
		}

		panelBase?.Show();
		return panelBase;
	}
	public void HideAllPanel()
	{
		foreach (var item in UICanvas.GetComponentsInChildren<PanelBase>())
		{
			item.Hide();
		}
	}
	public void Hide<T>() where T : PanelBase
	{
		T t = UICanvas.gameObject.GetComponentInChildren<T>(true);
		t?.Hide();
	}
	public T GetPanel<T>() where T : PanelBase
	{
		T t= UICanvas.gameObject.GetComponentInChildren<T>(true);
		return t;
	}
}
