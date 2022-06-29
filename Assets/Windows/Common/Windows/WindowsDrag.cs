using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static WindowsMouseButtonEvent;


public class WindowsDrag:MonoBehaviour
{
    public Vector2 minSize = new Vector2(480, 270);
    public bool AutoMaxWindow=false;

    private static bool isDraging = false;//是否在拖拽中
    public static bool IsDraging {
        get {
            return isDraging;
        }
    }

    private DragDirection dragDirection = DragDirection.None;
    private Vector2 lastPos;

    
    public void InitWindowData(Vector2 minSize) {
        this.minSize = minSize;
      
    }


    
    private void Awake()
    {
#if !UNITY_EDITOR && PLATFORM_STANDALONE_WIN
        MinimumWindowSize.Set((int )minSize.x,(int )minSize.y);

          WindowsTools.HideTitle();
        if (AutoMaxWindow)
        {
        WindowsToolsOver.MaxWindows();

     
        // WindowsTools.SetMaxWindows();

    }
        else { 
        WindowsToolsOver.MinWindows();
       
         //WindowsTools.SetNormalWindow();

        }


#endif

    }

    private void Start()
    {
        if (AutoMaxWindow)
        {
#if !UNITY_EDITOR && PLATFORM_STANDALONE_WIN

            WindowsToolsOver.MaxWindows();
#endif

        }
    }

    private void OnEnable()
    {
     
        //自定义方法
         WindowsMouseButtonEvent.Instance.SetHook(MouseButtonsHandle);

        //系统自带的方法 
        //WindowsMouseButtonEvent.Instance.SetHook(WindowsMouseButtonsHandle);


    }
    private void OnDisable()
    {
        WindowsMouseButtonEvent.Instance.UnHook();

    }

    private void WindowsMouseButtonsHandle(MouseButtons mouseButtons)
    {
        if (WindowsTools.IsMax)
        {
            return;
        }
        switch (mouseButtons)
        {
            case MouseButtons.LeftButtonDown:
                 
                dragDirection = DetectionDragDirection();
             
                if (dragDirection != DragDirection.None)
                {
                    isDraging = true;
                    WindowsTools.MoveWindow(dragDirection);
                }

                break;
            case MouseButtons.LeftButtonUp:
                isDraging = false;
                break;
            case MouseButtons.RightButtonDown:
                break;
            case MouseButtons.RightButtonUp:
                break;
            case MouseButtons.MiddleButtonDown:
                break;
            case MouseButtons.MiddleButtonUp:
                break;
            case MouseButtons.MouseMove:
                DetectionDragDirection();
                break;
            default:
                break;
        }

    }

    private void MouseButtonsHandle(MouseButtons mouseButtons)
    {

        if (WindowsTools.IsMax)
        {
            return;
        }
        switch (mouseButtons)
        {
            case MouseButtons.LeftButtonDown:
              
                dragDirection = DetectionDragDirection();
              

                if (dragDirection != DragDirection.None) {
                   
                    lastPos = WindowsTools.GetCursorPos();
                    isDraging = true;

                }


                break;
            case MouseButtons.LeftButtonUp:
                isDraging = false;

                break;
            case MouseButtons.RightButtonDown:
                break;
            case MouseButtons.RightButtonUp:
                break;
            case MouseButtons.MiddleButtonDown:
                break;
            case MouseButtons.MiddleButtonUp:
                break;
            case MouseButtons.MouseMove:
                if (isDraging)
                {
                    

                    Vector2 offset = WindowsTools.GetCursorPos() - lastPos;

                    lastPos = WindowsTools.GetCursorPos();

                    Rect rect = FloatingWindow(dragDirection, offset);

                    WindowsTools.SetWindowPos(rect);
                   
                }

                DetectionDragDirection();


                break;
            default:
                break;
        }

    }

