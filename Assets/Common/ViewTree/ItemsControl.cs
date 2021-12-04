using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemExpandingArgs : EventArgs
{
    /// <summary>
    /// Data Item
    /// </summary>
    public object Item
    {
        get;
        private set;
    }

    /// <summary>
    /// 使用此属性指定项的子项
    /// </summary>
    public IEnumerable Children
    {
        get;
        set;
    }

    public ItemExpandingArgs(object item)
    {
        Item = item;
    }
}
public class ItemBindDataArgs : EventArgs
{
    public bool hasChild;//是否有子项
    public string showContent;//显示的内容
    /// <summary>
    /// Data Item
    /// </summary>
    public object Item
    {
        get;
        private set;
    }
    public ItemBindDataArgs(object item)
    {
        Item = item;
    }
}
public class ItemsControl : MonoBehaviour
{
     private List<TreeItemBase> treeItemBaseList;
    private MaskItem maskItem;
    /// <summary>
    /// 当前鼠标悬停的物体
    /// </summary>
    private TreeItemBase droupItemBase;
    public object DroupItemObj
    {
        get
        {
            if (droupItemBase == null)
            {
                return null;
            }
            return droupItemBase.ItemObj;
        }
    }
    /// <summary>
    /// 数据项
    /// </summary>
    private IList<object> itemObjs;
    public IEnumerable ItemObjs
    {
        get { return itemObjs; }
        set
        {
            if (value == null)
            {
                itemObjs = null;
            }
            else
            {
                itemObjs = value.OfType<object>().ToList();
            }
            DataBind();
        }
    }
    private void Awake()
    {
        maskItem = GetComponentInChildren<MaskItem>();
    }

    private void OnEnable()
    {
        TreeItemBase.OnExpand += OnExpand;
        TreeItemBase.BeginDrag += OnBeginDrag;
        TreeItemBase.Drag += OnDrag;
        TreeItemBase.EndDrag += OnEndDrag;

        TreeItemBase.PointerEnter += OnItemPointerEnter;
        TreeItemBase.PointerExit += OnPointerExit;


    }
    private void OnPointerExit(TreeItemBase sender, PointerEventData eventData)
    {
        if (!CanHandleEvent(sender))
        {
            return;
        }
        droupItemBase = null;
        maskItem.SetDropItemBase(droupItemBase);
    }
    private void OnItemPointerEnter(TreeItemBase sender, PointerEventData eventData)
    {
        if (!CanHandleEvent(sender))
        {
            return;
        }
        droupItemBase = sender;
        maskItem.SetDropItemBase(droupItemBase);

    }
    private void OnBeginDrag(TreeItemBase sender, PointerEventData eventData)
    {
      
    }
    private void OnDrag(TreeItemBase sender, PointerEventData eventData)
    {
        maskItem.SetPoistion(eventData.position);

    }
    private void OnEndDrag(TreeItemBase sender, PointerEventData eventData)
    {
        
    }

   

    
    private void OnDisable()
    {
        
    }
    private void OnExpand(TreeItemBase item, PointerEventData eventData)
    {
        if (!item.IsExpand)
        {
            if (item.Childs.Count <= 0)
            {
                ItemExpandingArgs args = new ItemExpandingArgs(item.ItemObj);

                ///没有子项的时候让用户自行添加数据
                OnExpand(args);
                IEnumerable children = args.Children;

                int containerIndex = item.siblingIndex;
                ///点击的时候主要是为了填充子节点  让用户自行去添加子节点

                if (children != null)
                {
                    //设置状态为展开状态
                    item.IsExpand = true;
                    foreach (object childItem in children)
                    {
                        containerIndex++;
                        TreeItem childTreeItem = (TreeItem)InstantiateTreeItemBase(containerIndex);
                        //设置父节点
                        childTreeItem.Parent = item;
                        childTreeItem.ItemObj = childItem;

                        //设置子节点
                        item.AddChild(childTreeItem);

                        DataBindItem(this, childTreeItem);
                    }

                }
            }
            else
            {

                //如果已经有子项，则直接展开子项

                ExpandTreeItem(item);
            }

        }
        else
        {
            // 直接则叠起来



            FoldTreeItem(item);

        }
   
    }

  
    protected virtual void OnExpand(ItemExpandingArgs args) { 


    }
    
       

