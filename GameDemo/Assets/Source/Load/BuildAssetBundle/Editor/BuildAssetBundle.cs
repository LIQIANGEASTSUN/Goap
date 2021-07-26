using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;
using System.Text;
using System.Security.Cryptography;
using System.Linq;

// 注意 因为 Windows 的路径分隔符为 '/', Mac OS 系统的路径分隔符号为 '\' 大坑
// 当时是在 Mac 上写的打包方法， 没注意到这个大坑， 后来在 Windows 上使用时发现此坑
// 如果发现打包不成功，可以打印一下代码中有关路径的地方，看看是不是有问题
// 大致就是使用如   pathReplace = path.Replace('\\', '/');   pathReplace = path.Replace('/', '\\');
// 转换下即可
public static class BuildAssetBundle
{
    //导出包路径
    private static string AssetBundleOutPsth = Application.streamingAssetsPath;
    //打AS包
    public static void BuildABAsset(int type = 0)
    {
        Caching.ClearCache();
        BuildAsset(type);
        BuildScene();

        //刷新资源路径,避免生成的文件不显示
        AssetDatabase.Refresh();

        BuildAssetBundleVersion.BuildVersion();
    }

    // 打包资源
    /// <summary>
    /// 
    /// </summary>
    /// <param name="type">0 增量打包，1 重新打包</param>
    public static void BuildAsset(int type = 0)
    {
        //根据不同平台拼接不同平台导出路径
        string outPath = GetABPath();

        //如果不存在到处路径文件，创建一个
        if (!Directory.Exists(outPath))
        {
            Directory.CreateDirectory(outPath);
        }

        Debug.Log("OutPath : " + outPath);

        if (type == 0)       //增量打包
        {
            BuildPipeline.BuildAssetBundles(outPath, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        }
        else if (type == 1)  // 强制重新打包
        {
            BuildPipeline.BuildAssetBundles(outPath, BuildAssetBundleOptions.ForceRebuildAssetBundle, EditorUserBuildSettings.activeBuildTarget);
        }
        Debug.Log("资源打包完成");
    }

    // 打包场景
    public static void BuildScene()
    {
        //根据不同平台拼接不同平台导出路径
        string outPath = GetABPath();

        outPath = Path.Combine(outPath, "Scene");

        //如果不存在到处路径文件，创建一个
        if (!Directory.Exists(outPath))
        {
            Directory.CreateDirectory(outPath);
        }

		// 注意这里【区别】通常我们打包，第2个参数都是指定文件夹目录，在此方法中，此参数表示具体【打包后文件的名字】
		// 记得指定目标平台，不同平台的打包文件是不可以通用的。最后的BuildOptions要选择流格式
        // BuildPipeline.BuildPlayer(levels, Application.dataPath+"/Scene.unity3d", BuildTarget.Android, BuildOptions.BuildAdditionalStreamedScenes);

        List<string> scenePathList = GetBuildSettingsScenes();
        for (int i = 0; i < scenePathList.Count; ++i)
        {
            string path = Path.Combine(outPath, Path.GetFileNameWithoutExtension(scenePathList[i]) + ".unity3d");
            string[] scenes = { scenePathList[i] };

            BuildPipeline.BuildPlayer(scenes, path, EditorUserBuildSettings.activeBuildTarget, BuildOptions.BuildAdditionalStreamedScenes);
            EditorUtility.DisplayProgressBar("BuildPlayer", "BuildSceneAssetBundle", i * 1.0f / scenePathList.Count);
        }
        EditorUtility.ClearProgressBar();
        Debug.Log("场景打包完成");
    }

    public static Dictionary<string, bool> excludeScene = new Dictionary<string, bool> { { "InitScene", true} };
    public static List<string> GetBuildSettingsScenes()
    {
        string path = Path.Combine(Application.dataPath, "Scenes");
        string[] files = Directory.GetFiles(path, "*.unity", SearchOption.AllDirectories);

        List<string> scenePathList = new List<string>();
        for (int i = 0; i < files.Length; ++i)
        {
            string sceneName = Path.GetFileNameWithoutExtension(files[i]);
            if (excludeScene.ContainsKey(sceneName))
            {
                continue;
            }

            int index = files[i].LastIndexOf("Assets");
            if (index <= 0)
            {
                continue;
            }

            string scenePath = files[i].Substring(index);
            scenePathList.Add(scenePath);
        }
        return scenePathList;
    }

    public static void DeleteOldAB()
	{
		string outPath = GetABPath();
		if (Directory.Exists (outPath)) {
			DirectoryInfo dirInfo = new DirectoryInfo (outPath);
			dirInfo.Delete (true);
		}

		AssetDatabase.Refresh();
	}

	private static string GetABPath()
	{
		string path = Path.Combine(AssetBundleOutPsth, Plathform.GetPlatformFolder(EditorUserBuildSettings.activeBuildTarget));
		return path;
	}

    private static void ClearAssetBundlesName()
    {
        int length = AssetDatabase.GetAllAssetBundleNames().Length;
        string[] oldAssetBundleNames = new string[length];
        for (int i = 0; i < length; i++)
        {
            oldAssetBundleNames[i] = AssetDatabase.GetAllAssetBundleNames()[i];
        }

        for (int j = 0; j < oldAssetBundleNames.Length; j++)
        {
            Debug.LogError(oldAssetBundleNames[j]);
            AssetDatabase.RemoveAssetBundleName(oldAssetBundleNames[j], true);
        }
    }
}

//根据切换的平台返回相应的导出路径
public class Plathform
{
    public static string GetPlatformFolder(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.Android:   //Android平台导出到 Android文件夹中
                return "AssetBundle";
            case BuildTarget.iOS:
                return "AssetBundle";
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return "AssetBundle";
            case BuildTarget.StandaloneOSXIntel:
            case BuildTarget.StandaloneOSXIntel64:
            //case BuildTarget.StandaloneOSXUniversal:
                return "AssetBundle";
            default:
                return null;
        }
    }
}

