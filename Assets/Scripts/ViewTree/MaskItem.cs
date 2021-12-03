using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 处理拖动Item的时候显示的遮罩
/// </summary>
public class MaskItem : MonoBehaviour
{
    private TreeItemBase dropItemBase;
    private RectTransform rectTransform;
 
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetDropItemBase(TreeItemBase itemBase) {
        this.dropItemBase = itemBase;
    }

    public void SetPoistion(Vector2 mousePos) {
        if (dropItemBase==null) { 
        
        }
    
    }
}
