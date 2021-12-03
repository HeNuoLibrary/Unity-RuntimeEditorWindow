using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WindowTitle : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{

    public Button close;
    public Button max;
    public Button min;
    private void Awake()
    {

       

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!WindowsDrag.IsDraging) {
           
            max.GetComponent<Image>().sprite = Resources.Load<Sprite>("Title/2");
            WindowsTools.DragWindow();

        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!WindowsDrag.IsDraging)
        {
            //WindowsTools.DragWindow();
           
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
       
    }

   
    void Start()
    {
        
        close = transform.Find("Close").GetComponent<Button>();
        max = transform.Find("Max").GetComponent<Button>();
        min = transform.Find("Min").GetComponent<Button>();


        close.onClick.AddListener(() => {
           WindowsTools.ShowMessageBox("ȷ���˳���", "�˳�Ӧ��", (result) => {
               if (result)
               {
                   Debug.LogError("�˳�");
                   Application.Quit();
               }
               else { 
                   Debug.LogError("ȡ��");

               }

           });
        });


        max.onClick.AddListener(() => {
           FullScreen();
        });
        
        min.onClick.AddListener(() => {
            WindowsTools.SetMinWindows();

        });



        SetIcom();
    }

    /// <summary>
    /// ȫ����С����
    /// </summary>
    public  void FullScreen()
    {

        if (WindowsTools.IsMax)
        {
             Debug.LogError("��ԭ");
           
            WindowsTools.SetNormalWindow();
        }
        else
        {
            Debug.LogError("���");
            WindowsTools.SetMaxWindows();
        }

        SetIcom();


    }

    private void SetIcom() {
        if (WindowsTools.IsMax)
        {
           
            max.GetComponent<Image>().sprite = Resources.Load<Sprite>("Title/3");

        }
        else
        {
           

            max.GetComponent<Image>().sprite = Resources.Load<Sprite>("Title/2");

        }
    }
}
