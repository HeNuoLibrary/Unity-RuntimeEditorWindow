
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using UnityEngine.Events;
/// <summary>
/// 监听Windows下鼠标按键事件
/// </summary>
public class WindowsMouseButtonEvent
{

    private static WindowsMouseButtonEvent instance;

    public static  WindowsMouseButtonEvent Instance
    {
        get {
            if (instance == null)
            {
                instance = new WindowsMouseButtonEvent();


            }
            return instance;
        }
        
    }
    public UnityAction<MouseButtons> mouseButtonEvent;
    public enum MouseButtons
    {
        LeftButtonDown,
        LeftButtonUp,
        RightButtonDown,
        RightButtonUp,
        MiddleButtonDown,
        MiddleButtonUp,
        MouseMove,

    }

    [StructLayout(LayoutKind.Sequential)]
    public class POINT
    {
        public int x;
        public int y;
    }
    [StructLayout(LayoutKind.Sequential)]
    public class MouseHookStruct
    {
        public POINT pt;
        public int hwnd;
        public int wHitTestCode;
        public int dwExtraInfo;
    }
    public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);
    //安装钩子
    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);
    //卸载钩子
    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern bool UnhookWindowsHookEx(int idHook);
    //调用下一个钩子
    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);


    public const int WH_MOUSE_LL = 14;
    public HookProc hProc;
    private int hHook;
    private static int hMouseHook = 0;
    private const int WM_MOUSEMOVE = 0x200;
    private const int WM_LBUTTONDOWN = 0x201;
    private const int WM_RBUTTONDOWN = 0x204;
    private const int WM_MBUTTONDOWN = 0x207;
    private const int WM_LBUTTONUP = 0x202;
    private const int WM_RBUTTONUP = 0x205;
    private const int WM_MBUTTONUP = 0x208;
    private const int WM_LBUTTONDBLCLK = 0x203;
    private const int WM_RBUTTONDBLCLK = 0x206;
    private const int WM_MBUTTONDBLCLK = 0x209;

   


    public int SetHook(UnityAction<MouseButtons> mouseButtonEvent)
    {
        if (this.mouseButtonEvent==null) {
            hProc = new HookProc(MouseHookProc);
            hHook = SetWindowsHookEx(WH_MOUSE_LL, hProc, IntPtr.Zero, 0);
        }

        //目前只支持一个
        this.mouseButtonEvent = mouseButtonEvent;

       
        return hHook;
    }

    
    public void UnHook()
    {
        UnhookWindowsHookEx(hHook);
        hProc = null;
    }
    private int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
    {
        MouseHookStruct MyMouseHookStruct = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));
        if (nCode < 0)
        {
            return CallNextHookEx(hHook, nCode, wParam, lParam);
        }
        else
        {


            switch ((Int32)wParam)
            {
                case WM_LBUTTONDOWN:
                    mouseButtonEvent.Invoke(MouseButtons.LeftButtonDown);
                   
                    break;
                case WM_RBUTTONDOWN:
                    mouseButtonEvent.Invoke(MouseButtons.RightButtonDown);

                   
                    break;
                case WM_MBUTTONDOWN:
                    mouseButtonEvent.Invoke(MouseButtons.MiddleButtonDown);

                    break;
                case WM_LBUTTONUP:
                    mouseButtonEvent.Invoke(MouseButtons.LeftButtonUp);

                    break;
                case WM_RBUTTONUP:
                    mouseButtonEvent.Invoke(MouseButtons.RightButtonUp);

                    break;
                case WM_MBUTTONUP:
                    mouseButtonEvent.Invoke(MouseButtons.MiddleButtonUp);

                    break;

                case WM_MOUSEMOVE:
                    mouseButtonEvent.Invoke(MouseButtons.MouseMove);

                    break;
            }


            return CallNextHookEx(hHook, nCode, wParam, lParam);

        }
    }



}
