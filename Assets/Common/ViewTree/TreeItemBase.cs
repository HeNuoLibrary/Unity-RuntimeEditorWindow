using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public delegate void ItemEventHandler(TreeItemBase sender, PointerEventData eventData);


/// <summary>
/// ͣ����λ��
/// </summary>
public enum DropSibling { 
    None,//ͣ���ڷǲ�������
    Child,//ͣ��Ŀ����ӽڵ�
    PrevSibling,//Ŀͣ���������
    NextSibling,//ͣ��Ŀ�������
}

public class TreeItemBase :MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler
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

    public int indent;

    private List<TreeItemBase> childs = new List<TreeItemBase>();
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

            if (parent == null)
            {
                Debug.LogError("û�и�����");
                indent = 0;
            }
            else
            {
                indent = parent.indent + 40;

            }
            RectOffset rectOffset = new RectOffset(indent, 0, 0, 0);

            GetComponentInChildren<HorizontalLayoutGroup>().padding = rectOffset;

        }
    }


    private bool isExpand;//�Ƿ���չ��״̬

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


    private bool hasChild;

    public bool HasChild
    {
        get
        {
            return hasChild;
        }
        set
        {
            hasChild = value;

            if (hasChild)
            {
                Fold();
                
            }
            else
            {
                NotHasChild();
            }

        }
    }
    public int siblingIndex => transform.GetSiblingIndex();//��ǵ�ǰ������



    public virtual void InitData(string showContent) { 
    
    }
    public void AddChild(TreeItemBase treeItemBase)
    {
        if (!childs.Contains(treeItemBase))
        {
            childs.Add(treeItemBase);
     
        }
        else
        {
            Debug.LogError("�Ѿ��������ӽڵ㣬��Ӧ�ó����������");
        }
    }

    public void RemoveChild(TreeItemBase treeItemBase)
    {

        if (childs.Contains(treeItemBase))
        {
            childs.Remove(treeItemBase);
        }
        else
        {
            Debug.LogError("����û�а�������ڵ㣬��Ӧ�ó����������");
        }

    }
    /// <summary>
    /// ����������
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
        else {
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

    public virtual void ResetState()
    {
        
       
    }

}
