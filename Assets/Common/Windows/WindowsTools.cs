
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using UnityEngine.Events;
/// <summary>
/// ���õ�һЩ���ڹ���
/// </summary>

public class WindowsTools
{
    /// <summary>
    /// ����ָ�����ڵ���ʾ״̬�Լ��ָ�����С�������λ�á�
    /// </summary>
    /// <param name="hWnd"></param>
    /// <param name="lpwndpl">https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-windowplacement</param>
    /// <returns></returns>
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);




    /// <summary>
    /// ��ʾһ��ģʽ�Ի���
    /// </summary>
    /// <param name="handle">Ҫ��������Ϣ��������ߴ��ڵľ��</param>
    /// <param name="message">Ҫ��ʾ����Ϣ</param>
    /// <param name="title">����</param>
    /// <param name="type">https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-messagebox</param>
    /// <returns></returns>
    [DllImport("User32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern int MessageBox(IntPtr handle, string message, string title, int type);

    /// <summary>
    /// ��ָ���Ĵ��ڴ��� Z ˳��Ķ���
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-bringwindowtotop
    /// </summary>
    /// <param name="hWnd"></param>
    /// <returns>Ҫ��������Ϣ��������ߴ��ڵľ��</returns>
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool BringWindowToTop(IntPtr hWnd);


    /// <summary>
    /// ����Ļ�����м���������λ�á�
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getcursorpos
    /// </summary>
    /// <param name="lpPoint"></param>
    /// <returns></returns>
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetCursorPos(ref POINT lpPoint);

    /// <summary>
    /// ����ָ�����ڱ��������ı�
    /// </summary>
    /// <param name="hWnd">Ҫ��������Ϣ��������ߴ��ڵľ��</param>
    /// <param name="lpString">��������</param>
    /// <returns></returns>

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetWindowTextA(IntPtr hWnd, string lpString);


    /// <summary>
    /// ����ָ�����ڵı߽���εĳߴ硣�ߴ����������Ļ���Ͻǵ���Ļ���������
    /// </summary>
    /// <param name="hWnd">Ҫ��������Ϣ��������ߴ��ڵľ��</param>
    /// <param name="lpRect">https://docs.microsoft.com/en-us/windows/win32/api/windef/ns-windef-rect</param>
    /// <returns></returns>
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

    /// <summary>
    /// �������ڿͻ��������ꡣ�ͻ�����ָ���ͻ��������ϽǺ����½ǡ���Ϊ�ͻ���������ڴ��ڿͻ��������Ͻǣ��������Ͻǵ������� (0,0)��
    /// </summary>
    /// <param name="hWnd">Ҫ��������Ϣ��������ߴ��ڵľ��</param>
    /// <param name="lpRect">https://docs.microsoft.com/en-us/windows/win32/api/windef/ns-windef-rect</param>
    /// <returns></returns>

    [DllImport("user32.dll")]
    private static extern bool GetClientRect(IntPtr hWnd, ref RECT lpRect);



    // ��ȡ��ǰ���ڵĲ�������
    /// <summary>
    /// �����й�ָ�����ڵ���Ϣ���ú�������ָ��ƫ�������� 32 λ ( DWORD ) ֵ����������Ĵ����ڴ��С�
    /// </summary>
    /// <param name="hwd"></param>
    /// <param name="nIndex">https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getwindowlonga</param>
    /// <returns></returns>

    [DllImport("user32.dll")]
    public static extern long GetWindowLong(IntPtr hwd, int nIndex);



    /// <summary>
    /// ���õ�ǰ���ڵ���ʾ״̬  ���  ��С�� ��ԭ
    /// </summary>
    /// <param name="hwnd"></param>
    /// <param name="nCmdShow">https://docs.microsoft.com/en-us/windows/win32/winmsg/window-styles</param>
    /// <returns></returns>
    [DllImport("user32.dll")]
    public static extern bool ShowWindow(System.IntPtr hwnd, int nCmdShow);

    //���ҵ�ǰӦ�ö�Ӧ�Ĵ��ھ��
    /// <summary>
    /// ���������ʹ�������ָ���ַ���ƥ��Ķ������ڵľ��
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-findwindowa
    /// </summary>
    /// <param name="className"></param>
    /// <param name="windowName">�������ƣ����ڵı��⣩</param>
    /// <returns></returns>
    [DllImport("user32.dll", EntryPoint = "FindWindow")]
    public static extern IntPtr FindWindow(System.String className, System.String windowName);

    //���ô��ڱ߿�
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowlonga
    /// ����ָ�����ڵ����ԡ��ú�������ָ��ƫ�������� 32 λ�������ͣ�ֵ���õ�����Ĵ����ڴ��С�
    /// </summary>
    /// <param name="hwnd">Ҫ��������Ϣ��������ߴ��ڵľ��</param>
    /// <param name="_nIndex"></param>
    /// <param name="dwNewLong"></param>
    /// <returns></returns>
    [DllImport("user32.dll")]
    public static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, long dwNewLong);

    //���ô���λ�ã���С
    /// <summary>
    /// �����Ӵ��ڡ��������ڻ򶥼����ڵĴ�С��λ�ú� Z ˳��
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowpos
    /// </summary>
    /// <param name="hWnd">Ҫ��������Ϣ��������ߴ��ڵľ��</param>
    /// <param name="hWndInsertAfter"></param>
    /// <param name="X">����������λ�ã��Կͻ��������ʾ��</param>
    /// <param name="Y">���ڶ�������λ�ã��Կͻ��������ʾ��</param>
    /// <param name="cx">���ڵ��¿�ȣ�������Ϊ��λ��</param>
    /// <param name="cy">���ڵ��¸߶ȣ�������Ϊ��λ��</param>
    /// <param name="uFlags">���ڴ�С�Ͷ�λ��־��</param>
    /// <returns></returns>
    [DllImport("user32.dll")]
    public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);




    /// <summary>
    /// �ӵ�ǰ�߳��еĴ����ͷ���겶�񲢻ָ�������������봦��
    /// </summary>
    /// <returns></returns>
    [DllImport("user32.dll")]
    public static extern bool ReleaseCapture();
    /// <summary>
    /// ��ָ������Ϣ���͵�һ���������ڡ�
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendmessage
    /// </summary>
    /// <param name="hwnd">���ڹ��̽�������Ϣ�Ĵ��ھ����</param>
    /// <param name="wMsg">Ҫ���͵���Ϣ��</param>
    /// <param name="wParam">���ӵ���Ϣ�ض���Ϣ��</param>
    /// <param name="lParam">���ӵ���Ϣ�ض���Ϣ��</param>
    /// <returns></returns>
    [DllImport("user32.dll")]
    public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

    //������

    const int WS_CAPTION = 0x00c00000;

    //�߿����

    const int GWL_STYLE = -16;//�����µĴ�����ʽ
    const int SW_SHOWNORMAL = 1;//�ָ�Ϊԭ������
    const int SW_MAXIMIZE = 3;//��󻯴���
    const int SW_SHOWMINIMIZED = 2;//(��С������)

    const uint WS_VISIBLE = 0x10000000;//��߿���ʾ����

    const uint SWP_SHOWINDOW = 0x0040;//��ʾ����
    const int WS_POPUP = 0x800000;


    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWPLACEMENT
    {
        public uint length;
        public uint flags;
        public uint showCmd;//��ԭ��1   �����3   ��С����2
        public POINT ptMinPosition;
        public POINT ptMaxPosition;
        public RECT rcNormalPosition;
        public RECT rcDevice;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left; //��������
        public int Top; //��������
        public int Right; //��������
        public int Bottom; //��������
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
    /// �жϵ�ǰ�Ĵ����Ƿ������
    /// </summary>
    public static bool IsMax => GetWindowPlacement() == 3;



    private static IntPtr myWindows;
    /// <summary>
    /// ��ȡ��ǰӦ�õĴ��ھ��
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
    /// ��ȡ���ڵĲ���
    /// </summary>
    /// <returns></returns>
    public static Rect GetWindowRect()
    {
        RECT rect = new RECT();
        Rect targetRect = new Rect();
        GetWindowRect(GetForegroundWindow(), ref rect);
        targetRect.width = Mathf.Abs(rect.Right - rect.Left);
        targetRect.height = Mathf.Abs(rect.Top - rect.Bottom);

        //ê�������Ͻ�
        targetRect.x = rect.Left;
        targetRect.y = rect.Top;

        // Debug.LogError(targetRect.x + "  " + targetRect.y + "   " + targetRect.width + "   " + targetRect.height);
        return targetRect;

    }
    /// <summary>
    /// ԭʼ����
    /// </summary>
    /// <returns></returns>
    public static RECT GetWindowRECT()
    {
        RECT rect = new RECT();
       
        GetWindowRect(GetForegroundWindow(), ref rect);
       
     
        return rect;

    }




    //��С������
    public static void SetMinWindows()
    {

        ShowWindow(GetForegroundWindow(), SW_SHOWMINIMIZED);
        //���崰�ڲ�������
    }

    /// <summary>
    /// ����Ϊ��󴰿�
    /// </summary>
    public static void SetMaxWindows()
    {
       
        ShowWindow(GetForegroundWindow(), SW_MAXIMIZE);

    }
    /// <summary>
    /// ��ʾΪ��������
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
    /// �ö�Ӧ��
    /// </summary>
    public static void BringWindowToTop() {
        if (BringWindowToTop(GetForegroundWindow()))
        {
            Debug.LogError("�ö��ɹ�");
        }
        else { 
            Debug.LogError("�ö�ʧ��");

        }

    }

    const int SC_SIZE = 0xF000;
    const int WMSZ_BOTTOM = 6;//�ױ�
    const int WMSZ_BOTTOMLEFT = 7;//����
    const int WMSZ_BOTTOMRIGHT = 8;//����
    const int WMSZ_LEFT = 1;//���
    const int WMSZ_RIGHT = 2;//�ұ�
    const int WMSZ_TOP=3;//����
    const int WMSZ_TOPLEFT = 4;//���Ͻ�
    const int WMSZ_TOPRIGHT = 5;//���Ͻ�


    //https://docs.microsoft.com/en-us/windows/win32/menurc/wm-syscommand
    const int  WM_SYSCOMMAND = 0x0112;//���û�ѡ����󻯰�ť����С����ť���ָ���ť��رհ�ťʱ�����ڻ��յ�����Ϣ��

    


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
            Debug.LogError("�϶��ı�=="+commond);
            SendMessage(GetForegroundWindow(), WM_SYSCOMMAND, SC_SIZE |commond, 0);
           
        }
    }
    //�϶�����
    public static void DragWindow()
    {
        IntPtr hwnd = GetForegroundWindow();

        ReleaseCapture();

        SendMessage(hwnd, 0xA1, 0x02, 0);
        SendMessage(hwnd, 0x0202, 0, 0);
    }

}

