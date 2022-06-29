using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 拖动标签生成的默认跟随窗口
/// </summary>

public class DefaultWindowPanel : SingletonGameObject<DefaultWindowPanel>
{
    private  bool isFollow;
    public RectTransform rectTransform { get; private set; }
    private CanvasGroup canvasGroup;
    private Text lableText;
    private void Start()
    {
        lableText = transform.Find("TitleRect/Lable/Text").GetComponent<Text>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup==null) {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        Hide();
    }

    private void Update()
    {
        
        if (isFollow) {
            transform.position= UIUtil.GetScreenPointToWorldPointInRectangle(Input.mousePosition,UIManager.Instance.UICanvas);
           // transform.position = Input.mousePosition;
        
        }
    }

    public void Show() {
        canvasGroup.alpha = 1;
        transform.SetAsLastSibling();
       

    }

    public void Show(string lableName, Vector2 size, Vector3 pos) {
        Show();
        SetFollow(true);
        lableText.text = lableName;
        SetRect(size,pos);
    }

    public void Hide() {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        SetFollow(false);
    }
    public void SetFollow(bool isFollow) {
        this.isFollow = isFollow;
    }

    public void SetRect(Vector2 size,Vector3 pos) {

        UIUtil.SetRectTransformSizeWithCurrentAnchors(rectTransform, size, pos);
       

    }

   
}
