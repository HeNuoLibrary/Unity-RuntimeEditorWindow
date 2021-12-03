using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace UnityMainWin32
{
    public enum WindowsMessages : uint
    {
        DROPFILES = 0x0233
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        public int x;
        public int y;
        public Point(int pointX, int pointY)
        {
            x = pointX;
            y = pointY;
        }
        public override string ToString()
        {
            return "(" + x + ", " + y + ")";
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Message
    {
        public IntPtr hwnd;
        public WindowsMessages winMessage;
        public IntPtr wParam;
        public IntPtr lParam;
        public ushort time;
        public Point point;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
        public Rect(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
    }
    public static class UnityMainWin32DragAndDrop
    {
        public static readonly int GETMESSAGE = 3;

        public delegate IntPtr HookProcDelegate(int code, IntPtr wParam, ref Message lParam);
        public delegate bool ThreadProcDelegate(IntPtr hwnd, IntPtr lParam);

        public delegate void DroppedFilesEvent(List<string> filePaths, Point point);
        public static event DroppedFilesEvent OnDroppedFilesEvent;

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string moduleName);
        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int hookType, HookProcDelegate lpfn, IntPtr mod, uint dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnhookWindowsHookEx(IntPtr hook);

        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, ref Message lParam);

        [DllImport("shell32.dll")]
        public static extern void DragAcceptFiles(IntPtr hwnd, bool fAccept);
        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]

        public static extern uint DragQueryFile(IntPtr hDrop, uint iFile, System.Text.StringBuilder lpszFile, uint cch);
        [DllImport("shell32.dll")]
        public static extern void DragFinish(IntPtr hDrop);

        [DllImport("shell32.dll")]
        public static extern void DragQueryPoint(IntPtr hDrop, out Point pos);
        [DllImport("user32.dll")]
        public static extern bool EnumThreadWindows(uint dwThreadId, ThreadProcDelegate lpfn, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int maxCount);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        public static extern   bool IsZoomed(IntPtr hWnd);
        public const int SW_SHOWMINIMIZED = 2; //｛最小化, 激活｝  
        public const int SW_SHOWMAXIMIZED = 3;//最大化  
        public const int SW_SHOWRESTORE = 1;

        private static IntPtr hook;
        private static string unityClassName = "UnityWndClass";
        private static uint threadId;
        private static IntPtr mainWindow = IntPtr.Zero;

        /// <summary>
        /// 设置窗口状态: SW_SHOWRESTORE = 1 (正常);SW_SHOWMINIMIZED = 2(最小化);  SW_SHOWMAXIMIZED = 3
        /// </summary>
        /// <param name="status"></param>
        public static void SetUnityWindowStatus(int status)
        {
            ShowWindow(GetForegroundWindow(), status);
        }
        /// <summary>
        /// 窗口是否最大
        /// </summary>
        /// <returns></returns>
        public static bool IsWindowMaximized()
        {
            return IsZoomed(GetForegroundWindow());
        }
        public static string GetClassNameByWnd(IntPtr hWnd)
        {
            int capacity = 256;
            var stringBuilder = new StringBuilder(capacity);
            int count = GetClassName(hWnd, stringBuilder, capacity);
            return stringBuilder.ToString(0, count);
        }        
        [AOT.MonoPInvokeCallback(typeof(ThreadProcDelegate))]
        private static bool EnumCallback(IntPtr wnd, IntPtr lParam)
        {
            if (IsWindowVisible(wnd))
            {
                if((mainWindow == IntPtr.Zero || GetClassNameByWnd(wnd) == unityClassName))
                {
                    mainWindow = wnd;
                }            
            }
            return true;
        }

        public static void InitUnityDragAndDrop()
        {
            threadId = GetCurrentThreadId();
            if (threadId > 0)
            {
                EnumThreadWindows(threadId, EnumCallback, IntPtr.Zero);
            }        

            var module = GetModuleHandle(null);
            if(module !=IntPtr.Zero)
            {
                hook = SetWindowsHookEx(GETMESSAGE, WindowsCallback, module, threadId);
                DragAcceptFiles(mainWindow, true);
            }          
        }
        private static readonly uint dragQueryFileSize = 1024;

        [AOT.MonoPInvokeCallback(typeof(HookProcDelegate))]
        private static IntPtr WindowsCallback(int code, IntPtr wParam, ref Message lParam)
        {
            if (code == 0 && lParam.winMessage == WindowsMessages.DROPFILES)
            {
                Point point;
                DragQueryPoint(lParam.wParam, out point);
             
                uint filesCount = DragQueryFile(lParam.wParam, 0xFFFFFFFF, null, 0);
                var stringBuilder = new StringBuilder(1024);

                List<string> files = new List<string>();
                for (uint i = 0; i < filesCount; i++)
                {
                    int length = (int)DragQueryFile(lParam.wParam, i, stringBuilder, dragQueryFileSize);
                    files.Add(stringBuilder.ToString(0, length));
                    stringBuilder.Length = 0;
                }
                DragFinish(lParam.wParam);
                if (OnDroppedFilesEvent != null)
                {
                    OnDroppedFilesEvent(files, point);
                }
                  
            }
            return CallNextHookEx(hook, code, wParam, ref lParam);
        }
        public static void UnitUnityDragAndDrop()
        {
            UnhookWindowsHookEx(hook);
            hook = IntPtr.Zero;
            DragAcceptFiles(mainWindow, false);
        }
      
    }
}
