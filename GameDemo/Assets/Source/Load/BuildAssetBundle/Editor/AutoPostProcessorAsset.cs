using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public static class BuideABPath
{
	// 只打包列表中包含的目录
	public static List<string> includePathList = new List<string> (){ "Assets/AllAssets" };
}

public class AutoPostProcessorAsset : AssetPostprocessor {

	static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		for (int i = 0; i < importedAssets.Length; ++i)
		{
			SetAssetBundleName (importedAssets[i]);
		}

		for (int i = 0; i < movedAssets.Length; ++i) 
		{
			SetAssetBundleName (movedAssets[i]);
		}
	}

	private static void SetAssetBundleName(string path)
	{
		if (!CheckAsset (path)) {
			return;
		}

		AssetImporter assetImport = AssetImporter.GetAtPath (path);
		if (assetImport == null) {
			return;
		}

        path = path.Substring(7);
        path = Path.ChangeExtension (path, "unity3d");
		assetImport.assetBundleName = path;
	}

	// 判断是否为有效的文件和路径
	private static bool CheckAsset(string path)
	{
        if (!IsAvlidPath(path))
        {
            return false;
        }

        // 排除 C# 脚本（C#脚本不能被打包）
        if (path.EndsWith (".cs") || path.EndsWith(".meta")) {
			return false;
		}

		// 没有后缀（文件夹）为无效
		if (!Path.HasExtension (path)) {
			return false;
		}

		return true;
	}

	// 判断是否为有效的目录
	private static bool IsAvlidPath(string path)
	{
		// 不是 Assets 目录下的跳过
		if (!path.Contains ("Assets")) {
			return false;
		}

		for (int i = 0; i < BuideABPath.includePathList.Count; ++i) {
			if (path.StartsWith(BuideABPath.includePathList [i])) {
				return true;
			}
		}

		return false;
	}
}
