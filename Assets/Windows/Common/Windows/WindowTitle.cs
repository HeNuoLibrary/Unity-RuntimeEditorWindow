using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

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
            WindowsToolsOver.isMax = false;

            Debug.LogError("�϶�");

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
        WindowsToolsOver.DragEnd();


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

            WindowsToolsOver.MinWindows();
            Debug.LogError("��С��");


        });



        SetIcom();
    }

    /// <summary>
    /// ȫ����С����
    /// </summary>
    public  void FullScreen()
    {

        if (WindowsToolsOver.isMax)
        {
             Debug.LogError("��ԭ");

            WindowsToolsOver.Normal();

           // WindowsTools.SetNormalWindow();
        }
        else
        {
            Debug.LogError("���");
           // WindowsTools.SetMaxWindows();
            WindowsToolsOver.MaxWindows();

        }

        SetIcom();


    }

    

    

    

    private void SetIcom() {
        if (WindowsToolsOver.isMax)
        {
           
            max.GetComponent<Image>().sprite = Resources.Load<Sprite>("Title/3");

        }
        else
        {
           

            max.GetComponent<Image>().sprite = Resources.Load<Sprite>("Title/2");

        }
    }
}
