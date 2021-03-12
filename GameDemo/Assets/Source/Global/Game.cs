using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour {
    public static Game Instance;

    protected void Awake()
    {
        Instance = this;
        Init();

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update () {
        OnFrame();
    }

    private void Init()
    {
        AssetBundleManager.Instance.Init();
        LoadTableData.Instance.LoadAllTable();
    }

    private void OnFrame()
    {
        AssetBundleManager.Instance.OnFrame();
    }
}