    private Rect FloatingWindow(DragDirection dragDirection, Vector2 offsetValue)
    {
        Vector2 newSize = Vector2.zero;
        Rect windowRec = WindowsTools.GetWindowRect();

        float newWidth=0;
        float newHeight=0;
        Vector3 newPos = windowRec.position;
        switch (dragDirection)
        {
            case DragDirection.None:
                return windowRec;
            case DragDirection.Left:

                //拖动左边  高度（y）不受影响,计算新的窗体宽度

                newWidth = windowRec.size.x - offsetValue.x;

                //判断窗口是否在用户设定的范围内
                if (newWidth < minSize.x)
                {

                    offsetValue.x -= (minSize.x- newWidth);
                }
               
                newSize = windowRec.size - new Vector2(offsetValue.x, 0);//移动的新区域大小
                //限制在设定范围之内


                newPos = windowRec.position + new Vector2(offsetValue.x, 0);

                break;
            case DragDirection.Top:
                //拖动上边边  宽度（x）不受影响,计算新的窗体宽度

                newHeight = windowRec.size.y + offsetValue.y;

                //判断窗口是否在用户设定的范围内
                if (newHeight < minSize.y)
                {

                    offsetValue.y -= (minSize.y- newHeight);
                }


                newSize = windowRec.size - new Vector2(0, offsetValue.y);//移动的新区域大小
                newPos = windowRec.position + new Vector2(0, offsetValue.y);
                break;
            case DragDirection.Right:
                //拖动右边  高度（y）不受影响,计算新的窗体宽度
                //不会影响位置变化
                newPos = windowRec.position;


                newWidth = windowRec.size.x + offsetValue.x;

                //判断窗口是否在用户设定的范围内
                if (newWidth < minSize.x)
                {
                    //Debug.LogError("新的偏移量"+(offsetValue.x - (minSize.x - newWidth)));
                    offsetValue.x += (minSize.x - newWidth);
                }

                newSize = windowRec.size + new Vector2(offsetValue.x, 0);//移动的新区域大小
                break;
            case DragDirection.Bottom:
                //从下边拖动，不影响窗体的位置变化，只会引起高度（y）变化

               

                //所以新的位置任然是原来的位置
                newPos = windowRec.position;

                //根据最小范围重新设置一下偏移量
                newHeight = windowRec.size.y + offsetValue.y;
                if (newHeight < minSize.y)
                {
                    offsetValue.y += ( minSize.y- newHeight);
                }


                newSize = windowRec.size + new Vector2(0, offsetValue.y);//移动的新区域大小
                break;
            case DragDirection.LeftTop:
                newWidth = windowRec.size.x - offsetValue.x;

                //判断窗口是否在用户设定的范围内
                if (newWidth < minSize.x)
                {

                    offsetValue.x -= (minSize.x- newWidth);
                }
                //根据最小范围重新设置一下偏移量
                newHeight = windowRec.size.y - offsetValue.y;
                if (newHeight < minSize.y)
                {

                    offsetValue.y -= (minSize.y- newHeight);
                   
                    
                }

                newPos = windowRec.position + offsetValue;
                newSize = windowRec.size - new Vector2(offsetValue.x, offsetValue.y);//移动的新区域大小
                break;
            case DragDirection.LeftBottom:

                newWidth = windowRec.size.x - offsetValue.x;

                //判断窗口是否在用户设定的范围内
                if (newWidth < minSize.x)
                {

                    offsetValue.x -= ( minSize.x- newWidth);
                }
                //根据最小范围重新设置一下偏移量
                newHeight = windowRec.size.y + offsetValue.y;
                if (newHeight < minSize.y)
                {

                    offsetValue.y += (minSize.y- newHeight);
                }

                newSize = windowRec.size + new Vector2(-offsetValue.x, offsetValue.y);//移动的新区域大小
                newPos = windowRec.position + new Vector2(offsetValue.x,0);
                break;
            case DragDirection.RightTop:

                newWidth = windowRec.size.x + offsetValue.x;

                //判断窗口是否在用户设定的范围内
                if (newWidth < minSize.x)
                {

                    offsetValue.x += (minSize.x- newWidth);
                }
                //根据最小范围重新设置一下偏移量
                newHeight = windowRec.size.y - offsetValue.y;
                if (newHeight < minSize.y)
                {

                    offsetValue.y -= (minSize.y- newHeight);
                }
                newSize = windowRec.size + new Vector2(offsetValue.x, -offsetValue.y);//移动的新区域大小
               
                newPos = windowRec.position + new Vector2(0, offsetValue.y);
                break;
            case DragDirection.RightBottom:

                newWidth = windowRec.size.x + offsetValue.x;

                //判断窗口是否在用户设定的范围内
                if (newWidth < minSize.x)
                {

                    offsetValue.x += ( minSize.x- newWidth);
                }
                //根据最小范围重新设置一下偏移量
                newHeight = windowRec.size.y + offsetValue.y;
                if (newHeight < minSize.y)
                {

                    offsetValue.y += (minSize.y- newHeight);
                }

                //新的位置不会改变
                newPos = windowRec.position;

                newSize = windowRec.size + new Vector2(offsetValue.x, offsetValue.y);//移动的新区域大小
                break;
            default:
                break;
        }
    
        


       
        windowRec.position = newPos;

        windowRec.size = newSize;

        return windowRec;

    }
    private float detectionDis=10;

