using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ToolsComponent {

    public static T FindChildCom<T>(Transform parent, string childName) where T : Component
    {
        T component = default(T);
        if (parent == null)
        {
            Debug.LogError("Parent is null");
            return component;
        }
        Transform childTr = parent.FindChild(childName);
        if (childTr == null)
        {
            return component;
        }

        component = GetComponent<T>(childTr);

        return component;
    }

    public static T GetComponent<T>(Transform tr)
    {
        if (tr == null)
        {
            Debug.LogError("Tr is null");
            return default(T);
        }
        return GetComponent<T>(tr.gameObject);
    }

    public static T GetComponent<T>(GameObject go)
    {
        T component = default(T);
        if (go == null)
        {
            Debug.LogError("GameObject is null");
            return component;
        }

        component = go.GetComponent<T>();
        if (component == null)
        {
            Debug.LogError("Component is null :" + go.GetType());
        }

        return component;
    }

    public static Transform CloneItem(Transform parent, Transform childTr)
    {
        Transform cloneTr = CloneItem(childTr);
        AddChild(parent, cloneTr);
        return cloneTr;
    }

    public static Transform CloneItem(Transform item)
    {
        Transform cloneTr = UnityEngine.Object.Instantiate(item) as Transform;
        return cloneTr;
    }

    public static void AddChild(Transform p_parent, Transform p_child, bool isFirstChild = false)
    {
        if (p_parent == null || p_child == null)
        {
            return;
        }
        p_child.SetParent( p_parent);
        p_child.localPosition = Vector3.zero;
        p_child.localScale = Vector3.one;
        p_child.localRotation = Quaternion.identity;

        if (!isFirstChild)
        {
            p_child.SetAsLastSibling();
        }
        else
        {
            p_child.SetAsFirstSibling();
        }
    }
}