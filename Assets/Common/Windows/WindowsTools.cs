
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using UnityEngine.Events;
/// <summary>
/// 常用的一些窗口功能
/// </summary>

public class WindowsTools
{
    /// <summary>
    /// 检索指定窗口的显示状态以及恢复、最小化和最大化位置。
    /// </summary>
    /// <param name="hWnd"></param>
    /// <param name="lpwndpl">https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-windowplacement</param>
    /// <returns></returns>
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);




    /// <summary>
    /// 显示一个模式对话框
    /// </summary>
    /// <param name="handle">要创建的消息框的所有者窗口的句柄</param>
    /// <param name="message">要显示的消息</param>
    /// <param name="title">标题</param>
    /// <param name="type">https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-messagebox</param>
    /// <returns></returns>
    [DllImport("User32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern int MessageBox(IntPtr handle, string message, string title, int type);

    /// <summary>
    /// 将指定的窗口带到 Z 顺序的顶部
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-bringwindowtotop
    /// </summary>
    /// <param name="hWnd"></param>
    /// <returns>要创建的消息框的所有者窗口的句柄</returns>
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool BringWindowToTop(IntPtr hWnd);


    /// <summary>
    /// 在屏幕坐标中检索鼠标光标的位置。
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getcursorpos
    /// </summary>
    /// <param name="lpPoint"></param>
    /// <returns></returns>
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetCursorPos(ref POINT lpPoint);

    /// <summary>
    /// 更改指定窗口标题栏的文本
    /// </summary>
    /// <param name="hWnd">要创建的消息框的所有者窗口的句柄</param>
    /// <param name="lpString">标题内容</param>
    /// <returns></returns>

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetWindowTextA(IntPtr hWnd, string lpString);


    /// <summary>
    /// 检索指定窗口的边界矩形的尺寸。尺寸以相对于屏幕左上角的屏幕坐标给出。
    /// </summary>
    /// <param name="hWnd">要创建的消息框的所有者窗口的句柄</param>
    /// <param name="lpRect">https://docs.microsoft.com/en-us/windows/win32/api/windef/ns-windef-rect</param>
    /// <returns></returns>
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

    /// <summary>
    /// 检索窗口客户区的坐标。客户坐标指定客户区的左上角和右下角。因为客户坐标相对于窗口客户区的左上角，所以左上角的坐标是 (0,0)。
    /// </summary>
    /// <param name="hWnd">要创建的消息框的所有者窗口的句柄</param>
    /// <param name="lpRect">https://docs.microsoft.com/en-us/windows/win32/api/windef/ns-windef-rect</param>
    /// <returns></returns>

    [DllImport("user32.dll")]
    private static extern bool GetClientRect(IntPtr hWnd, ref RECT lpRect);



    // 获取当前窗口的参数设置
    /// <summary>
    /// 检索有关指定窗口的信息。该函数还将指定偏移量处的 32 位 ( DWORD ) 值检索到额外的窗口内存中。
    /// </summary>
    /// <param name="hwd"></param>
    /// <param name="nIndex">https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getwindowlonga</param>
    /// <returns></returns>

    [DllImport("user32.dll")]
    public static extern long GetWindowLong(IntPtr hwd, int nIndex);



    /// <summary>
    /// 设置当前窗口的显示状态  最大化  最小化 还原
    /// </summary>
    /// <param name="hwnd"></param>
    /// <param name="nCmdShow">https://docs.microsoft.com/en-us/windows/win32/winmsg/window-styles</param>
    /// <returns></returns>
    [DllImport("user32.dll")]
    public static extern bool ShowWindow(System.IntPtr hwnd, int nCmdShow);

    //查找当前应用对应的窗口句柄
    /// <summary>
    /// 检索类名和窗口名与指定字符串匹配的顶级窗口的句柄
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-findwindowa
    /// </summary>
    /// <param name="className"></param>
    /// <param name="windowName">窗口名称（窗口的标题）</param>
    /// <returns></returns>
    [DllImport("user32.dll", EntryPoint = "FindWindow")]
    public static extern IntPtr FindWindow(System.String className, System.String windowName);

    //设置窗口边框
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowlonga
    /// 更改指定窗口的属性。该函数还将指定偏移量处的 32 位（长整型）值设置到额外的窗口内存中。
    /// </summary>
    /// <param name="hwnd">要创建的消息框的所有者窗口的句柄</param>
    /// <param name="_nIndex"></param>
    /// <param name="dwNewLong"></param>
    /// <returns></returns>
    [DllImport("user32.dll")]
    public static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, long dwNewLong);

    //设置窗口位置，大小
    /// <summary>
    /// 更改子窗口、弹出窗口或顶级窗口的大小、位置和 Z 顺序
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowpos
    /// </summary>
    /// <param name="hWnd">要创建的消息框的所有者窗口的句柄</param>
    /// <param name="hWndInsertAfter"></param>
    /// <param name="X">窗口左侧的新位置，以客户端坐标表示。</param>
    /// <param name="Y">窗口顶部的新位置，以客户端坐标表示。</param>
    /// <param name="cx">窗口的新宽度，以像素为单位。</param>
    /// <param name="cy">窗口的新高度，以像素为单位。</param>
    /// <param name="uFlags">窗口大小和定位标志。</param>
    /// <returns></returns>
    [DllImport("user32.dll")]
    public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);




    /// <summary>
    /// 从当前线程中的窗口释放鼠标捕获并恢复正常的鼠标输入处理。
    /// </summary>
    /// <returns></returns>
    [DllImport("user32.dll")]
    public static extern bool ReleaseCapture();
    /// <summary>
    /// 将指定的消息发送到一个或多个窗口。
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendmessage
    /// </summary>
    /// <param name="hwnd">窗口过程将接收消息的窗口句柄。</param>
    /// <param name="wMsg">要发送的消息。</param>
    /// <param name="wParam">附加的消息特定信息。</param>
    /// <param name="lParam">附加的消息特定信息。</param>
    /// <returns></returns>
    [DllImport("user32.dll")]
    public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

    //标题栏

    const int WS_CAPTION = 0x00c00000;

    //边框参数

    const int GWL_STYLE = -16;//设置新的窗口样式
    const int SW_SHOWNORMAL = 1;//恢复为原来窗口
    const int SW_MAXIMIZE = 3;//最大化窗口
    const int SW_SHOWMINIMIZED = 2;//(最小化窗口)

    const uint WS_VISIBLE = 0x10000000;//外边框显示隐藏

    const uint SWP_SHOWINDOW = 0x0040;//显示窗口
    const int WS_POPUP = 0x800000;


    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWPLACEMENT
    {
        public uint length;
        public uint flags;
        public uint showCmd;//还原是1   最大化是3   最小化是2
        public POINT ptMinPosition;
        public POINT ptMaxPosition;
        public RECT rcNormalPosition;
        public RECT rcDevice;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left; //最左坐标
        public int Top; //最上坐标
        public int Right; //最右坐标
        public int Bottom; //最下坐标
    }
    /// <summary>
    /// https://docs.microsoft.com/en-us/previous-versions/dd162805(v=vs.85)
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;
    }

    /// <summary>
    /// 判断当前的窗口是否是最大化
    /// </summary>
    public static bool IsMax => GetWindowPlacement() == 3;



    private static IntPtr myWindows;
    /// <summary>
    /// 获取当前应用的窗口句柄
    /// </summary>
    /// <returns></returns>
    public static IntPtr GetForegroundWindow()
    {
        if (myWindows==IntPtr.Zero) {

            myWindows= FindWindow(null, UnityEngine.Application.productName);
        }
        return myWindows;   
    }
    public static void HideTitle()
    {

        IntPtr window = GetForegroundWindow();

        var wl = GetWindowLong(window, GWL_STYLE);
        wl &= ~WS_CAPTION& WS_VISIBLE;
        SetWindowLong(window, GWL_STYLE, wl);

    }
    const int WS_BORDER = 0x00800000;
    public static void ShowTitle()
    {
        var wl = GetWindowLong(GetForegroundWindow(), GWL_STYLE);
        wl |= WS_CAPTION;
        SetWindowLong(GetForegroundWindow(), GWL_STYLE, wl& WS_BORDER);
    }


    public static uint GetWindowPlacement() {

        if (GetWindowPlacement(GetForegroundWindow(),out WINDOWPLACEMENT result)) {
            //Debug.LogError("result.showCmd==" + result.showCmd);
        }
        
        return result.showCmd;
    }


    /// <summary>
    /// 获取窗口的参数
    /// </summary>
    /// <returns></returns>
    public static Rect GetWindowRect()
    {
        RECT rect = new RECT();
        Rect targetRect = new Rect();
        GetWindowRect(GetForegroundWindow(), ref rect);
        targetRect.width = Mathf.Abs(rect.Right - rect.Left);
        targetRect.height = Mathf.Abs(rect.Top - rect.Bottom);

        //锚点在左上角
        targetRect.x = rect.Left;
        targetRect.y = rect.Top;

        // Debug.LogError(targetRect.x + "  " + targetRect.y + "   " + targetRect.width + "   " + targetRect.height);
        return targetRect;

    }
    /// <summary>
    /// 原始数据
    /// </summary>
    /// <returns></returns>
    public static RECT GetWindowRECT()
    {
        RECT rect = new RECT();
       
        GetWindowRect(GetForegroundWindow(), ref rect);
       
     
        return rect;

    }




    //最小化窗口
    public static void SetMinWindows()
    {

        ShowWindow(GetForegroundWindow(), SW_SHOWMINIMIZED);
        //具体窗口参数看这
    }

    /// <summary>
    /// 设置为最大窗口
    /// </summary>
    public static void SetMaxWindows()
    {
       
        ShowWindow(GetForegroundWindow(), SW_MAXIMIZE);

    }
    /// <summary>
    /// 显示为正常窗口
    /// </summary>
    public static void SetNormalWindow()
    {
        ShowWindow(GetForegroundWindow(), SW_SHOWNORMAL);
    }
    

    public static void SetWindowPos(Rect rect)
    {
        SetWindowPos(GetForegroundWindow(), 0, (int)rect.position.x, (int)rect.position.y, (int)rect.size.x, (int)rect.size.y, SWP_SHOWINDOW);
    }


   

    public static Vector2 GetCursorPos()
    {
        Vector2 mousePos = new Vector2();
        POINT point = new POINT();
        if (GetCursorPos(ref point))
        {
            mousePos = new Vector2(point.X, point.Y);
           
        }
        return mousePos;
    }
 
    public static void  ShowMessageBox(string message, string title,UnityAction<bool> result) {
      int re= MessageBox(GetForegroundWindow(),  message,  title, 1);
        Debug.LogError(re);
        result.Invoke(re==1);
    }


    public static void SetWindowText(string titel) {
        SetWindowTextA(GetForegroundWindow(),titel);
    }

    /// <summary>
    /// 置顶应用
    /// </summary>
    public static void BringWindowToTop() {
        if (BringWindowToTop(GetForegroundWindow()))
        {
            Debug.LogError("置顶成功");
        }
        else { 
            Debug.LogError("置顶失败");

        }

    }

    const int SC_SIZE = 0xF000;
    const int WMSZ_BOTTOM = 6;//底边
    const int WMSZ_BOTTOMLEFT = 7;//左下
    const int WMSZ_BOTTOMRIGHT = 8;//右下
    const int WMSZ_LEFT = 1;//左边
    const int WMSZ_RIGHT = 2;//右边
    const int WMSZ_TOP=3;//顶边
    const int WMSZ_TOPLEFT = 4;//左上角
    const int WMSZ_TOPRIGHT = 5;//右上角


    //https://docs.microsoft.com/en-us/windows/win32/menurc/wm-syscommand
    const int  WM_SYSCOMMAND = 0x0112;//当用户选择最大化按钮、最小化按钮、恢复按钮或关闭按钮时，窗口会收到此消息。

    


    public static void MoveWindow(DragDirection dragDirection) {
        int commond = -1;
        switch (dragDirection)
        {
            case DragDirection.None:
                break;
            case DragDirection.Left:
                commond = WMSZ_LEFT;
                break;
            case DragDirection.Top:
                commond = WMSZ_TOP;

                break;
            case DragDirection.Right:
                commond = WMSZ_RIGHT;

                break;
            case DragDirection.Bottom:
                commond = WMSZ_BOTTOM;

                break;
            case DragDirection.LeftTop:
                commond = WMSZ_TOPLEFT;

                break;
            case DragDirection.LeftBottom:
                commond = WMSZ_BOTTOMLEFT;

                break;
            case DragDirection.RightTop:
                commond = WMSZ_TOPRIGHT;

                break;
            case DragDirection.RightBottom:
                commond = WMSZ_BOTTOMRIGHT;

                break;
            default:
                break;
        }
        if (commond!=-1) {
            Debug.LogError("拖动的边=="+commond);
            SendMessage(GetForegroundWindow(), WM_SYSCOMMAND, SC_SIZE |commond, 0);
           
        }
    }
    //拖动窗口
    public static void DragWindow()
    {
        IntPtr hwnd = GetForegroundWindow();

        ReleaseCapture();

        SendMessage(hwnd, 0xA1, 0x02, 0);
        SendMessage(hwnd, 0x0202, 0, 0);
    }

}

