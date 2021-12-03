using System.Runtime.InteropServices;
using System;



/// <summary>
/// 用于设置鼠标系统图标
/// </summary>
public class WindowsMouseCursor 
{
    //加载光标资源加载函数
    [DllImport("user32.dll")]
    public static extern IntPtr LoadCursorFromFile(string fileName);

    public const uint OCR_NORMAL = 32512;


    [DllImport("user32.dll")]
    public static extern bool SetSystemCursor(IntPtr hcur, uint id);
 



    public const uint SPIF_SENDWININICHANGE = 2;

    //重置系统光标
    public const uint SPI_SETCURSORS = 87;
    [DllImport("user32.dll")]
    public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, IntPtr pvParam, uint fWinIni);

    /// <summary>
    /// 重置系统光标
    /// </summary>
    public static void SystemParametersInfo()
    {
        //恢复为系统默认图标
        SystemParametersInfo(SPI_SETCURSORS, 0, IntPtr.Zero, SPIF_SENDWININICHANGE);
    }

    public static void SetCursor(string path) {
        IntPtr hcur = LoadCursorFromFile(path);
        SetSystemCursor(hcur, OCR_NORMAL);
    }

    
}
