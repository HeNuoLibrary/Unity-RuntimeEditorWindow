using System;
using System.Runtime.InteropServices;

namespace Win32Api
{
    public delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    class WinUser32
    {
        // Ref:
        // https://docs.microsoft.com/zh-cn/windows/win32/winmsg/about-windows#desktop-window
        // https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-showwindow
        public const int SW_HIDE = 0;                               // 隐藏窗口，大小不变，激活状态不变
        public const int SW_MAXIMIZE = 3;                           // 最大化窗口，显示状态不变，激活状态不变
        public const int SW_SHOW = 5;                               // 在窗口原来的位置以原来的尺寸激活和显示窗口
        public const int SW_MINIMIZE = 6;                           // 最小化指定的窗口并且激活在Z序中的下一个顶层窗口
        public const int SW_RESTORE = 9;                            // 激活并显示窗口。如果窗口最小化或最大化，则系统将窗口恢复到原来的尺寸和位置。在恢复最小化窗口时，应用程序应该指定这个标志

        // https://docs.microsoft.com/zh-cn/windows/win32/api/winuser/nf-winuser-showwindow?redirectedfrom=MSDN
        public const int WM_CREATE = 0x0001;
        public const int WM_DESTROY = 0x0002;
        public const int WM_COMMAND = 0x0111;
        public const int WM_SYSCOMMAND = 0x0112;
        public const int WM_LBUTTONDOWN = 0x0201;                   // 左键
        public const int WM_LBUTTONDBLCLK = 0x0203;                 // 左键双击
        public const int WM_RBUTTONDOWN = 0x0204;                   // 右键
        public const int WM_RBUTTONDBLCLK = 0x0206;                 // 右键双击
        public const int WM_MBUTTONDOWN = 0x0207;                   // 中键

        public const int SC_CLOSE = 0xF060;
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_MINIMIZE = 0xF020;

        public const int GWL_EXSTYLE = -0x14;
        public const int WS_EX_TOOLWINDOW = 0x0080;
        public const int WS_EX_APPWINDOW = 0x00040000;


        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);
        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpszClass, string lpszTitle);

        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();
        [DllImport("User32.dll")]
        public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong32(HandleRef hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, IntPtr dwNewLong);
        [DllImport("user32.dll", EntryPoint = "DefWindowProcA")]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, uint wMsg, IntPtr wParam, IntPtr lParam);
        // 将消息信息传递给指定的窗口过程
        [DllImport("user32.dll")]
        public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern bool IsIconic(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool IsZoomed(IntPtr hWnd);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public static IntPtr GetWindow(string titleOrClassname)
        {
            IntPtr hWnd = FindWindow(null, titleOrClassname); ;
            if (hWnd == IntPtr.Zero)
            {
                hWnd = FindWindow(titleOrClassname, null);
            }

            return hWnd;
        }

        public static IntPtr SetWindowLongPtr(HandleRef hWnd, int nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size == 8)
                return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            else
            {
                return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
            }
        }

        // 展示任务栏
        public static void ShowTaskWnd()
        {
            ShowWindowAsync(FindWindow("Shell_TrayWnd", null), SW_RESTORE);
        }

        // 隐藏任务栏
        public static void HideTaskWnd()
        {
            ShowWindowAsync(FindWindow("Shell_TrayWnd", null), SW_HIDE);
        }

        // 展示任务栏上的图标
        public static void ShowTaskIcon(string titleOrClassname)
        {
            // https://stackoverflow.com/questions/1462504/how-to-make-window-appear-in-taskbar
            IntPtr mainWindIntPtr = GetWindow(titleOrClassname);
            if (mainWindIntPtr != IntPtr.Zero)
            {
                HandleRef pMainWindow = new HandleRef(null, mainWindIntPtr);
                SetWindowLongPtr(pMainWindow, GWL_EXSTYLE, (IntPtr)(GetWindowLong(mainWindIntPtr, GWL_EXSTYLE).ToInt32() | WS_EX_APPWINDOW));

                ShowWindowAsync(mainWindIntPtr, SW_HIDE);
                ShowWindowAsync(mainWindIntPtr, SW_SHOW);
            }
        }

        // 隐藏任务栏上的图标
        public static void HideTaskIcon(string titleOrClassname)
        {
            // https://forum.unity.com/threads/can-the-taskbar-icon-of-a-unity-game-be-hidden.888625/?_ga=2.191055082.1747733629.1614429624-1257832814.1586182347#post-5838658
            // https://docs.microsoft.com/en-us/windows/win32/shell/taskbar#managing-taskbar-buttons
            IntPtr mainWindIntPtr = GetWindow(titleOrClassname);
            if (mainWindIntPtr != IntPtr.Zero)
            {
                HandleRef pMainWindow = new HandleRef(null, mainWindIntPtr);
                SetWindowLongPtr(pMainWindow, GWL_EXSTYLE, (IntPtr)(GetWindowLong(mainWindIntPtr, GWL_EXSTYLE).ToInt32() | WS_EX_TOOLWINDOW));

                ShowWindowAsync(mainWindIntPtr, SW_HIDE);
                ShowWindowAsync(mainWindIntPtr, SW_SHOW);
            }
        }

        #region 创建菜单

        public const uint WM_DRAWITEM = 0x002b;
        public const uint WM_MEASUREITEM = 0x002c;

        [Flags]
        public enum MenuFlags : uint
        {
            MF_STRING = 0,
            MF_BYPOSITION = 0x400,
            MF_SEPARATOR = 0x800,
            MF_REMOVE = 0x1000,
        }

        // http://www.pinvoke.net/default.aspx/user32/CreatePopupMenu.html
        [DllImport("user32")]
        public static extern IntPtr CreatePopupMenu();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool AppendMenu(IntPtr hMenu, MenuFlags uFlags, uint uIDNewItem, string lpNewItem);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static  extern bool DestroyMenu(IntPtr hMenu);

        #endregion

        #region 弹出菜单

        [DllImport("user32.dll")]
        public static extern bool TrackPopupMenuEx(IntPtr hmenu, uint fuFlags, int x, int y,
            IntPtr hwnd, IntPtr lptpm);

        #endregion

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            internal int Left;
            internal int Top;
            internal int Right;
            internal int Bottom;
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        // https://improve.dk/modifying-window-location-and-size/
        // The SetWindowPos function is used to both resize and change the location of windows. The uFlags parameter
        // can take any number of flags, with zero being a neutral flag, the same goes for the hWndInsertAfter parameter.
        // X, Y is the new location of the window, cx and cy is the new height / width of the window. Via uFlags it can
        // be set to ignore the new location and/or the new size of the window.
        // See http://msdn2.microsoft.com/en-us/library/ms633545.aspx for full documentation.
        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        // An enumeration containing all the possible HWND values. Window handles (HWND) used for hWndInsertAfter
        public enum HWND : int
        {
            TOP = 0,                                // 在前面
            BOTTOM = 1,                             // 在后面
            TOPMOST = -1,                           // 在前面, 位于任何顶部窗口的前面
            NOTOPMOST = -2                          // 在前面, 位于其他顶部窗口的后面
        }                                           
                                                    
        // And enumeration containing all the possible SWP values. SetWindowPos Flags
        public enum SWP : uint                      
        {                                           // 
            ASYNCWINDOWPOS = 0x4000,                // 
            DEFERERASE = 0x2000,                    // 
            FRAMECHANGED = 0x0020,                  // 强制发送 WM_NCCALCSIZE 消息, 一般只是在改变大小时才发送此消息
            HIDEWINDOW = 0x0080,                    // 
            NOACTIVATE = 0x0010,                    // 不激活
            NOCOPYBITS = 0x0100,                    // 
            NOMOVE = 0x0002,                        // 忽略 X、Y, 不改变位置
            NOOWNERZORDER = 0x0200,                 // 
            NOREDRAW = 0x0008,                      // 不重绘
            NOSENDCHANGING = 0x0400,                // 
            NOSIZE = 0x0001,                        // 忽略 cx、cy, 保持大小
            NOZORDER = 0x0004,                      // 忽略 hWndInsertAfter, 保持 Z 顺序
            SHOWWINDOW = 0x0040                     // 
        }

        public enum DwmWindowAttribute
        {
            DWMWA_NCRENDERING_ENABLED = 1,
            DWMWA_NCRENDERING_POLICY,
            DWMWA_TRANSITIONS_FORCEDISABLED,
            DWMWA_ALLOW_NCPAINT,
            DWMWA_CAPTION_BUTTON_BOUNDS,
            DWMWA_NONCLIENT_RTL_LAYOUT,
            DWMWA_FORCE_ICONIC_REPRESENTATION,
            DWMWA_FLIP3D_POLICY,
            DWMWA_EXTENDED_FRAME_BOUNDS,
            DWMWA_HAS_ICONIC_BITMAP,
            DWMWA_DISALLOW_PEEK,
            DWMWA_EXCLUDED_FROM_PEEK,
            DWMWA_CLOAK,
            DWMWA_CLOAKED,
            DWMWA_FREEZE_REPRESENTATION,
            DWMWA_LAST
        };

        [DllImport("dwmapi.dll")]
        public static extern int DwmGetWindowAttribute(IntPtr hWnd, DwmWindowAttribute dwAttribute, out RECT lpRect, int cbAttribute);
        [DllImport("dwmapi.dll", PreserveSig = true)]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, DwmWindowAttribute attr, IntPtr attrValue, int attrSize);

        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(SystemMetric smIndex);

        public enum SystemMetric
        {
            SM_CXSCREEN = 0,  // 0x00
            SM_CYSCREEN = 1,  // 0x01
            SM_CXVSCROLL = 2,  // 0x02
            SM_CYHSCROLL = 3,  // 0x03
            SM_CYCAPTION = 4,  // 0x04
            SM_CXBORDER = 5,  // 0x05
            SM_CYBORDER = 6,  // 0x06
            SM_CXDLGFRAME = 7,  // 0x07
            SM_CXFIXEDFRAME = 7,  // 0x07
            SM_CYDLGFRAME = 8,  // 0x08
            SM_CYFIXEDFRAME = 8,  // 0x08
            SM_CYVTHUMB = 9,  // 0x09
            SM_CXHTHUMB = 10, // 0x0A
            SM_CXICON = 11, // 0x0B
            SM_CYICON = 12, // 0x0C
            SM_CXCURSOR = 13, // 0x0D
            SM_CYCURSOR = 14, // 0x0E
            SM_CYMENU = 15, // 0x0F
            SM_CXFULLSCREEN = 16, // 0x10
            SM_CYFULLSCREEN = 17, // 0x11
            SM_CYKANJIWINDOW = 18, // 0x12
            SM_MOUSEPRESENT = 19, // 0x13
            SM_CYVSCROLL = 20, // 0x14
            SM_CXHSCROLL = 21, // 0x15
            SM_DEBUG = 22, // 0x16
            SM_SWAPBUTTON = 23, // 0x17
            SM_CXMIN = 28, // 0x1C
            SM_CYMIN = 29, // 0x1D
            SM_CXSIZE = 30, // 0x1E
            SM_CYSIZE = 31, // 0x1F
            SM_CXSIZEFRAME = 32, // 0x20
            SM_CXFRAME = 32, // 0x20
            SM_CYSIZEFRAME = 33, // 0x21
            SM_CYFRAME = 33, // 0x21
            SM_CXMINTRACK = 34, // 0x22
            SM_CYMINTRACK = 35, // 0x23
            SM_CXDOUBLECLK = 36, // 0x24
            SM_CYDOUBLECLK = 37, // 0x25
            SM_CXICONSPACING = 38, // 0x26
            SM_CYICONSPACING = 39, // 0x27
            SM_MENUDROPALIGNMENT = 40, // 0x28
            SM_PENWINDOWS = 41, // 0x29
            SM_DBCSENABLED = 42, // 0x2A
            SM_CMOUSEBUTTONS = 43, // 0x2B
            SM_SECURE = 44, // 0x2C
            SM_CXEDGE = 45, // 0x2D
            SM_CYEDGE = 46, // 0x2E
            SM_CXMINSPACING = 47, // 0x2F
            SM_CYMINSPACING = 48, // 0x30
            SM_CXSMICON = 49, // 0x31
            SM_CYSMICON = 50, // 0x32
            SM_CYSMCAPTION = 51, // 0x33
            SM_CXSMSIZE = 52, // 0x34
            SM_CYSMSIZE = 53, // 0x35
            SM_CXMENUSIZE = 54, // 0x36
            SM_CYMENUSIZE = 55, // 0x37
            SM_ARRANGE = 56, // 0x38
            SM_CXMINIMIZED = 57, // 0x39
            SM_CYMINIMIZED = 58, // 0x3A
            SM_CXMAXTRACK = 59, // 0x3B
            SM_CYMAXTRACK = 60, // 0x3C
            SM_CXMAXIMIZED = 61, // 0x3D
            SM_CYMAXIMIZED = 62, // 0x3E
            SM_NETWORK = 63, // 0x3F
            SM_CLEANBOOT = 67, // 0x43
            SM_CXDRAG = 68, // 0x44
            SM_CYDRAG = 69, // 0x45
            SM_SHOWSOUNDS = 70, // 0x46
            SM_CXMENUCHECK = 71, // 0x47
            SM_CYMENUCHECK = 72, // 0x48
            SM_SLOWMACHINE = 73, // 0x49
            SM_MIDEASTENABLED = 74, // 0x4A
            SM_MOUSEWHEELPRESENT = 75, // 0x4B
            SM_XVIRTUALSCREEN = 76, // 0x4C
            SM_YVIRTUALSCREEN = 77, // 0x4D
            SM_CXVIRTUALSCREEN = 78, // 0x4E
            SM_CYVIRTUALSCREEN = 79, // 0x4F
            SM_CMONITORS = 80, // 0x50
            SM_SAMEDISPLAYFORMAT = 81, // 0x51
            SM_IMMENABLED = 82, // 0x52
            SM_CXFOCUSBORDER = 83, // 0x53
            SM_CYFOCUSBORDER = 84, // 0x54
            SM_TABLETPC = 86, // 0x56
            SM_MEDIACENTER = 87, // 0x57
            SM_STARTER = 88, // 0x58
            SM_SERVERR2 = 89, // 0x59
            SM_MOUSEHORIZONTALWHEELPRESENT = 91, // 0x5B
            SM_CXPADDEDBORDER = 92, // 0x5C
            SM_DIGITIZER = 94, // 0x5E
            SM_MAXIMUMTOUCHES = 95, // 0x5F

            SM_REMOTESESSION = 0x1000, // 0x1000
            SM_SHUTTINGDOWN = 0x2000, // 0x2000
            SM_REMOTECONTROL = 0x2001, // 0x2001


            SM_CONVERTIBLESLATEMODE = 0x2003,
            SM_SYSTEMDOCKED = 0x2004,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(out POINT lpPoint);

    }
}
