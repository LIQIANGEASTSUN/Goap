using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UIFrameWork;
using Goap;

public class HpControllerPanel : UIBase {
    public static HpControllerPanel instance = null;

    private string[] hpNames = new string[] { "HpBlue", "HpRed" };
    private Dictionary<string, Transform> hpDic = new Dictionary<string, Transform>();

    private Dictionary<string, Queue<Transform>> hpQueueDic = new Dictionary<string, Queue<Transform>>();
    private int cacheCount = 10;

    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }

    protected override void GetData()
    {
        
    }

    protected override void GetView()
    {
        for (int i = 0; i < hpNames.Length; ++i)
        {
            Transform hpTr = ToolsComponent.FindChildCom<Transform>(transform, hpNames[i]);
            if (hpTr != null)
            {
                hpDic[hpNames[i]] = hpTr;
            }
        }
    }

    public override void RefreshFixed()
    {
        
    }

    public Hp GetHp(GoapAgent goapAgent)
    {
        Transform hpTr = GetHpTransform(goapAgent);
        hpTr.gameObject.SetActive(true);

        Hp hp = hpTr.GetComponent<Hp>();
        hp.SetAgent(goapAgent);

        return hp;
    }

    private Transform GetHpTransform(GoapAgent goapAgent)
    {
        string hpName = string.Format("Hp{0}", Enum.GetName(typeof(Camp), goapAgent.Camp));

        if (!hpQueueDic.ContainsKey(hpName))
        {
            hpQueueDic[hpName] = new Queue<Transform>();
        }

        Transform hpTr = null;
        if (hpQueueDic[hpName].Count <= 0)
        {
            GameObject newHpGo = Instantiate(hpDic[hpName].gameObject) as GameObject;
            newHpGo.name = hpName;
            hpTr = newHpGo.transform;
            ToolsComponent.AddChild(transform, hpTr);
            hpTr.position = new Vector3(0, 1000, 0);
        }
        else
        {
            hpTr = hpQueueDic[hpName].Dequeue();
        }

        return hpTr;
    }

    public void ReleaseHp(Hp hp)
    {
        Transform hpTr = hp.transform;
        if (hpQueueDic[hpTr.name].Count <= cacheCount)
        {
            hpQueueDic[hpTr.name].Enqueue(hpTr);
            hpTr.gameObject.SetActive(false);
        }
        else
        {
            GameObject.Destroy(hpTr.gameObject);
        }
    }
}