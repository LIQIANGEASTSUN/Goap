using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

/// <summary>
/// 管理加载资源路径
/// </summary>
public class ResourcePathManager
{
    //单例
    public static readonly ResourcePathManager Instance = new ResourcePathManager();

    // 根据不同平台获取资源路径,移动平台为沙盒路径, 放在 StreamingAssets 下的文件不在此处
    public string GetPersistentDataPath
    {
        get
        {
            if (Application.isEditor)
            {
                //return "file://" + Application.persistentDataPath + "/";
                return Application.persistentDataPath + "/";
            }

            //不同平台下 Persistent 的路径是不同的，这里需要注意一下。
#if UNITY_ANDROID
            return "file://" + Application.persistentDataPath + "/";
#elif UNITY_IPHONE
            return "file://" + Application.persistentDataPath + "/";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
            return "file://" + Application.persistentDataPath + "/";
#else
			return string.Empty;
#endif
        }
    }

    //优先获取沙盒内的资源，如果沙盒不存在该资源，从本地 StreamingAssets文件夹下获取
    public string CheckFilePath(string persistentDataPath, string filePath)
    {
        string url = System.IO.Path.Combine(persistentDataPath, filePath); // String.Format("{0}/{1}", persistentDataPath, filePath); // System.IO.Path.Combine(persistentDataPath, filePath);

        // 沙盒内存在加载文件则返回沙盒内路径
        if (File.Exists(url))
        {
            return url;
        }

        return GetStreamingAssetsPath(filePath);
    }

    // 返回本地 StreamAssetsPath 路径
    public string GetStreamingAssetsPath(string filePath)
    {
        if (Application.isEditor)
        {
            return "file:///" + Application.streamingAssetsPath + "/" + filePath;
        }

#if UNITY_ANDROID
        //return "jar:file:///" + Application.dataPath + "!/assets/";
        return Application.streamingAssetsPath + "/" + filePath;
#elif UNITY_IPHONE
        //return Application.dataPath + "/Raw/";
        return "file://" + Application.streamingAssetsPath + "/" + filePath;
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
        //return Application.dataPath + "/StreamingAssets/";
        return "file://" + Application.streamingAssetsPath + "/" + filePath;
#else
        return String.Format("{0}/{1}", Application.streamingAssetsPath, filePath);
#endif
    }

    //AB包资源后缀名
    public static readonly string abExtension = "unity3d";
    public string GetLoadPath( string path, bool isassetBundle = true)
    {
        string persistentPath = GetPersistentDataPath;
        string combinPath = path;
        if (isassetBundle)
        {
            combinPath = Path.Combine("AssetBundle", path);
        }
        string url = CheckFilePath(persistentPath, combinPath);
        //url = System.IO.Path.ChangeExtension(url, abExtension);

        return url;
    }

}
