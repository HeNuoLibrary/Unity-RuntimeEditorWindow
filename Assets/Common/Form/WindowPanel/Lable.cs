using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Lable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerDownHandler
{
    private Text lableText;
    public string lableName { get; private set; }//标签名字


    private bool canDrag=true;//标识现在的标签是否能拖动

    public bool inLableRect;//标记当前标签是否在标签栏下面
    public WindowPanel windowPanel { get; private set; }//关联的窗体

    private CanvasGroup canvasGroup;

    public RectTransform content;

    public bool CanInteraction
    {

        get {

            if (windowPanel.transform.parent.GetComponent<FloatWindow>() != null)
            {
                //如果这是一个浮动多的窗口，则优先处理浮动窗口的拖拽，所以先判断是否在拖动
               
               return  !windowPanel.transform.parent.GetComponent<FloatWindow>().isDraging;

            }
            else {

               ///如果是停靠窗口，则判断当前是否是最后一个面板，最后一个面板不允许拖动
                Node node = windowPanel.transform.parent.GetComponent<Node>();

                // windowPanel.isLastLable  判断是否是最后一个标签
                //node  当前是否在Node下面
                // node.brotherNode  判断是否存在兄弟节点，不存在则表示当前已经是根节点了

                if (windowPanel.isLastLable && node != null && node.brotherNode == null)
                {
                    return false;

                }
                else {
                  
                    return true;
                }
                

            }


        }

    }

   

   

    /// <summary>
    /// 设置当前标签的 现实状态
    /// </summary>
    /// <param name="isVisible"></param>
    public void SetLableVisible(bool isVisible) {
        if (isVisible)
        {
            canvasGroup.alpha = 1;
           
        }
        else {
           
            canvasGroup.alpha = 0;

        }

    }

    private void Awake()
    {
        lableText = transform.Find("Text").GetComponent<Text>();
        windowPanel = GetComponentInParent<WindowPanel>();
        lableName = transform.Find("Text").GetComponent<Text>().text;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// 设置当前标签的名字以及关联面板
    /// </summary>
    /// <param name="lableName"></param>
    /// <param name="windowPanel"></param>
    public void InitData(string lableName,WindowPanel windowPanel) {
       
        this.lableName = lableName;
        this.windowPanel = windowPanel;

        lableText.text = lableName;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        //判断能否拖动
        canDrag = CanInteraction;
       
        if (canDrag) {
            if (windowPanel.transform.parent.GetComponent<FloatWindow>() != null)
            {
                windowPanel.transform.parent.GetComponent<FloatWindow>().selectLable = true;
            }
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;

            //移除但是不删除
            //windowPanel.RemoveLable(this);
            transform.SetParent(WindowPanelManager.Instance.UICanvas.transform);
            transform.SetAsLastSibling();
            inLableRect = false;
            WindowPanelManager.Instance.OnBeginDragLabel(this);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.LogError("标签拖动中"+ canDrag);
        if (canDrag)
        {
            WindowPanelManager.Instance.OnDragLabel(this);
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
       // Debug.LogError("标签拖动结束"+ canDrag);

        if (canDrag)
        {
            if (windowPanel.transform.parent.GetComponent<FloatWindow>() != null)
            {
                windowPanel.transform.parent.GetComponent<FloatWindow>().selectLable=false ;
              

            }
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
            WindowPanelManager.Instance.OnEndDragLabel(this);
        }

    }

   
    
    /// <summary>
    /// 设置当前标签的颜色
    /// </summary>
    /// <param name="color"></param>
    public void SetLableColor(Color color)
    {
        GetComponent<Image>().color = color;
    }

	
	public void OnPointerDown(PointerEventData eventData)
	{
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            windowPanel.SetContent(this);
        }
        else
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                LablePanel lablePanel = UIManager.Instance.Show<LablePanel>();
                lablePanel.transform.position = eventData.position;
                lablePanel.Show(this);
            }
        }
    }

   
}
