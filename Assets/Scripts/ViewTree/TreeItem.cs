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

    public int siblingIndex => transform.GetSiblingIndex();//标记当前的索引
    private Button foldButton;
    private Text itemName;
    public int  indent;

    private TreeItem parent;
    public TreeItem Parent
    {
        get {
            return parent;
        }
        set {
            parent = value;

            if (parent == null)
            {
                Debug.LogError("没有父物体");
                indent = 0;
            }
            else {
                indent= parent.indent + 40;
             
            }
            RectOffset rectOffset = new RectOffset(indent, 0, 0, 0);

            GetComponentInChildren<HorizontalLayoutGroup>().padding = rectOffset;

        }
    }

  
    private bool isExpand;//是否是展开状态

    public bool IsExpand
    {
        get
        {
            return isExpand;
        }
        set
        {
            isExpand = value;


            foldButton.GetComponent<CanvasGroup>().alpha = 1;

            if (isExpand)
            {
                foldButton.transform.localEulerAngles = -Vector3.up * 90;

            }
            else
            {

                foldButton.transform.localEulerAngles = Vector3.zero;
            }
        }
    }

 
    private bool hasChild;

    public bool HasChild
    {
        get {
            return hasChild;
        }
        set {
            hasChild = value;
           
        }
    }
    public void InitData(string name)
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




    private void Update()
    {

    }




}
