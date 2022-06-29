using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WindowsToolsOver
{

    private static Rect currentRect;

    public static bool isMax;
    public static bool isMin;
    public static void MaxWindows()
    {
        

        isMax = true;
        if (currentRect != null)
        {
            currentRect = WindowsTools.GetWindowRect();
        }
       

        int width = WindowsTools.GetSystemMetricsByIndex(0);
        int height = WindowsTools.GetSystemMetricsByIndex(1);

        int taskHeight = WindowsTools.GetSystemMetricsByIndex(4);



        int posX = 0;

        Rect windoeRect;
        if (currentRect != null)
        {
            windoeRect = currentRect;
        }
        else {
             
            windoeRect = WindowsTools.GetWindowRect();

        }


        if (Display.displays.Length>1) {

            posX = 0;
            for (int i = 1; i < Display.displays.Length; i++)
            {
                if (windoeRect.position.x+(Display.displays[i].systemWidth/2) > Display.displays[i].systemWidth)
                {
                    posX += Display.displays[i-1].systemWidth;
                    height= Display.displays[i].systemHeight;
                    width= Display.displays[i].systemWidth; 
                }
            }
        }

        

        Rect rect = new Rect();
        rect.width = width;
        rect.height = height - taskHeight;
        rect.position = new Vector2(posX, 0);

        Debug.LogError(rect.width + "    " + rect.height + "  " + rect.position);


       // WindowsTools.SetMaxWindows();

        WindowsTools.SetWindowPos(rect);
    }

    public static void MinWindows()
    {
        currentRect = WindowsTools.GetWindowRect();
        WindowsTools.SetMinWindows();
        isMin = true;

    }

    public static void Normal()
    {
        //WindowsTools.SetNormalWindow();
        isMax = false;

        if (currentRect==null) {
            int width = WindowsTools.GetSystemMetricsByIndex(0);
            int height = WindowsTools.GetSystemMetricsByIndex(1);
            currentRect = new Rect(new Vector2(width/2,height/2), new Vector2(width / 3, height / 3));
        }
        WindowsTools.SetWindowPos(currentRect);
    }

    public static void  DragEnd() {
        currentRect = WindowsTools.GetWindowRect();
    }
}


