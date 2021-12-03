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
            WindowsTools.SetMinWindows();

        });



        SetIcom();
    }

    /// <summary>
    /// 全屏或小窗口
    /// </summary>
    public  void FullScreen()
    {

        if (WindowsTools.IsMax)
        {
             Debug.LogError("还原");
           
            WindowsTools.SetNormalWindow();
        }
        else
        {
            Debug.LogError("最大化");
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
