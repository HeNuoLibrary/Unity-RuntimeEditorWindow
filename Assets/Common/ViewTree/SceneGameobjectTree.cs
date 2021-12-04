using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class SceneGameobjectTree : ItemsControl
{

 
    void Start()
    {
        ///���õ�ǰ�ڵ���
        ItemObjs = Resources.FindObjectsOfTypeAll<GameObject>().Where(go => !IsPrefab(go.transform) && go.transform.parent == null).OrderBy(t => t.transform.GetSiblingIndex());

    }
    public static bool IsPrefab(Transform This)
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            throw new InvalidOperationException("Does not work in edit mode");
        }
        return This.gameObject.scene.buildIndex < 0;
    }
    protected override void DataBindItem(ItemBindDataArgs args)
    {

        GameObject gameObject = args.Item as GameObject;
        args.hasChild = gameObject.transform.childCount > 0;
        args.showContent = gameObject.name;
    }
   

    /// <summary>
    /// �����չ��ʱ��
    /// </summary>
    /// <param name="item"></param>
    protected override void OnExpand(ItemExpandingArgs args)
    {
        GameObject gameObject = args.Item as GameObject;
        GameObject[] childObj = new GameObject[gameObject.transform.childCount];

        for (int i = 0; i < childObj.Length; i++)
        {
            childObj[i] = gameObject.transform.GetChild(i).gameObject;
        }
        args.Children = childObj;


    }

}
