using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class BuildAssetBundleWindow : EditorWindow {

	private static BuildAssetBundleWindow window;

	public static bool enableBuild = true;
	[MenuItem("Tool/AssetBundle")]
	static void Init()
	{
		window = (BuildAssetBundleWindow)EditorWindow.GetWindow (typeof(BuildAssetBundleWindow));
		window.position = new Rect (100, 100, 500, 500);
		window.titleContent = new GUIContent ("打资源AB包");

        isLoadScene = false;

        window.Show ();
        enableBuild = true;
	}
		
	void OnGUI()
	{
		GUILayout.Space (15);

		EditorGUILayout.BeginVertical ("box");
		GUILayout.Label ("指定打包路径：");
		for (int i = 0; i < BuideABPath.includePathList.Count; ++i) 
		{
			GUILayout.Label (BuideABPath.includePathList[i], EditorStyles.boldLabel);
			GUILayout.Space (5);
		}

		EditorGUILayout.EndVertical ();

        GUILayout.Space(10);
        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("指定打包的场景：(添加到 Build Settings 且未勾选的场景)");
        DrawScene();
        EditorGUILayout.EndVertical();

        GUILayout.Space (10);

		GUI.enabled = enableBuild;
		//EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("一键增量打包", GUILayout.ExpandWidth(true), GUILayout.Height(30))) {
			enableBuild = false;
			BuildAssetBundle.BuildABAsset (0);
			enableBuild = true;
		}

		GUILayout.Space (20);
		if (GUILayout.Button ("一键重新打包", GUILayout.ExpandWidth(true), GUILayout.Height(30))) {
			enableBuild = false;
			BuildAssetBundle.BuildABAsset (1);
			enableBuild = true;
		}

        GUILayout.Space(20);
        if (GUILayout.Button("清除旧资源包", GUILayout.ExpandWidth(true), GUILayout.Height(30)))
        {
            BuildAssetBundle.DeleteOldAB();
        }

        GUILayout.Space(20);
        if (GUILayout.Button("清理缓存", GUILayout.ExpandWidth(true), GUILayout.Height(30)))
        {
            Caching.CleanCache();
            Debug.LogError("Caching.CleanCache()");
        }

        GUILayout.Space(20);
        if (GUILayout.Button("Version", GUILayout.ExpandWidth(true), GUILayout.Height(30)))
        {
            BuildAssetBundleVersion.BuildVersion();
        }

        //EditorGUILayout.EndHorizontal ();
        GUI.enabled = true;

		if (Event.current.type == EventType.MouseMove) 
		{
			Repaint ();
		}
	}

    private List<SceneAsset> sceneAssetList = new List<SceneAsset>();
    private static bool isLoadScene = false;
    private void LoadAssetScene()
    {
        if (isLoadScene)
        {
            return;
        }

        isLoadScene = true;

        sceneAssetList.Clear();

        List<string> scenePathList = BuildAssetBundle.GetBuildSettingsScenes();
        for (int i = 0; i < scenePathList.Count; ++i)
        {
            SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePathList[i]);
            sceneAssetList.Add(sceneAsset);
        }
    }

    private void DrawScene()
    {
        LoadAssetScene();

        for (int i = 0; i < sceneAssetList.Count; ++i)
        {
            EditorGUILayout.ObjectField(new GUIContent("Scene:" + i.ToString()), sceneAssetList[i], typeof(SceneAsset), false);
        }

        if (sceneAssetList.Count <= 0)
        {
            GUILayout.Space(8);
            GUILayout.Label("没有要打包的场景");
        }
    }
}
