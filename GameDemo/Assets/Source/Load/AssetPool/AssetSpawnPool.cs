using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

#pragma warning disable 0219,0162

public delegate void LoadCallBackHandler(HandlerParam p_handleParam);

public class HandlerParam
{
    public string assetName;                    // 资源名
    public System.Object paramObj;              // 回调参数组
    public AssetBundleData m_assetBundleData;   // 加载数据
    public UnityEngine.Object assetObj;        // 加载资源

    public HandlerParam()
    {
        assetName = string.Empty;
        paramObj = null;
        m_assetBundleData = null;
        assetObj = null;
    }
}

/// <summary>
/// 资源生成池
/// </summary>
public class AssetSpawnPool
{
    //private string spawnName;   
    private string assetPath;
    private bool isUsePool;
    private bool isAsynchLoad;
    private string extension;

    /// <summary>
    /// 调试 AB 使用：true 调试 AB
    /// </summary>
    public static bool isDebug = true;

    public AssetSpawnPool(string spawnName = "")
    {
        //this.spawnName = spawnName;
    }

    /// <summary>
    /// 资源路径
    /// </summary>
    public string AssetPath
    {
        get { return assetPath; }
        set { assetPath = value; }
    }

    /// <summary>
    /// 是否使用内存池
    /// </summary>
    public bool IsUsePool
    {
        get { return isUsePool; }
        set { isUsePool = value; }
    }

    /// <summary>
    /// 是否异步加载
    /// </summary>
    public bool IsAsynchLoad
    {
        get { return isAsynchLoad; }
        set { isAsynchLoad = value; }
    }

    public string Extension
    {
        get { return extension; }
        set { extension = value; }
    }

    /// <summary>
    /// 加载资源
    /// </summary>
    /// <typeparam name="T">加载资源类型</typeparam>
    /// <param name="assetName">资源名</param>
    /// <param name="CallBack">加载回调</param>
    /// <param name="paramsArr">加载传递参数</param>
    /// <param name="useType">加载解包时，是否使用类型 T</param>
    public void LoadAsset<T>(string assetName, LoadCallBackHandler CallBack, System.Object paramsArr, bool useType = true, bool isAsync = true) where T : UnityEngine.Object
    {
        if (string.IsNullOrEmpty(assetName))
        {
            Debug.LogWarning("Load AssetName is null " + assetName);
            return;
        }

        string _assetPath = string.Format("{0}{1}{2}", AssetPath, assetName, Extension);

        HandlerParam HP = new HandlerParam();
        HP.assetName = Path.GetFileNameWithoutExtension(assetName);
        HP.paramObj = paramsArr;

        if (Application.isEditor && !isDebug)
        {
            _assetPath = string.Format("{0}{1}", "Assets/", _assetPath);
            LoadAsset<T>(_assetPath, CallBack, HP);
            return;
        }

        _assetPath = Path.ChangeExtension(_assetPath, ResourcePathManager.abExtension);
        string url = ResourcePathManager.Instance.GetLoadPath(_assetPath);
        url = url.ToLower();
		string _assetBundleName = Path.ChangeExtension(_assetPath, ResourcePathManager.abExtension).ToLower();
        // allassets/prefab/building/dige_ziyuan.unity3d
        // 加载资源回调
        LoadABCallBack loadABCallBack = delegate (AssetBundleData assetBundleData)
        {
            HP.m_assetBundleData = assetBundleData;
            // 异步加载同时调用两次会卡死主线程
            //.m_AssetBundle.LoadAssetAsync<T>(p_handleParam.assetName);
            HP.assetObj = assetBundleData.m_AssetBundle.LoadAsset<T>(HP.assetName);

            if (CallBack != null)
            {
                CallBack(HP);
            }
        };

        Game.Instance.StartCoroutine(LoadAssetBundle.Instance.LoadAsset(url, loadABCallBack, _assetBundleName));
    }

    public void LoadScene(string sceneName, LoadCallBackHandler CallBack)
    {
        string _scenePath = string.Format("{0}{1}", AssetPath, sceneName);
        HandlerParam HP = new HandlerParam();
        HP.assetName = sceneName;
        HP.paramObj = null;

        if (Application.isEditor && !isDebug)
        {
            if (CallBack != null)
            {
                CallBack(HP);
            }

            SceneManager.LoadScene(HP.assetName);
            return;
        }

        string url = ResourcePathManager.Instance.GetLoadPath(_scenePath);
        url = Path.ChangeExtension(url, ResourcePathManager.abExtension);
		string _assetBundleName = Path.ChangeExtension(_scenePath, ResourcePathManager.abExtension).ToLower();
        LoadABCallBack loadABCallBack = delegate (AssetBundleData assetBundleData)
        {
            HP.m_assetBundleData = assetBundleData;
            if (CallBack != null)
            {
                CallBack(HP);
            }
            SceneManager.LoadScene(HP.assetName);
        };
        LoadAssetBundle.Instance.LoadScene(url, sceneName, _assetBundleName, loadABCallBack);
    }

    private void LoadAsset<T>(string _assetPath, LoadCallBackHandler loadCallBack, HandlerParam HP) where T : UnityEngine.Object
    {
#if UNITY_EDITOR
        T t = AssetDatabase.LoadAssetAtPath<T>(_assetPath);
        HP.assetObj = t;
        loadCallBack(HP);
#endif
    }
}
