using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneEnum
{
    NONE        = 0,
    /// <summary>
    /// 登录场景
    /// </summary>
    Login       = 1,

    /// <summary>
    /// 战斗
    /// </summary>
    Battle = 2,

    /// <summary>
    /// 大世界
    /// </summary>
    Kindom = 4,
}

public static class ManagerScene  {

    private static SceneEnum currentSceneState = SceneEnum.NONE;
    private static Dictionary<SceneEnum, SceneBase> SceneDic = new Dictionary<SceneEnum, SceneBase>();

    public static SceneBase m_CurrentScene = null;
    static ManagerScene()
    {
        SceneDic[SceneEnum.Login]        = new SceneLogin();
        SceneDic[SceneEnum.Battle]       = new SceneBattle();
    }

    // 获取当前场景状态
    public static SceneEnum CurrentSceneState
    {
        get { return currentSceneState; }
    }

    // 正在进入的场景
    private static SceneEnum enteringScene = SceneEnum.NONE;
    public static void LoadScene(SceneEnum state, string sceneName = "")
    {
        if (string.IsNullOrEmpty(sceneName) || sceneName.CompareTo("") == 0)
        {
            sceneName = ManagerScene.GetSceneName(state);
        }

        if (state == enteringScene)
        {
            //防止卡死,当前场景 就是目标场景 不需要切换"
            Debug.LogError("SceneState : " + state + "     enteringScene : " + enteringScene);
            return;
        }

        enteringScene = state;

        // 开始加载场景
        StartGoToScene(state);

        //AsyncOperation asyncOperation = null;
        //AssetPool.Instance.Scene.LoadScene(sceneName, out asyncOperation);
        //Game.Instance.StartCoroutine(WaitLoadScene(state, asyncOperation));

        // LoadScene(string sceneName, LoadCallBackHandler CallBack)
        AssetPool.Instance.Scene.LoadScene(sceneName, LoadSceneCallBack);
    }

    //private static IEnumerator WaitLoadScene(SceneEnum state, AsyncOperation asyncOperation)
    //{
    //    yield return asyncOperation;
    //    EnteredScene(state);
    //}

    private static void LoadSceneCallBack(HandlerParam hp)
    {
        EnteredScene(enteringScene);
    }

    // 开始切换场景
    public static void StartGoToScene(SceneEnum state)
    {
        if (m_CurrentScene != null)
        {
            m_CurrentScene.OnDestroy();
            m_CurrentScene = null;
        }

        if (!SceneDic.ContainsKey(state))
        {
            Debug.LogError("SceneState is null : " + state);
            return;
        }

        SceneDic[state].StartLoading();
    }

    // 进入场景
    public static void EnteredScene(SceneEnum state)
    {
        currentSceneState = state;
        if (!SceneDic.ContainsKey(state))
        {
            Debug.LogError("SceneState is null : " + state);
            return;
        }

        m_CurrentScene = SceneDic[currentSceneState];
        m_CurrentScene.OnEnter();
    }

    /// <summary>
    /// 通过场景类型获取场景名
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public static string GetSceneName(SceneEnum state)
    {
        string mapName = string.Empty;
        switch (state)
        {
            case SceneEnum.Login:
                mapName = "Login";
                break;
            case SceneEnum.Battle:
                mapName = "Battle";
                break;
            default:
                break;
        }

        return mapName;
    }
}