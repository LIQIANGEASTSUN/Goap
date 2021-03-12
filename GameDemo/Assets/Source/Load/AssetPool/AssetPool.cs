using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetPool
{
    public static AssetPool Instance = new AssetPool();


    /// <summary>
    /// 建筑
    /// </summary>
    private AssetSpawnPool _Build;

    /// <summary>
    /// 通用加载
    /// </summary>
    private AssetSpawnPool _Common;

    /// <summary>
    /// 场景资源
    /// </summary>
    private AssetSpawnPool _Scene;

    /// <summary>
    /// 数据配置表
    /// </summary>
    private AssetSpawnPool _TableData;

    /// <summary>
    /// Npc
    /// </summary>
    private AssetSpawnPool _Npc;
    
    /// <summary>
    /// UI
    /// </summary>
    private AssetSpawnPool _UI;

    /// <summary>
    /// 动态加载图片
    /// </summary>
    private AssetSpawnPool _UITexture;

    public AssetPool()
    {
        _Build = new AssetSpawnPool("Building");
        _Build.AssetPath = "AllAssets/Prefab/Building/";
        _Build.IsUsePool = false;
        _Build.IsAsynchLoad = true;
        _Build.Extension = ".prefab";

        _Common = new AssetSpawnPool("Common");
        _Common.AssetPath = "";
        _Common.IsUsePool = false;
        _Common.IsAsynchLoad = true;

        _Scene = new AssetSpawnPool("Scene");
        _Scene.AssetPath = "Scene/";
        _Scene.IsUsePool = false;
        _Scene.IsAsynchLoad = true;
        _Scene.Extension = ".unity";

        _TableData = new AssetSpawnPool("Table");
        _TableData.AssetPath = "AllAssets/Table/";
        _TableData.IsUsePool = false;
        _TableData.IsAsynchLoad = true;
        _TableData.Extension = ".txt";

        _Npc = new AssetSpawnPool("Npc");
        _Npc.AssetPath = "AllAssets/Prefab/NPC/";
        _Npc.IsUsePool = false;
        _Npc.IsAsynchLoad = true;
        _Npc.Extension = ".prefab";

        _UI = new AssetSpawnPool("UI");
        _UI.AssetPath = "AllAssets/Prefab/UI/";
        _UI.IsUsePool = false;
        _UI.IsAsynchLoad = true;
        _UI.Extension = ".prefab";

        _UITexture = new AssetSpawnPool("UITexture");
        _UITexture.AssetPath = "AllAssets/SpritePackage/";
        _UITexture.IsUsePool = false;
        _UITexture.IsAsynchLoad = true;
        _UITexture.Extension = ".png";
    }

    /// <summary>
    /// 建筑
    /// </summary>
    public AssetSpawnPool Build { get { return _Build; } }

    /// <summary>
    /// 通用加载
    /// </summary>
    public AssetSpawnPool Common { get { return _Common; } }

    /// <summary>
    /// 场景资源
    /// </summary>
    public AssetSpawnPool Scene { get { return _Scene; } }

    /// <summary>
    /// 数据配置表
    /// </summary>
    public AssetSpawnPool TableData { get { return _TableData; } }

    /// <summary>
    /// NPC
    /// </summary>
    public AssetSpawnPool Npc { get { return _Npc; } }

    /// <summary>
    /// UI
    /// </summary>
    public AssetSpawnPool UI { get { return _UI; } }

    /// <summary>
    /// UITexture
    /// </summary>
    public AssetSpawnPool UITexture { get { return _UITexture; } }

}