    protected bool CanHandleEvent(object sender)
    {
        TreeItemBase treeItemBase = sender as TreeItemBase;
        if (!treeItemBase)
        {
            return false;
        }
        return treeItemBase.transform.IsChildOf(transform);
    }
    protected virtual void DataBind()
    {
        treeItemBaseList = GetComponentsInChildren<TreeItemBase>().ToList();
        if (treeItemBaseList == null)
        {
            //初始化的数据为空则删除所有的节点
            for (int i = 0; i < treeItemBaseList.Count; ++i)
            {
                DestroyImmediate(treeItemBaseList[i].gameObject);
            }
        }
        else
        {
            int deltaItems = itemObjs.Count - treeItemBaseList.Count;
            if (deltaItems > 0)
            {
                //添加新的节点
                for (int i = 0; i < deltaItems; ++i)
                {
                    InstantiateTreeItemBase(treeItemBaseList.Count);
                }
            }
            else
            {
                //删除多余的节点
                int newLength = treeItemBaseList.Count + deltaItems;
                for (int i = treeItemBaseList.Count - 1; i >= newLength; i--)
                {
                    DestroyTreeItemBase(i);
                }
            }
        }

        for (int i = 0; i < treeItemBaseList.Count; ++i)
        {
            ///重置节点下面的数据
            TreeItemBase itemContainer = treeItemBaseList[i];
            if (itemContainer != null)
            {
                itemContainer.ResetState();
            }
        }

        if (itemObjs != null)
        {
            for (int i = 0; i < itemObjs.Count; ++i)
            {
                object item = itemObjs[i];
                TreeItemBase treeItemBase = treeItemBaseList[i];
 
                if (treeItemBase != null)
                {
                    treeItemBase.ItemObj = item;


                    ///将绑定数据的事件发出去
                    DataBindItem(item, treeItemBase);
                }
            }
        }
    }


    protected virtual TreeItemBase InstantiateTreeItemBase(int siblingIndex)
    {
        GameObject treeItemObj = Resources.Load<GameObject>("ViewTree/treeItem");
        TreeItemBase treeItemBase = Instantiate(treeItemObj, transform).GetComponent<TreeItemBase>();
        treeItemBaseList.Insert(siblingIndex, treeItemBase);
        treeItemBase.transform.SetSiblingIndex(siblingIndex);
        return treeItemBase;
    }
    /// <summary>
    /// 绑定数据
    /// </summary>
    /// <param name="item"></param>
    /// <param name="treeItemBase"></param>
    private  void DataBindItem(object item, TreeItemBase treeItemBase)
    {
        ItemBindDataArgs args = new ItemBindDataArgs(treeItemBase.ItemObj);
        DataBindItem(args);
        treeItemBase.HasChild = args.hasChild;
        treeItemBase.InitData(args.showContent);
    }

    protected virtual void DataBindItem(ItemBindDataArgs args) { 
    
    }
    private void DestroyTreeItemBase(int siblingIndex)
    {
        if (treeItemBaseList == null)
        {
            return;
        }

        if (siblingIndex >= 0 && siblingIndex < treeItemBaseList.Count)
        {
            DestroyImmediate(treeItemBaseList[siblingIndex].gameObject);
            treeItemBaseList.RemoveAt(siblingIndex);
        }
    }

    public void ExpandTreeItem(TreeItemBase treeItemBase) {
        treeItemBase.IsExpand = true;
        for (int i = 0; i < treeItemBase.Childs.Count; i++)
        {
            if (treeItemBase.Childs[i].IsExpand) {
                ExpandChiidTreeItem(treeItemBase.Childs[i]);
            }
            else {
                treeItemBase.Childs[i].gameObject.SetActive(true);
            }
           
        }
    }

    private void ExpandChiidTreeItem(TreeItemBase child) {
        child.gameObject.SetActive(true);
       
        if (child.IsExpand)
        {


            for (int i = 0; i < child.Childs.Count; i++)
            {
                ExpandChiidTreeItem(child.Childs[i]);

            }

        }
       
    }

    public void FoldTreeItem(TreeItemBase treeItemBase) {
        treeItemBase.IsExpand = false;
        for (int i = 0; i < treeItemBase.Childs.Count; i++)
        {
            if (treeItemBase.Childs[i].HasChild)
            {
                FoldChiidTreeItem(treeItemBase.Childs[i]);
            }
            else {
                treeItemBase.Childs[i].gameObject.SetActive(false);
            }
        }
     
        ///找到最后一个兄弟节点
       
    }

    private void FoldChiidTreeItem(TreeItemBase child) {
        child.gameObject.SetActive(false);

        if (child.HasChild)
        {
            for (int i = 0; i < child.Childs.Count; i++)
            {
                FoldChiidTreeItem(child.Childs[i]);

            }

        }
       
    }

}
