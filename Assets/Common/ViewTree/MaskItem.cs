using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ͣ����λ��
/// </summary>
public enum DropSibling
{
    None,//ͣ���ڷǲ�������
    Child,//ͣ��Ŀ����ӽڵ�
    PrevSibling,//Ŀͣ���������
    NextSibling,//ͣ��Ŀ�������
}

/// <summary>
/// �����϶�Item��ʱ����ʾ������
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
