using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstRun : MonoBehaviour {
    
    private int count = 0;
    private void Awake()
    {
    }

    private void Start()
    {
        LoadGlobalPrefabs();
    }

    private void LoadGlobalPrefabs()
    {
        // 加载资源 count + 1
        ++count;
        AssetPool.Instance.Common.LoadAsset<GameObject>("AllAssets/Prefab/DontDestroy/DontDestroy.prefab", LoadDontDestroyCallBack, null);
    }

    private void OnLoadedGlobalPrefabs()
    {
        // 全部加载完成
        ManagerScene.LoadScene(SceneEnum.Login);
    }

    private void LoadDontDestroyCallBack(HandlerParam handlerParam)
    {
        if (handlerParam.assetObj == null)
        {
            return;
        }

        GameObject go = Instantiate(handlerParam.assetObj) as GameObject;
        go.AddComponent<DontDestroy>();

        // 加载资源完成 count - 1
        --count;
        if (count > 0)
        {
            return;
        }

        OnLoadedGlobalPrefabs();
    }

}
