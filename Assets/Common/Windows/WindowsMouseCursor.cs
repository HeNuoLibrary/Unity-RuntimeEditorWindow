using System.Runtime.InteropServices;
using System;



/// <summary>
/// �����������ϵͳͼ��
/// </summary>
public class WindowsMouseCursor 
{
    //���ع����Դ���غ���
    [DllImport("user32.dll")]
    public static extern IntPtr LoadCursorFromFile(string fileName);

    public const uint OCR_NORMAL = 32512;


    [DllImport("user32.dll")]
    public static extern bool SetSystemCursor(IntPtr hcur, uint id);
 



    public const uint SPIF_SENDWININICHANGE = 2;

    //����ϵͳ���
    public const uint SPI_SETCURSORS = 87;
    [DllImport("user32.dll")]
    public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, IntPtr pvParam, uint fWinIni);

    /// <summary>
    /// ����ϵͳ���
    /// </summary>
    public static void SystemParametersInfo()
    {
        //�ָ�ΪϵͳĬ��ͼ��
        SystemParametersInfo(SPI_SETCURSORS, 0, IntPtr.Zero, SPIF_SENDWININICHANGE);
    }

    public static void SetCursor(string path) {
        IntPtr hcur = LoadCursorFromFile(path);
        SetSystemCursor(hcur, OCR_NORMAL);
    }

    
}
