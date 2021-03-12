using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SSList;
using System;

#pragma warning disable 0414

/// <summary>
/// AB 资源管理器
/// </summary>
public class AssetBundleManager {
	public static readonly AssetBundleManager Instance = new AssetBundleManager();
    public void Init()
    {
        //预先加载 AssetBundleManifest
        LoadAssetBundle.Instance.LoadManifest();
    }

	private static float m_InvaternalTime = 0.0f;

    /// <summary>
    /// 字典缓存资源数据
    /// </summary>
	private SList<AssetBundleData> m_assertBundlePool = new SList<AssetBundleData>();

	private Dictionary<string, Element<AssetBundleData>> assetBundleDataCache = new Dictionary<string, Element<AssetBundleData>>();
	private Dictionary<string, Element<AssetBundleData>> AssetBundleDataCache
    {
        get { return assetBundleDataCache; }
    }

    private void AssetBundleDataCacheRemvoe(string assetBundleName)
    {
        AssetBundleDataCache.Remove(assetBundleName);
    }

    /// <summary>
    /// 字典缓存依赖数据
    /// </summary>
    private Dictionary<string, string[]> dependCache = new Dictionary<string, string[]>();
    private Dictionary<string, string[]> DependCache
    {
        get { return dependCache; }
    }

    /// <summary>
    /// 字典缓存错误数据
    /// </summary>
    private Dictionary<string, string> errorCache = new Dictionary<string, string>();
    private Dictionary<string, string> ErrorCache
    {
        get { return errorCache; }
    }

    /// <summary>
    /// 是否缓存了资源数据
    /// </summary>
    /// <param name="assetBundleName"></param>
    /// <returns></returns>
    internal bool InCache(string assetBundleName)
    {
        return AssetBundleDataCache.ContainsKey(assetBundleName);
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    /// <param name="assetBundleName"></param>
   // public void UnloadAssetBundle(string assetBundleName)
   // {
   //     UnloadAssetBundleInternal(assetBundleName);
   //     UnLoadDependencies(assetBundleName);
   // }

   // /// <summary>
   // /// 释放依赖资源
   // /// </summary>
   // /// <param name="assetBundleName"></param>
   // public void UnLoadDependencies(string assetBundleName)
   // {
   //     string[] dependencies = null;
   //     if (!DependCache.TryGetValue(assetBundleName, out dependencies))
   //     {
   //         return;
   //     }

   //     for (int i = 0; i < dependencies.Length; ++i)
   //     {
   //         UnloadAssetBundleInternal(dependencies[i]);
   //     }
   // }

   // /// <summary>
   // /// 释放特定名资源
   // /// </summary>
   // /// <param name="assetBundleName"></param>
   // public void UnloadAssetBundleInternal(string assetBundleName)
   // {
   //     Element<AssetBundleData> assetBundleData = null;
   //     if (AssetBundleDataCache.TryGetValue(assetBundleName, out assetBundleData))
   //     {
			//assetBundleData.baseElement.Release();
   //     }
   // }

    /// <summary>
    /// 根据资源名获取资源数据
    /// </summary>
    /// <param name="assetBundleName"></param>
    /// <returns></returns>
    public AssetBundleData GetAssetBundleData(string assetBundleName)
    {
        Element<AssetBundleData> assetBundleData = null;
        AssetBundleDataCache.TryGetValue(assetBundleName, out assetBundleData);
		if (assetBundleData != null) {
			return assetBundleData.baseElement;
		}
		return null;
    }

    public AssetBundleData AddAssetBundle(string assetBundleName)
    {
        Element<AssetBundleData> f_assetBundleData = m_assertBundlePool.Pop();
        f_assetBundleData.baseElement.Init();
        f_assetBundleData.baseElement.m_AssetBundleName = assetBundleName;
        f_assetBundleData.baseElement.Retain();
        assetBundleDataCache[assetBundleName] = f_assetBundleData;
        return f_assetBundleData.baseElement;
    }

    /// <summary>
    /// 添加资源数据
    /// </summary>
    /// <param name="assetBundleName"></param>
    /// <param name="assetBundleData"></param>
    /*public void SetAssetBundleData(string assetBundleName, AssetBundleData assetBundleData)
    {
        if (!AssetBundleDataCache.ContainsKey(assetBundleName))
        {
            assetBundleData.m_AssetBundleName = assetBundleName;
            assetBundleData.Retain();
            AssetBundleDataCache.Add(assetBundleName, assetBundleData);
        }
    }*/

    /// <summary>
    /// 根据资源名获取依赖
    /// </summary>
    /// <param name="assetBundleName"></param>
    /// <returns></returns>
    public string[] GetDepends(string assetBundleName)
    {
        string[] depends = null;
        DependCache.TryGetValue(assetBundleName, out depends);
        return depends;
    }

    /// <summary>
    /// 添加依赖数据
    /// </summary>
    public void SetDepends(string assetBundleName, string[] depends)
    {
        if (!DependCache.ContainsKey(assetBundleName))
        {
            DependCache.Add(assetBundleName, depends);
        }
    }

    /// <summary>
    /// 获取错误数据
    /// </summary>
    /// <returns></returns>
    public string GetError(string assetBundleName)
    {
        string error = string.Empty;
        ErrorCache.TryGetValue(assetBundleName, out error);
        return error;
    }

    /// <summary>
    /// 添加错误数据
    /// </summary>
    /// <param name="assetBundleName"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    public void SetError(string assetBundleName, string error)
    {
        if (!ErrorCache.ContainsKey(assetBundleName))
        {
            ErrorCache.Add(assetBundleName, error);
        }
    }

    /// <summary>
    /// 引用计数为 0 时，将数据移除
    /// </summary>
    /// <param name="assetBundleName"></param>
    public void FreeAssetBundle(string assetBundleName)
    {
		Element<AssetBundleData> assetBundleData = null;
		AssetBundleDataCache.TryGetValue(assetBundleName, out assetBundleData);
		if (assetBundleData != null) {
			m_assertBundlePool.Push (assetBundleData);
            AssetBundleDataCacheRemvoe(assetBundleName);
		}
    }

    public void OnFrame()
    {
		if (!AssetBundleData.delayMode) {
			return;
		}
		m_InvaternalTime += Time.deltaTime;
		if (m_InvaternalTime > 1.0f) {
			ListSeek<AssetBundleData> f_seek = m_assertBundlePool.GetGlobalBusyListSeek();
			Element<AssetBundleData> f_element = f_seek.GetNextElement();
			while (f_element != null)
			{
				if (f_element.baseElement.OnFrame (m_InvaternalTime)) {
					f_element.baseElement.OnReleaseRes ();
					m_assertBundlePool.Push (f_element);
                    AssetBundleDataCacheRemvoe(f_element.baseElement.m_AssetBundleName);
				}
				f_element = f_seek.GetNextElement();
			}
			m_InvaternalTime = 0.0f;
		}
    }

}
