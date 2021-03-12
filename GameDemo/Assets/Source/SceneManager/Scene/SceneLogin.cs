using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrameWork;

public class SceneLogin : SceneBase
{

    public SceneLogin() : base(SceneEnum.Login)
    {
    }

    public override void StartLoading()
    {
    }

    public override void OnEnter()
    {
        UIManage.Instance.CloseAllUI();

        SceneBattle.m_isFirstLogin = true;
        LoadLoginUI();
    }

    public override void OnDestroy()
    {
        UIManage.Instance.CloseUI((int)EnumUIType.LoginControllerPanel);
    }

    private void LoadLoginUI()
    {
        UIManage.Instance.OpenUI((int)EnumUIType.LoginControllerPanel, "LoginControllerPanel", null);
    }
}
