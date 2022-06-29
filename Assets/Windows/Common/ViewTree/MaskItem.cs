using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 停留的位置
/// </summary>
public enum DropSibling
{
    None,//停留在非操作区域
    Child,//停留目标的子节点
    PrevSibling,//目停留标的上面
    NextSibling,//停留目标的下面
}

/// <summary>
/// 处理拖动Item的时候显示的遮罩
/// </summary>
public class MaskItem : MonoBehaviour
{
    private TreeItemBase dropItemBase;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private RectTransform line;
    private RectTransform frame;

    public DropSibling dropSibling { get; private set; }


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        line = transform.Find("Line") as RectTransform;
        frame = transform.Find("Frame") as RectTransform;

    }

    public void SetDropItemBase(TreeItemBase itemBase) {
        
        this.dropItemBase = itemBase;
		if (dropItemBase != null)
		{
			canvasGroup.alpha = 1;
		}
		else
		{
			dropSibling = DropSibling.None;
			canvasGroup.alpha = 0;

		}
	}

    public void SetPoistion(Vector2 mousePos) {
        if (dropItemBase != null)
        {
            RectTransform rectTransform = dropItemBase.GetComponent<RectTransform>();
            Vector2 pos = rectTransform.GetScreenPointToLocalPointInRectangle(mousePos, UIManager.Instance.UICanvas);
            transform.position = dropItemBase.transform.position;

            float height = rectTransform.rect.height/2-5;
            if (pos.y > height)
            {
                dropSibling = DropSibling.PrevSibling;
               
            }
            else {
                if (-pos.y > height)
                {
                    dropSibling = DropSibling.NextSibling;

                }
                else
                {
                    dropSibling = DropSibling.Child;


                }
            }

            SetDropSibling(dropSibling);
        }
       
    }

    private void SetDropSibling(DropSibling dropSibling) {

        line.gameObject.SetActive(false);
        frame.gameObject.SetActive(false);

        switch (dropSibling)
        {
            case DropSibling.None:
                break;
            case DropSibling.Child:
              
                frame.gameObject.SetActive(true);
                break;
            case DropSibling.PrevSibling:
               
                line.gameObject.SetActive(true);
               
                 line.localPosition = Vector3.up*18;

                break;
            case DropSibling.NextSibling:
               
                line.gameObject.SetActive(true);

                line.localPosition = -Vector3.up * 18;


                break;
            default:
                break;
        }
       
    }
}
