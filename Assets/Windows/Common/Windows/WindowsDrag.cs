using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static WindowsMouseButtonEvent;


public class WindowsDrag:MonoBehaviour
{
    public Vector2 minSize = new Vector2(480, 270);
    public bool AutoMaxWindow=false;

    private static bool isDraging = false;//�Ƿ�����ק��
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
     
        //�Զ��巽��
         WindowsMouseButtonEvent.Instance.SetHook(MouseButtonsHandle);

        //ϵͳ�Դ��ķ��� 
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

                //�϶����  �߶ȣ�y������Ӱ��,�����µĴ�����

                newWidth = windowRec.size.x - offsetValue.x;

                //�жϴ����Ƿ����û��趨�ķ�Χ��
                if (newWidth < minSize.x)
                {

                    offsetValue.x -= (minSize.x- newWidth);
                }
               
                newSize = windowRec.size - new Vector2(offsetValue.x, 0);//�ƶ����������С
                //�������趨��Χ֮��


                newPos = windowRec.position + new Vector2(offsetValue.x, 0);

                break;
            case DragDirection.Top:
                //�϶��ϱ߱�  ��ȣ�x������Ӱ��,�����µĴ�����

                newHeight = windowRec.size.y + offsetValue.y;

                //�жϴ����Ƿ����û��趨�ķ�Χ��
                if (newHeight < minSize.y)
                {

                    offsetValue.y -= (minSize.y- newHeight);
                }


                newSize = windowRec.size - new Vector2(0, offsetValue.y);//�ƶ����������С
                newPos = windowRec.position + new Vector2(0, offsetValue.y);
                break;
            case DragDirection.Right:
                //�϶��ұ�  �߶ȣ�y������Ӱ��,�����µĴ�����
                //����Ӱ��λ�ñ仯
                newPos = windowRec.position;


                newWidth = windowRec.size.x + offsetValue.x;

                //�жϴ����Ƿ����û��趨�ķ�Χ��
                if (newWidth < minSize.x)
                {
                    //Debug.LogError("�µ�ƫ����"+(offsetValue.x - (minSize.x - newWidth)));
                    offsetValue.x += (minSize.x - newWidth);
                }

                newSize = windowRec.size + new Vector2(offsetValue.x, 0);//�ƶ����������С
                break;
            case DragDirection.Bottom:
                //���±��϶�����Ӱ�촰���λ�ñ仯��ֻ������߶ȣ�y���仯

               

                //�����µ�λ����Ȼ��ԭ����λ��
                newPos = windowRec.position;

                //������С��Χ��������һ��ƫ����
                newHeight = windowRec.size.y + offsetValue.y;
                if (newHeight < minSize.y)
                {
                    offsetValue.y += ( minSize.y- newHeight);
                }


                newSize = windowRec.size + new Vector2(0, offsetValue.y);//�ƶ����������С
                break;
            case DragDirection.LeftTop:
                newWidth = windowRec.size.x - offsetValue.x;

                //�жϴ����Ƿ����û��趨�ķ�Χ��
                if (newWidth < minSize.x)
                {

                    offsetValue.x -= (minSize.x- newWidth);
                }
                //������С��Χ��������һ��ƫ����
                newHeight = windowRec.size.y - offsetValue.y;
                if (newHeight < minSize.y)
                {

                    offsetValue.y -= (minSize.y- newHeight);
                   
                    
                }

                newPos = windowRec.position + offsetValue;
                newSize = windowRec.size - new Vector2(offsetValue.x, offsetValue.y);//�ƶ����������С
                break;
            case DragDirection.LeftBottom:

                newWidth = windowRec.size.x - offsetValue.x;

                //�жϴ����Ƿ����û��趨�ķ�Χ��
                if (newWidth < minSize.x)
                {

                    offsetValue.x -= ( minSize.x- newWidth);
                }
                //������С��Χ��������һ��ƫ����
                newHeight = windowRec.size.y + offsetValue.y;
                if (newHeight < minSize.y)
                {

                    offsetValue.y += (minSize.y- newHeight);
                }

                newSize = windowRec.size + new Vector2(-offsetValue.x, offsetValue.y);//�ƶ����������С
                newPos = windowRec.position + new Vector2(offsetValue.x,0);
                break;
            case DragDirection.RightTop:

                newWidth = windowRec.size.x + offsetValue.x;

                //�жϴ����Ƿ����û��趨�ķ�Χ��
                if (newWidth < minSize.x)
                {

                    offsetValue.x += (minSize.x- newWidth);
                }
                //������С��Χ��������һ��ƫ����
                newHeight = windowRec.size.y - offsetValue.y;
                if (newHeight < minSize.y)
                {

                    offsetValue.y -= (minSize.y- newHeight);
                }
                newSize = windowRec.size + new Vector2(offsetValue.x, -offsetValue.y);//�ƶ����������С
               
                newPos = windowRec.position + new Vector2(0, offsetValue.y);
                break;
            case DragDirection.RightBottom:

                newWidth = windowRec.size.x + offsetValue.x;

                //�жϴ����Ƿ����û��趨�ķ�Χ��
                if (newWidth < minSize.x)
                {

                    offsetValue.x += ( minSize.x- newWidth);
                }
                //������С��Χ��������һ��ƫ����
                newHeight = windowRec.size.y + offsetValue.y;
                if (newHeight < minSize.y)
                {

                    offsetValue.y += (minSize.y- newHeight);
                }

                //�µ�λ�ò���ı�
                newPos = windowRec.position;

                newSize = windowRec.size + new Vector2(offsetValue.x, offsetValue.y);//�ƶ����������С
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
    /// �϶���ʱ�������λ�����ķ�λ
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
        //�ָ�ΪϵͳĬ��ͼ��
        WindowsMouseCursor.SystemParametersInfo();
    }

    static DragDirection  cursorDirection = DragDirection.None;//��ʾһ�µ�ǰ�����ק�ķ���
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

