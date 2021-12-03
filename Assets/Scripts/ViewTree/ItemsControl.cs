using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemsControl : MonoBehaviour
{
     private List<TreeItemBase> treeItemBaseList;
    /// <summary>
    /// ��ǰ�����ͣ������
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
    /// ������
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

    private void OnEnable()
    {
        TreeItemBase.OnExpand += OnExpand;
        TreeItemBase.PointerEnter += OnItemPointerEnter;

    }

    private void OnExpand(TreeItemBase sender, PointerEventData eventData)
    {
        if (sender) { 
        
        }
        OnExpand(sender);
    }
    protected virtual void OnExpand(TreeItemBase sender) { 
    
    }

    private void OnItemPointerEnter(TreeItemBase sender, PointerEventData eventData)
    {
        if (!CanHandleEvent(sender)) {
            return ;
        }
        droupItemBase = sender;
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
            //��ʼ��������Ϊ����ɾ�����еĽڵ�
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
                //����µĽڵ�
                for (int i = 0; i < deltaItems; ++i)
                {
                    InstantiateTreeItemBase(treeItemBaseList.Count);
                }
            }
            else
            {
                //ɾ������Ľڵ�
                int newLength = treeItemBaseList.Count + deltaItems;
                for (int i = treeItemBaseList.Count - 1; i >= newLength; i--)
                {
                    DestroyTreeItemBase(i);
                }
            }
        }

        for (int i = 0; i < treeItemBaseList.Count; ++i)
        {
            ///���ýڵ����������
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
                    ///�������ݵ��¼�����ȥ
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
    /// ������
    /// </summary>
    /// <param name="item"></param>
    /// <param name="treeItemBase"></param>
    public virtual void DataBindItem(object item, TreeItemBase treeItemBase)
    {
    }
    protected void DestroyTreeItemBase(int siblingIndex)
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
}