    /// <summary>
    /// 拖动的时候检测鼠标位于面板的方位
    /// </summary>
    public DragDirection DetectionDragDirection()
    {
        DragDirection dragDir = DragDirection.None;

        Vector2[] corners = new Vector2[4];

        Vector2 mousePos = WindowsTools.GetCursorPos();


        Rect rect = WindowsTools.GetWindowRect();

        if (!rect.Contains(mousePos)) {
            if (dragDirection != dragDir)
            {
                dragDirection = dragDir;
                SetSystemCursor(dragDir);

            }
            return DragDirection.None;
        }
       

        corners[0] = rect.position + Vector2.up * rect.height;
        corners[1] = rect.position;
        corners[2] = rect.position + Vector2.right * rect.width; ;
        corners[3] = rect.position + rect.size;






        float leftDis = Math.Abs(mousePos.x - corners[0].x);
        float topDis = Math.Abs(corners[1].y - mousePos.y);
        float rightDis = Math.Abs(corners[3].x - mousePos.x);
        float bottomDis = Math.Abs(mousePos.y - corners[0].y);


        if (Vector3.Distance(mousePos, corners[0]) < detectionDis)
        {
            dragDir = DragDirection.LeftBottom;
        }
        else
            if (Vector3.Distance(mousePos, corners[1]) < detectionDis)
        {
            dragDir = DragDirection.LeftTop;
        }
        else
            if (Vector3.Distance(mousePos, corners[2]) < detectionDis)
        {
            dragDir = DragDirection.RightTop;
        }
        else

            if (Vector3.Distance(mousePos, corners[3]) < detectionDis)
        {
            dragDir = DragDirection.RightBottom;
        }
        else
            if (leftDis < detectionDis && leftDis < topDis && leftDis < rightDis && leftDis < bottomDis)
        {
            dragDir = DragDirection.Left;
        }
        else
            if (topDis < detectionDis && topDis < leftDis && topDis < rightDis && topDis < bottomDis)
        {
            dragDir = DragDirection.Top;

        }
        else
            if (rightDis < detectionDis && rightDis < topDis && rightDis < leftDis && rightDis < bottomDis)
        {

            dragDir = DragDirection.Right;

        }
        else

            if (bottomDis < detectionDis && bottomDis < topDis && bottomDis < rightDis && bottomDis < leftDis)
        {
            dragDir = DragDirection.Bottom;


        }
        if (dragDirection!=dragDir) {
           dragDirection = dragDir;
           SetSystemCursor(dragDir);

        }

        return dragDir;

    }

    void OnApplicationQuit()
    {
        //恢复为系统默认图标
        WindowsMouseCursor.SystemParametersInfo();
    }

    static DragDirection  cursorDirection = DragDirection.None;//表示一下当前鼠标拖拽的方向
    public static void SetSystemCursor(DragDirection dragDirection)
    {
        if (cursorDirection== dragDirection) {
            return;
        }
         cursorDirection = dragDirection;
       
       // Debug.LogError(dragDirection);

       

        string path = UnityEngine.Application.streamingAssetsPath;


        switch (dragDirection)
        {
            case DragDirection.None:
                WindowsMouseCursor. SystemParametersInfo();
                break;
            case DragDirection.Left:
                WindowsMouseCursor.SetCursor(path + "/Cursous/aero_ew_l.cur");
               


                break;
            case DragDirection.Top:
                WindowsMouseCursor.SetCursor(path + "/Cursous/aero_ns_l.cur");

           

                break;
            case DragDirection.Right:
                WindowsMouseCursor.SetCursor(path + "/Cursous/aero_ew_l.cur");

                

                break;
            case DragDirection.Bottom:
                WindowsMouseCursor.SetCursor(path + "/Cursous/aero_ns_l.cur");

               

                break;
            case DragDirection.LeftTop:
                WindowsMouseCursor.SetCursor(path + "/Cursous/aero_nwse_l.cur");

                break;
            case DragDirection.LeftBottom:
                WindowsMouseCursor.SetCursor(path + "/Cursous/aero_nesw_l.cur");

                break;
            case DragDirection.RightTop:
                WindowsMouseCursor.SetCursor(path + "/Cursous/aero_nesw_l.cur");

                break;
            case DragDirection.RightBottom:
                WindowsMouseCursor.SetCursor(path + "/Cursous/aero_nwse_l.cur");

                break;
            default:
                break;
        }

    }


}

