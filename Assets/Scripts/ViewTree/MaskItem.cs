using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����϶�Item��ʱ����ʾ������
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
