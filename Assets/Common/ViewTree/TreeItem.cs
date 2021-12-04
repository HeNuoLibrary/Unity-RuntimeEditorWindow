using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum TreeItemState
{
    Normal,//正常状态
    Selected,//选中状态
    Enter,//进入状态
}

public class TreeItem : TreeItemBase
{
    private Button foldButton;
    private Text itemName;

    public override void InitData(string name)
    {
        itemName.text = name;
    }

    private void Awake()
    {
        foldButton = transform.Find("Content/Fold").GetComponent<Button>();
        itemName = transform.Find("Content/Name").GetComponent<Text>();
        foldButton.onClick.AddListener(() =>
        {
            OnExpand?.Invoke(this, null);
        });
    }

    public override void Expand()
    {
        foldButton.GetComponent<CanvasGroup>().alpha = 1;
        foldButton.interactable = true;
        foldButton.transform.localEulerAngles = -Vector3.forward*90;
    }

    public override void Fold()
    {
        foldButton.GetComponent<CanvasGroup>().alpha = 1;
        foldButton.interactable = true;
        foldButton.transform.localEulerAngles = Vector3.zero;
    }

    public override void NotHasChild()
    {
        foldButton.GetComponent<CanvasGroup>().alpha = 0;
        foldButton.interactable = false;
    }

}
