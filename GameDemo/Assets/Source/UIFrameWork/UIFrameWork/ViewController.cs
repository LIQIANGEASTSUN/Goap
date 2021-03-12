using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MVC UI 控制器
/// </summary>
public abstract class ViewController : MonoBehaviour {

    /// <summary>
    /// View 获取
    /// </summary>
    protected abstract void GetView();

    /// <summary>
    /// Model 数据获取
    /// </summary>
    protected abstract void GetData();

    /// <summary>
    /// 刷新固定UI
    /// </summary>
    public abstract void RefreshFixed();

    /// <summary>
    /// 刷新可变 UI
    /// </summary>
    public virtual void RefreshUI()
    {

    }
}

public interface UIModel
{
    /// <summary>
    /// 固定数据
    /// </summary>
    void FixedData();

    /// <summary>
    /// 会发生变化的数据
    /// </summary>
    void ReSet();
}