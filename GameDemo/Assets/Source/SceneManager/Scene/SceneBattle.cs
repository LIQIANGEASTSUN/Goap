using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrameWork;

/// <summary>
/// 主城
/// </summary>
public class SceneBattle : SceneBase
{
    public static bool m_isFirstLogin = false;
    public SceneBattle() : base(SceneEnum.Battle)
    {
    }

    public override void StartLoading()
    {

    }

    public override void OnEnter()
    {
        LoadInitUI();
        CreateRoom();
    }

    public override void OnDestroy()
    {
        Room.Release();
    }

    // 加载初始化UI
    private void LoadInitUI()
    {
        if (!m_isFirstLogin)
        {
            return;
        }
        m_isFirstLogin = false;

        UIManage.Instance.OpenUI((int)EnumUIType.HpControllerPanel, "HpControllerPanel", null);
    }

    private void CreateRoom()
    {
        Room.CreateRoom();
    }
}