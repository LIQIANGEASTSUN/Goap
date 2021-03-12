using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 保存 AB 引用计数
/// </summary>
public class AssetBundleData
{
    internal AssetBundle m_AssetBundle = null;          // 加载的AB资源
    internal string m_AssetBundleName = string.Empty;   // 资源 assetBundleName 
    internal int m_ReferenceCount = 0;                  // 资源引用计数
	internal float m_cacheTime = 20.0f;
	public static bool delayMode = false;
    #region ReferenceCount
    /// <summary>
    /// 引用计数 +1
    /// </summary>
    public void Retain()
    {
        ++m_ReferenceCount;
    }

	public void Init()
	{
		m_cacheTime = 20.0f;
		m_ReferenceCount = 0;
		m_AssetBundleName = string.Empty;
		m_AssetBundle = null;
	}

    /// <summary>
    /// 释放引用时将引用计数 -1
    /// </summary>
    public void Release()
    {
        --m_ReferenceCount;
		if (!delayMode) {
			if (m_ReferenceCount <= 0)     // 引用数为 0 时将 AB 卸载
			{
				ReleaseAB ();
			}
		}
    }

	private void ReleaseAB()
	{
		if (m_AssetBundle != null)
		{
			m_AssetBundle.Unload(false);
			AssetBundleManager.Instance.FreeAssetBundle(m_AssetBundleName);
		}
		m_AssetBundle = null;
	}

	public void OnReleaseRes()
	{
		m_AssetBundle.Unload(false);
	}

	public bool OnFrame(float p_time)
	{
		if (delayMode) {
			if (m_ReferenceCount <= 0) {
				m_cacheTime -= p_time;
				if(m_cacheTime <= 0)
				{
					return true;
				}
			}
		}
		return false;
	}

    /// <summary>
    /// 获取引用计数
    /// </summary>
    public int ReferenceCount
    {
        get { return m_ReferenceCount; }
    }

    #endregion
}
