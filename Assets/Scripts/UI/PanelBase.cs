using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
///显示面板的基类，主要定义一些共同的方法
/// </summary>
public class PanelBase : MonoBehaviour
{
    private Dictionary<string, List<MonoBehaviour>> mComponentDic = new Dictionary<string, List<MonoBehaviour>>();
    public T FindComponent<T>(string componentName) where T : MonoBehaviour
    {

        if (mComponentDic.ContainsKey(componentName))
        {
            for (int i = 0; i < mComponentDic[componentName].Count; i++)
            {
                if (mComponentDic[componentName][i] is T)
                {
                    return mComponentDic[componentName][i] as T;
                }
            }
        }
        FindAllComponents<T>();

        if (mComponentDic.ContainsKey(componentName))
        {
            for (int i = 0; i < mComponentDic[componentName].Count; i++)
            {
                if (mComponentDic[componentName][i] is T)
                {
                    return mComponentDic[componentName][i] as T;
                }
            }
        }

        return null;
    }
    /// <summary>
    /// 找到当前所有组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private void FindAllComponents<T>() where T : MonoBehaviour
    {
        T[] components = this.GetComponentsInChildren<T>(true);//得到所有组件
        for (int i = 0; i < components.Length; i++)
        {
            //如果包含该名字得组件，直接添加
            if (mComponentDic.ContainsKey(components[i].name))
            {
                mComponentDic[components[i].name].Add(components[i]);
            }
            else
            {
                mComponentDic.Add(components[i].name, new List<MonoBehaviour> { components[i] });
            }
        }
    }
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}

