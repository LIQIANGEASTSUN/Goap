using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

// 回调方法
public delegate void LoadABCallBack(AssetBundleData assetBundleData);

// AS 文件只能加载一次，重复加载出错，两种方案，
// 一、加载后卸载，下次需要使用时重新加载，
// 二、加载后将AS保存起来，下载加载相同AS时直接取出来使用
public class LoadAssetBundle
{
    //单例
    public static readonly LoadAssetBundle Instance = new LoadAssetBundle();

    //所有资源总依赖文件
    public AssetBundleManifest assetBundleManifest = null;
    public bool isLoadingAssetBundleMainfest = false;

    private HashSet<string> loadingUrl = new HashSet<string>();
    private void AddLoading(string key)
    {
        loadingUrl.Add(key);
    }

    private void DelLoading(string key)
    {
        if (loadingUrl.Contains(key))
        {
            loadingUrl.Remove(key);
        }
    }

    private bool IsLoading(string key)
    {
        return loadingUrl.Contains(key);
    }

    /// <summary>
    /// 加载 AB 资源
    /// </summary>
    /// <param name="url"></param>
    /// <param name="abCallBack"></param>
    /// <returns></returns>
    private IEnumerator LoadAB(string url, LoadABCallBack loadABCallBack, bool isDepends = false)
    {
        if (isDepends && IsLoading(url))
        {
            yield break;
        }

        WWW www = new WWW(url);
        yield return www;

        DelLoading(url);

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.LogError(www.error);
            yield break;
        }

        AssetBundleData newAssetBundleData = AssetBundleManager.Instance.AddAssetBundle(url); //new AssetBundleData();
        newAssetBundleData.m_AssetBundle = www.assetBundle;

        if (loadABCallBack != null)
        {
            loadABCallBack(newAssetBundleData);
        }

        yield break;
    }

    /// <summary>
    /// 加载资源
    /// </summary>
    /// <param name="assetPath"></param>
    /// <param name="callBack"></param>
    /// <param name="assetBundleName"></param>
    /// <returns></returns>
    public IEnumerator LoadAsset(string assetPath, LoadABCallBack loadABCallBack, string assetBundleName)
	{
        while (IsLoading(assetPath))
        {
            yield return new WaitForEndOfFrame();
        }

        {
            // 从管理器中获取加载数据
            AssetBundleData assetBundleData = AssetBundleManager.Instance.GetAssetBundleData(assetPath);
            // 获取到的数据不为空就不再重新加载了
            if (assetBundleData != null)
            {
                // 引用 +1
                assetBundleData.Retain();
                if (loadABCallBack != null)
                {
                    loadABCallBack(assetBundleData);
                }

                yield break;  // 已经有了直接返回
            }
        }

        if (assetBundleManifest == null)
        {
            yield return Game.Instance.StartCoroutine(LoadManifest());
        }

		if (assetBundleManifest == null) {
			Debug.LogError ("assetBundlManifest is");
			yield break;
		}

        AddLoading(assetPath);

        // 加载依赖资源
        // 获取依赖资源数据
        string[] depends = assetBundleManifest.GetAllDependencies(assetBundleName);
        int count = depends.Length;
        if (count > 0)
        {
            AssetBundleManager.Instance.SetDepends(assetBundleName, depends);
        }

        List<string> urlList = new List<string>();
        for (int i = 0; i < count; ++i)
        {
            string url = ResourcePathManager.Instance.CheckFilePath(ResourcePathManager.Instance.GetPersistentDataPath, "AssetBundle/" + depends[i]);
            url = url.ToLower();

            AssetBundleData assetBundleData = AssetBundleManager.Instance.GetAssetBundleData(url);
            // 获取到的数据不为空就不再重新加载了
            if (assetBundleData != null)
            {
                continue;
            }

            Game.Instance.StartCoroutine(LoadAB(url, null, true));

            urlList.Add(url);
            AddLoading(url);
        }

        int index = 0;
        while(urlList.Count > 0)
        {
            index %= urlList.Count;
            if (!IsLoading(urlList[index]))
            {
                urlList.RemoveAt(index);
            }

            ++index;
            yield return new WaitForEndOfFrame();
        }

        LoadABCallBack CallBack = delegate (AssetBundleData assetBundleData)
        {
            loadABCallBack(assetBundleData);
        };

        yield return Game.Instance.StartCoroutine(LoadAB(assetPath, CallBack));
	}
    //AB.Unload  卸载 AssetBundl：
    //参数 false 只卸载 AssetBundl 资源，已经从 AssetBundle 资源中 Load 并且实例化出来的对象不会被释放
    //参数 true  卸载 AssetBundl 的同时，也会把从 AssetBundl 资源中Load 并且实例化出来的对象一起销毁（很危险，慎用）
    public void LoadScene(string sceneAssetPath, string sceneName, string assetBundleName, LoadABCallBack loadABCallBack)
    {
        Game.Instance.StartCoroutine(LoadAsset(sceneAssetPath, loadABCallBack, assetBundleName));
    }

    //加载总依赖资源文件 AssetBundleManifest
    public IEnumerator LoadManifest()
    {
        if (!isLoadingAssetBundleMainfest)
        {
            isLoadingAssetBundleMainfest = true;

            LoadABCallBack loadABCallBack = delegate (AssetBundleData assetBundleData)
            {
                if (assetBundleData.m_AssetBundle != null)
                {
                    assetBundleManifest = assetBundleData.m_AssetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                }
            };

            string url = ResourcePathManager.Instance.CheckFilePath(ResourcePathManager.Instance.GetPersistentDataPath, "AssetBundle/AssetBundle");
            yield return Game.Instance.StartCoroutine(LoadAB(url, loadABCallBack));
        }

		while (assetBundleManifest == null) {
			yield return new WaitForEndOfFrame ();
		}

		yield break;
    }
}