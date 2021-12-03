using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public delegate void ItemEventHandler(TreeItemBase sender, PointerEventData eventData);


/// <summary>
/// 停留的位置
/// </summary>
public enum DropSibling { 
    None,//停留在非操作区域
    Child,//停留目标的子节点
    PrevSibling,//目停留标的上面
    NextSibling,//停留目标的下面
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

   
    /// <summary>
    /// 关联的数据
    /// </summary>
    public object ItemObj
    {
        get;
        set;
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
