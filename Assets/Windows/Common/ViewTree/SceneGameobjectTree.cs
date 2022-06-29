using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using static ViewTreeArgs;

public class SceneGameobjectTree : ItemsControl
{
	protected override void InitData(InitDataArgs initDataArgs)
	{
        initDataArgs.ItemData = Resources.FindObjectsOfTypeAll<GameObject>().Where(go => !IsPrefab(go.transform) && go.transform.parent == null&& go.transform.GetComponent<Canvas>()==null).OrderBy(t => t.transform.GetSiblingIndex()).ToArray();

    }
	public static bool IsPrefab(Transform This)
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            throw new InvalidOperationException("Does not work in edit mode");
        }
        return This.gameObject.scene.buildIndex < 0;
    }
    /// <summary>
    /// 当产生新item的时候去绑定item要处理的数据
    /// </summary>
    /// <param name="args"></param>
    protected override void DataBindItem(ItemBindDataArgs args)
    {
		GameObject gameObject = args.Item as GameObject;
		args.canExpand = gameObject.transform.childCount > 0;
		args.showContent = gameObject.name;
	}
   

    /// <summary>
    /// 点击扩展的时候 需要用户自行定义展开子项对应的数据
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

	protected override void OnStructuralChangeArgs(StructuralChangeArgs structuralChangeArgs)
	{
        //处理这个改变
        structuralChangeArgs.isHandle = true;
        GameObject dragGameobject = structuralChangeArgs.dragItem.ItemObj as GameObject;
        GameObject droupGameobjject = structuralChangeArgs.droupItem.ItemObj as GameObject;

        switch (structuralChangeArgs.dropSibling)
		{
			case DropSibling.None:
				break;
			case DropSibling.Child:

                dragGameobject.transform.SetParent(droupGameobjject.transform,true);
                dragGameobject.transform.SetAsLastSibling();
                break;
			case DropSibling.PrevSibling:
                dragGameobject.transform.SetAsLastSibling();

                Debug.LogError("上面" + dragGameobject.name + "    " + droupGameobjject.name);

                if (droupGameobjject.transform.parent == dragGameobject.transform.parent)
                {
                    dragGameobject.transform.SetSiblingIndex(droupGameobjject.transform.GetSiblingIndex());

                }
                else
                {
                    dragGameobject.transform.SetParent(droupGameobjject.transform.parent);
                    dragGameobject.transform.SetSiblingIndex(droupGameobjject.transform.GetSiblingIndex());

                }
                break;
			case DropSibling.NextSibling:
                dragGameobject.transform.SetAsLastSibling();

                //Debug.LogError("下面"+ dragGameobject.name+"    "+ droupGameobjject.name);
                if (droupGameobjject.transform.parent == dragGameobject.transform.parent)
                {
                    dragGameobject.transform.SetSiblingIndex(droupGameobjject.transform.GetSiblingIndex() + 1);

                }
                else
                {

                    if (structuralChangeArgs.droupItem.IsExpand)
                    {
                        dragGameobject.transform.SetParent(droupGameobjject.transform);

                    }
                    else { 
                    dragGameobject.transform.SetParent(droupGameobjject.transform.parent);

                    }
                    dragGameobject.transform.SetSiblingIndex(droupGameobjject.transform.GetSiblingIndex() + 1);

                }

                break;
			default:
				break;
		}
	}

}
