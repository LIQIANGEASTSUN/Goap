using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneBase
{
    protected SceneEnum sceneState = SceneEnum.NONE;

    public SceneBase(SceneEnum p_sceneState)
    {
        sceneState = p_sceneState;
    }

    /// <summary>
    /// 开始加载场景
    /// </summary>
    public abstract void StartLoading();

    /// <summary>
    /// 已经进入场景
    /// </summary>
    public abstract void OnEnter();

    /// <summary>
    /// 销毁场景
    /// </summary>
    public abstract void OnDestroy();

}
