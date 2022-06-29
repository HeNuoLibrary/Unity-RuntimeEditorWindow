using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Win32Api
{
    public class Shell_NotifyIconEx
    {
        // https://www.codeproject.com/articles/4972/shell-notifyiconex-with-balloon-tooltip

        // 注意:此组件由于重点在气泡提示，它要求Shell32.dll 5.0(ie 5.0) 以上

        private readonly IntPtr formTmpHwnd = IntPtr.Zero;
        // 这是一个由VersionPass 返回的属性，它允许开发者检测当前机子的Shell32.dll(可能在win95 或未知平台上版本) 合适此组，不符则用.net 自己的notifyicon
        // 这是一个私有标志，它允许开发者在程序退出时忘记调用DelNotifyBox 来清除图标时会自动在析构里清掉它。
        private bool forgetDelNotifyBox = false;

        // 这是调用此组件的主窗口句柄（当前实例有效，可多个icon 不冲突）
        internal IntPtr formHwnd = IntPtr.Zero;
        // 这是菜单的句柄（当前实例有效，可多个icon 不冲突）
        internal IntPtr contextMenuHwnd = IntPtr.Zero;

        public Shell_NotifyIconEx(IntPtr windHandle) // 构造
        {
            //WM_NOTIFY_TRAY += 1; // 消息ID +1，避免多个ICON 消息处理冲突
            //uID += 1; // 同上
            //formTmp = new InnerClass(this); // 新实例一个消息循环
            //formTmpHwnd = formTmp.Handle; // 新实例句柄
            formTmpHwnd = windHandle;
        }

        ~Shell_NotifyIconEx()
        {
            if (forgetDelNotifyBox)
                this.DelNotifyBox(); //如果开发者忘记则清理icon
        }

        #region API_Consts
        public const int WM_NOTIFY_TRAY = 0x0400 + 2001;
        public const int uID = 5000;

        // 常数定义，有VC 的可以参见 shellapi.h
        private const int NIIF_NONE = 0x00;
        private const int NIIF_INFO = 0x01;
        private const int NIIF_WARNING = 0x02;
        private const int NIIF_ERROR = 0x03;

        private const int NIF_MESSAGE = 0x01;
        private const int NIF_ICON = 0x02;
        private const int NIF_TIP = 0x04;
        private const int NIF_STATE = 0x08;
        private const int NIF_INFO = 0x10;

        private const int NIM_ADD = 0x00;
        private const int NIM_MODIFY = 0x01;
        private const int NIM_DELETE = 0x02;
        private const int NIM_SETFOCUS = 0x03;
        private const int NIM_SETVERSION = 0x04;

        private const int NIS_HIDDEN = 0x01;
        private const int NIS_SHAREDICON = 0x02;

        private const int NOTIFYICON_OLDVERSION = 0x00;
        private const int NOTIFYICON_VERSION = 0x03;

        [DllImport("shell32.dll", EntryPoint = "Shell_NotifyIcon", CharSet = CharSet.Unicode)]
        private static extern bool Shell_NotifyIcon(int dwMessage, ref NOTIFYICONDATA lpData);

        /// <summary>
        /// 此API 的作用是当 this.focus() 无效时可以考虑使用，效果很好
        /// </summary>
        /// <param name="hwnd">this.Handle, 当前窗体句柄</param>
        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern int SetForegroundWindow(
         IntPtr hwnd
        );

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr ExtractAssociatedIcon(IntPtr hInst, StringBuilder lpIconPath,
            out ushort lpiIcon);
        [DllImport("shell32.dll")]
        static extern IntPtr ExtractIcon(IntPtr hInst, string lpszExeFileName, int nIconIndex);

        // 注意一定要指定字符集为Unicode，否则气泡内容不能支持中文
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct NOTIFYICONDATA
        {
            internal int cbSize;
            internal IntPtr hwnd;
            internal int uID;
            internal int uFlags;
            internal int uCallbackMessage;
            internal IntPtr hIcon;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            internal string szTip;
            internal int dwState; // 这里往下几个是 5.0 的精华
            internal int dwStateMask;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            internal string szInfo;
            internal int uTimeoutAndVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            internal string szInfoTitle;
            internal int dwInfoFlags;
        }

        // http://www.pinvoke.net/default.aspx/user32/LoadIcon.html
        [DllImport("user32.dll")]
        //static extern IntPtr LoadIcon(IntPtr hInstance, string lpIconName);
        public static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr lpIconName);
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string moduleName);

        public enum SystemIcons
        {
            IDI_APPLICATION = 32512,
            IDI_HAND = 32513,
            IDI_QUESTION = 32514,
            IDI_EXCLAMATION = 32515,
            IDI_ASTERISK = 32516,
            IDI_WINLOGO = 32517,
            IDI_WARNING = IDI_EXCLAMATION,
            IDI_ERROR = IDI_HAND,
            IDI_INFORMATION = IDI_ASTERISK,
        }
        #endregion

        /// <summary>
        /// 建一个结构
        /// </summary>
        private NOTIFYICONDATA GetNOTIFYICONDATA(IntPtr iconHwnd, string sTip, string boxTitle, string boxText)
        {
            NOTIFYICONDATA nData = new NOTIFYICONDATA();
            // 结构的大小
            nData.cbSize = Marshal.SizeOf(nData);
            // 处理消息循环的窗体句柄，可以移成主窗体
            nData.hwnd = formTmpHwnd;
            // 消息的 WParam，回调时用
            nData.uID = uID;
            // 标志，表示由消息、图标、提示、信息组成
            nData.uFlags = NIF_MESSAGE | NIF_ICON | NIF_TIP | NIF_INFO;
            // 消息ID，回调用
            nData.uCallbackMessage = WM_NOTIFY_TRAY;
            if (iconHwnd != IntPtr.Zero)
            {
                nData.hIcon = iconHwnd;
            }
            else
            {
                // 使用默认的程序图标
                nData.hIcon = LoadIcon(IntPtr.Zero, (IntPtr)SystemIcons.IDI_APPLICATION);
            }

            // 提示的超时值（几秒后自动消失）和版本
            //nData.uTimeoutAndVersion = 10 * 1000 | NOTIFYICON_VERSION; 
            // 类型标志，有INFO、WARNING、ERROR，更改此值将影响气泡提示框的图标类型
            nData.dwInfoFlags = NIIF_INFO;

            // 图标的提示信息
            nData.szTip = sTip;
            // 气泡提示框的标题
            nData.szInfoTitle = boxTitle;
            // 气泡提示框的提示内容
            nData.szInfo = boxText;

            return nData;
        }

        /// <summary>
        /// 加一个新图标
        /// </summary>
        /// <param name="iconHwnd">图标句柄</param>
        /// <param name="sTip">提示, 5.0 最大: 128 char</param>
        /// <param name="boxTitle">气泡标题, 最大: 64 char</param>
        /// <param name="boxText">气泡内容, 最大: 256 char</param>
        /// <returns>成功、失败或错误(-1)</returns>
        public int AddNotifyBox(IntPtr iconHwnd, string sTip, string boxTitle, string boxText)
        {
            NOTIFYICONDATA nData = GetNOTIFYICONDATA(iconHwnd, sTip, boxTitle, boxText);
            if (Shell_NotifyIcon(NIM_ADD, ref nData))
            {
                this.forgetDelNotifyBox = true;
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public int DelNotifyBox()
        {
            NOTIFYICONDATA nData = GetNOTIFYICONDATA(IntPtr.Zero, null, null, null);
            if (Shell_NotifyIcon(NIM_DELETE, ref nData))
            {
                this.forgetDelNotifyBox = false;
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public int ModiNotifyBox(IntPtr iconHwnd, string sTip, string boxTitle, string boxText)
        {
            NOTIFYICONDATA nData = GetNOTIFYICONDATA(iconHwnd, sTip, boxTitle, boxText);
            return Shell_NotifyIcon(NIM_MODIFY, ref nData) ? 1 : 0;
        }

        #region Optional Module //这里是可选方法
        /// <summary>
        /// 连接一个已存在的 contextMenu
        /// </summary>
        /// <param name="_formHwnd">窗体句柄，用来处理菜单的消息</param>
        /// <param name="_contextMenuHwnd">菜单的句柄</param>
        public void ConnectMyMenu(IntPtr _formHwnd, IntPtr _contextMenuHwnd)
        {
            formHwnd = _formHwnd;
            contextMenuHwnd = _contextMenuHwnd;
        }

        public void Dispose()
        {
        }
        #endregion
    }
}

