using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewTreeDemo : MonoBehaviour
{
    SceneGameobjectTree treeController;
    private void Awake()
    {
        treeController = GetComponent<SceneGameobjectTree>();
    }
    private void OnEnable()
    {
        //treeController.itemExpandingArgs += OnItemExpand;
        //treeController.itemBindDataArgs += OnBindDataArgs;
    }
    private void OnDisable()
    {
        //treeController.itemExpandingArgs -= OnItemExpand;
        //treeController.itemBindDataArgs -= OnBindDataArgs;
    }

    private void OnBindDataArgs(object sender, ItemBindDataArgs e)
    {
        GameObject gameObject = e.Item as GameObject;
        e.hasChild = gameObject.transform.childCount > 0;
        e.showContent = gameObject.name;
    }

    private void OnItemExpand(object sender, ItemExpandingArgs e)
    {
  
        GameObject gameObject = e.Item as GameObject;
        GameObject[] childObj = new GameObject[gameObject.transform.childCount];

        for (int i = 0; i < childObj.Length; i++)
        {
            childObj[i] = gameObject.transform.GetChild(i).gameObject;
        }
        e.Children = childObj;
    }
}