public static class BuildAssetBundleVersion
{
    public static string assetPath = string.Empty;
    public static void BuildVersion()
    {
        assetPath = Path.Combine(Application.streamingAssetsPath, "AssetBundle");

        CreateVersionTXT();

        CreateUpdateTXT();

        AssetDatabase.Refresh();
    }

    //创建版本记录
    private static void CreateVersionTXT()
    {
        string versionPrefix = "Version:";
        string versionPath = Path.Combine(Application.streamingAssetsPath, "Version/version.txt");
        string versionContent = ReadTXT(versionPath);

        versionContent = versionContent.Substring(versionPrefix.Length);

        string[] versionArr = versionContent.Split('.');
        int value = 0;
        for (int i = 0; i < versionArr.Length; ++i)
        {
            value *= 10;
            value += int.Parse(versionArr[i]);
        }
        value += 1;

        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(versionPrefix);

        string versionName = value.ToString().PadLeft(3, '0');
        for (int i = 0;  i < versionName.Length; ++i)
        {
            string s = versionName.Substring(i, 1);
            stringBuilder.Append(s);
            if (i <versionName.Length - 1)
            {
                stringBuilder.Append(".");
            }
        }
         
        WriteTXT(versionPath, stringBuilder.ToString());
    }

    // 创建更新文本
    private static void CreateUpdateTXT()
    {
        //var files = Directory.GetFiles(assetPath, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".unity3d") || s.EndsWith(".manifest") || !s.EndsWith(".meta"));
        var files = Directory.GetFiles(assetPath, "*.*", SearchOption.AllDirectories).Where(s => !s.EndsWith(".meta"));

        StringBuilder stringBuilder = new StringBuilder();
        foreach (string filePath in files)
        {
            string md5 = BuildFileMd5(filePath);

            string fileName = filePath.Substring(filePath.LastIndexOf("AssetBundle\\") + ("AssetBundle\\").Length);
            stringBuilder.AppendLine( string.Format("{0}:{1}", fileName, md5));
        }

        string updatePath = Path.Combine(Application.streamingAssetsPath, "Version/update.txt");
        WriteTXT(updatePath, stringBuilder.ToString());
    }

    private static string BuildFileMd5(string filePath)
    {
        string fileMd5 = string.Empty;
        try
        {
            using (FileStream fs = File.OpenRead(filePath))
            {
                MD5 md5 = MD5.Create();
                byte[] fileMd5Bytes = md5.ComputeHash(fs);  // 计算FileStream 对象的哈希值
                fileMd5 = FormatMd5(fileMd5Bytes);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex);
        }

        return fileMd5;
    }

    private static string FormatMd5(byte[] bytes)
    {
        return System.BitConverter.ToString(bytes).Replace("-", "").ToLower();
    }

    private static void WriteTXT(string path, string content)
    {
        string directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        using (FileStream fs = File.Create(path))
        {
			StreamWriter sw = new StreamWriter(fs, Encoding.ASCII);
            try
            {
                sw.Write(content);

                sw.Close();
                fs.Close();
                fs.Dispose();
            }
            catch (IOException e)
            {
                Debug.Log(e.Message);
            }
        }

        //{
        //    FileStream fs = new FileStream(path, FileMode.Create);
        //    // 将字符串转换为字节数组
        //    byte[] data = Encoding.UTF8.GetBytes(content);
        //    // 写入
        //    fs.Write(data, 0, data.Length);
        //    // 清空缓冲区，关闭流
        //    fs.Flush();
        //    fs.Close();
        //}
    }

    private static string ReadTXT(string path)
    {
        string content = "Version:0.0.0";
        if (!File.Exists(path))
        {
            return content;
        }

        content = File.ReadAllText(path);

        return content;
    }
}