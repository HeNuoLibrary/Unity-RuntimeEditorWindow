using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ItemBindDataArgs : EventArgs {
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

public class ViewTreeController : ItemsControl
{

    public EventHandler<ItemExpandingArgs> itemExpandingArgs;
    public EventHandler<ItemBindDataArgs> itemBindDataArgs;
    void Start()
    {
        ItemObjs = Resources.FindObjectsOfTypeAll<GameObject>().Where(go => !IsPrefab(go.transform) && go.transform.parent == null).OrderBy(t => t.transform.GetSiblingIndex());

    }
    public static bool IsPrefab(Transform This)
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            throw new InvalidOperationException("Does not work in edit mode");
        }
        return This.gameObject.scene.buildIndex < 0;
    }
    public override void DataBindItem(object item, TreeItemBase treeItemBase)
    {
       
        base.DataBindItem(item, treeItemBase);
        ItemBindDataArgs args = new ItemBindDataArgs(treeItemBase.ItemObj);
        itemBindDataArgs?.Invoke(this,args);//监听这个函数主要是为了更新
        TreeItem treeItem=(TreeItem)treeItemBase;
        treeItem.HasChild = args.hasChild;
        treeItem.InitData(args.showContent);
    }
   

    /// <summary>
    /// 点击扩展的时候
    /// </summary>
    /// <param name="item"></param>
    protected override void OnExpand(TreeItemBase item)
    {
        base.OnExpand(item);
        ItemExpandingArgs args = new ItemExpandingArgs(item.ItemObj);
        itemExpandingArgs(this, args);
        IEnumerable children = args.Children;
        TreeItem treeItem = item as TreeItem;

        int containerIndex = treeItem.siblingIndex;
      

        ///点击的时候主要是为了填充子节点  让用户自行去添加子节点

        if (children != null)
        {
            treeItem.IsExpand = true;
            foreach (object childItem in children)
            {
                containerIndex++;
                Debug.LogError(containerIndex);
                TreeItem childTreeItem = (TreeItem)InstantiateTreeItemBase(containerIndex);
                childTreeItem.Parent = treeItem;
                childTreeItem.ItemObj = childItem;
                DataBindItem(this, childTreeItem);
            }

        }
       
    }

}
