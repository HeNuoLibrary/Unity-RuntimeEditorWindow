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
    private Image selectImage;

    public override void InitData(string name)
    {
        itemName.text = name;
        this.name = name;
    }

    private void Awake()
    {
        foldButton = transform.Find("Content/Icon/Fold").GetComponent<Button>();
        itemName = transform.Find("Content/Name").GetComponent<Text>();

        selectImage = transform.Find("Selected").GetComponent<Image>();
        foldButton.onClick.AddListener(() =>
        {
            OnExpand?.Invoke(this, null);
        });
        UnSelectedItem();
    }

    public override void Expand()
    {
       // Debug.LogError("展开  " + (ItemObj as GameObject).name);

        foldButton.gameObject.SetActive(true);
        foldButton.transform.localEulerAngles = -Vector3.forward*90;
    }

    public override void Fold()
    {
        Debug.LogError("折叠  " + (ItemObj as GameObject).name);
        foldButton.gameObject.SetActive(true);

        foldButton.transform.localEulerAngles = Vector3.zero;
    }

    public override void NotHasChild()
    {
      //  Debug.LogError("不能展开  " + (ItemObj as GameObject).name);
        foldButton.gameObject.SetActive(false);

    }

	public override void SelectedItem()
	{
        selectImage.gameObject.SetActive(true);

    }

	public override void UnSelectedItem()
	{
        selectImage.gameObject.SetActive(false);

    }

}
