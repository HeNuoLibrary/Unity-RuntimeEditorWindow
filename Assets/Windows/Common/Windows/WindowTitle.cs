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

            Debug.LogError("拖动");

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
           WindowsTools.ShowMessageBox("确认退出？", "退出应用", (result) => {
               if (result)
               {
                   Debug.LogError("退出");
                   Application.Quit();
               }
               else { 
                   Debug.LogError("取消");

               }

           });
        });


        max.onClick.AddListener(() => {
           FullScreen();
        });
        
        min.onClick.AddListener(() => {

            WindowsToolsOver.MinWindows();
            Debug.LogError("最小化");


        });



        SetIcom();
    }

    /// <summary>
    /// 全屏或小窗口
    /// </summary>
    public  void FullScreen()
    {

        if (WindowsToolsOver.isMax)
        {
             Debug.LogError("还原");

            WindowsToolsOver.Normal();

           // WindowsTools.SetNormalWindow();
        }
        else
        {
            Debug.LogError("最大化");
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
