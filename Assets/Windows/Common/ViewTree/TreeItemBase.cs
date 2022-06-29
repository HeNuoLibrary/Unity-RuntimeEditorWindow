using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public delegate void ItemEventHandler(TreeItemBase sender, PointerEventData eventData);




public class TreeItemBase : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler
{
    //
    public static event EventHandler Selected;
    public static event EventHandler UnSelected;

    public static event ItemEventHandler PointerDown;
    public static event ItemEventHandler PointerUp;
    public static event ItemEventHandler DoubleClick;
    public static event ItemEventHandler PointerEnter;
    public static event ItemEventHandler PointerExit;
    public static event ItemEventHandler BeginDrag;
    public static event ItemEventHandler Drag;
    public static event ItemEventHandler Drop;
    public static event ItemEventHandler EndDrag;

    public static ItemEventHandler OnExpand;
    public static ItemEventHandler OnFold;
    public int indent => GetComponentInChildren<HorizontalLayoutGroup>().padding.left;

    private bool isSelected;
    public bool IsSelected
    {
        get {
            return isSelected;
        }
        set {

            if (value)
            {
                Selected?.Invoke(this, null);
                SelectedItem();
            }
            else {
                if (isSelected) { 
                    UnSelected?.Invoke(this, null);

                    UnSelectedItem();
                }
            }
            isSelected = value;
        }
    }



    private bool isInitData;//标记是否已经初始化数据
    public bool IsInitData {
        get {

            return isInitData;

        }
        set {
            isInitData = value;
        }
    }


    public List<TreeItemBase> childs = new List<TreeItemBase>();
    public List<TreeItemBase> Childs
    {
        get {
            return childs;
        }
    }

    private TreeItemBase parent;
    public TreeItemBase Parent
    {
        get
        {
            return parent;
        }
        set
        {
            parent = value;

           
        }
    }
    private bool canExpand;//标记当前节点是否能够展开
    public bool CanExpand
    {
        get
        {
            return canExpand;
        }
        set
        {
            canExpand = value;
            if (canExpand)
            {
                Fold();

            }
            else
            {
                NotHasChild();
            }
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

            if (isExpand)
            {
                Expand();
            }
            else
            {
                Fold();
            }
        }
    }

    #region  Unity 方法

    public void OnPointerDown(PointerEventData eventData)
    {
        if (PointerDown != null)
        {
            PointerDown(this, eventData);
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.clickCount == 2)
        {
            if (DoubleClick != null)
            {
                DoubleClick(this, eventData);
            }
        }
        else
        {
            if (PointerUp != null)
            {
                PointerUp(this, eventData);
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (PointerEnter != null)
        {
            PointerEnter(this, eventData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (PointerExit != null)
        {
            PointerExit(this, eventData);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (BeginDrag != null)
        {
            BeginDrag(this, eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Drag != null)
        {
            Drag(this, eventData);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (Drop != null)
        {
            Drop(this, eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (EndDrag != null)
        {
            EndDrag(this, eventData);
        }
    }

    #endregion
    
  
	
	public virtual void InitData(string showContent) { 
    
    }

    public int IndexOf(TreeItemBase treeItemBase) {
        if (childs.Contains(treeItemBase))
        {
            return childs.IndexOf(treeItemBase);
        }
        else
        {
            Debug.LogError("没有包含该子节点，不应该出现这种情况");
            return 0;

        }

    }

    public bool IsContain(TreeItemBase treeItemBase) {

		foreach (var item in Childs)
		{
            if (item == treeItemBase)
            {
                return true;
            }
            else {
               return item.IsContain(treeItemBase);
            }
		}

        return false;

    }
    /// <summary>
    /// 查找最后一个节点
    /// </summary>
    /// <param name="treeItemBase"></param>
    /// <returns></returns>
    public TreeItemBase FindLastNode(TreeItemBase treeItemBase) {

        // Debug.LogError("查找到的节点==" + (treeItemBase.ItemObj as GameObject).name);

        if (treeItemBase.Childs.Count > 0)
        {
            return FindLastNode(treeItemBase.Childs[treeItemBase.Childs.Count - 1]);

        }
        else {
            return treeItemBase;
        }
    }

    /// <summary>
    /// 更新子节点的位置
    /// </summary>
    /// <param name="treeItemBase"></param>
    public void UpdateChildRectOffset(TreeItemBase treeItemBase)
    {
        int index = IndexOf(treeItemBase);
        RectOffset rectOffset = new RectOffset(treeItemBase.Parent.indent + 30, 0, 0, 0);
        treeItemBase.GetComponentInChildren<HorizontalLayoutGroup>().padding = rectOffset;
       
        treeItemBase.gameObject.SetActive(true);
    }

    public void AddChild(int siblingIndex, TreeItemBase treeItemBase)
    {
        if (!childs.Contains(treeItemBase))
        {
            treeItemBase.Parent = this;
            treeItemBase.transform.SetAsLastSibling();
            
            TreeItemBase lastTree = FindLastNode(this);
            int index = lastTree.transform.GetSiblingIndex();
            //将节点放置在最后一个节点
            treeItemBase.transform.SetSiblingIndex(index + 1);
            childs.Insert(siblingIndex, treeItemBase);
        }
        else
        {
            Debug.LogError("已经包含该子节点，不应该出现这种情况");
        }
    }

    /// <summary>
    /// 移除一个节点
    /// </summary>
    /// <param name="treeItemBase"></param>
    public void RemoveChild(TreeItemBase treeItemBase)
    {

        if (childs.Contains(treeItemBase))
        {
            childs.Remove(treeItemBase);
            if (childs.Count==0) {
                IsExpand = false;
                CanExpand = false;
            }
        }
        else
        {
            Debug.LogError("里面没有包含这个节点，不应该出现这种情况");
        }

    }
    /// <summary>
    /// 关联的数据
    /// </summary>
    public object ItemObj
    {
        get;
        set;
    }


    public virtual void Expand() { 


    }

    public virtual void Fold() {
       
    }

    public virtual void  NotHasChild() {
       
    }

    public virtual void SelectedItem()
    {

    }

          public virtual void UnSelectedItem()
    {

    }





}
