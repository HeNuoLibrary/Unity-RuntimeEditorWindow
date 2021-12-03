using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Lable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerDownHandler
{
    private Text lableText;
    public string lableName { get; private set; }//��ǩ����


    private bool canDrag=true;//��ʶ���ڵı�ǩ�Ƿ����϶�

    public bool inLableRect;//��ǵ�ǰ��ǩ�Ƿ��ڱ�ǩ������
    public WindowPanel windowPanel { get; private set; }//�����Ĵ���

    private CanvasGroup canvasGroup;

    public RectTransform content;

    public bool CanInteraction
    {

        get {

            if (windowPanel.transform.parent.GetComponent<FloatWindow>() != null)
            {
                //�������һ��������Ĵ��ڣ������ȴ��������ڵ���ק���������ж��Ƿ����϶�
               
               return  !windowPanel.transform.parent.GetComponent<FloatWindow>().isDraging;

            }
            else {

               ///�����ͣ�����ڣ����жϵ�ǰ�Ƿ������һ����壬���һ����岻�����϶�
                Node node = windowPanel.transform.parent.GetComponent<Node>();

                // windowPanel.isLastLable  �ж��Ƿ������һ����ǩ
                //node  ��ǰ�Ƿ���Node����
                // node.brotherNode  �ж��Ƿ�����ֵܽڵ㣬���������ʾ��ǰ�Ѿ��Ǹ��ڵ���

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
    /// ���õ�ǰ��ǩ�� ��ʵ״̬
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
    /// ���õ�ǰ��ǩ�������Լ��������
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
        //�ж��ܷ��϶�
        canDrag = CanInteraction;
       
        if (canDrag) {
            if (windowPanel.transform.parent.GetComponent<FloatWindow>() != null)
            {
                windowPanel.transform.parent.GetComponent<FloatWindow>().selectLable = true;
            }
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;

            //�Ƴ����ǲ�ɾ��
            //windowPanel.RemoveLable(this);
            transform.SetParent(WindowPanelManager.Instance.UICanvas.transform);
            transform.SetAsLastSibling();
            inLableRect = false;
            WindowPanelManager.Instance.OnBeginDragLabel(this);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.LogError("��ǩ�϶���"+ canDrag);
        if (canDrag)
        {
            WindowPanelManager.Instance.OnDragLabel(this);
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
       // Debug.LogError("��ǩ�϶�����"+ canDrag);

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
    /// ���õ�ǰ��ǩ����ɫ
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
