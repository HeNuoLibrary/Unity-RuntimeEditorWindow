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
    private CanvasGroup canvasGroup;
    private RectTransform line;
    private RectTransform frame;


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
        else { 
            canvasGroup.alpha = 0;

        }
    }

    public void SetPoistion(Vector2 mousePos) {
        if (dropItemBase != null)
        {
            RectTransform rectTransform = dropItemBase.GetComponent<RectTransform>();
            Vector2 pos = rectTransform.GetScreenPointToLocalPointInRectangle(mousePos, UIManager.Instance.UICanvas);
            transform.position = dropItemBase.transform.position;

            float height = 20;
            if (pos.y > height)
            {

                SetDropSibling(DropSibling.PrevSibling);
            }
            else {
                if (-pos.y > height)
                {
                    SetDropSibling(DropSibling.NextSibling);
                }
                else
                {
                    SetDropSibling(DropSibling.Child);

                }
            }

            
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
                Debug.LogError("中间");
                frame.gameObject.SetActive(true);
                break;
            case DropSibling.PrevSibling:
                Debug.LogError("上面");

                line.gameObject.SetActive(true);
               
                 line.localPosition = Vector3.up*30;

                break;
            case DropSibling.NextSibling:
                Debug.LogError("下面");
                line.gameObject.SetActive(true);

                line.localPosition = -Vector3.up * 30;


                break;
            default:
                break;
        }
       
    }
}
