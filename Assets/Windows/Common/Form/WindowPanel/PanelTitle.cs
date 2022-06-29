using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelTitle : MonoBehaviour
{
    public LableRect lableRect;


    public void InitData()
    {
       
        lableRect = transform.Find("LableRect").GetComponent<LableRect>();
        lableRect.InitData();
    }

    private void Start()
    {
        InitData();
    }
}
