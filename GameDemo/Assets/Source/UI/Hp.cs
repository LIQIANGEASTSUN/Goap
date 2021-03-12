using Goap;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class Hp : ViewController
{ 
    private GoapAgent goapAgent;
    private Follow follow = null;

    private HpSlider hpSlider = null;

    private Transform textItem = null;
    private Queue<HpText> hpTextQueue = new Queue<HpText>();
    private List<HpText> useHpTextList = new List<HpText>();

    private void Awake()
    {
        GetView();
    }

    public void SetAgent(GoapAgent goapAgent)
    {
        this.goapAgent = goapAgent;
        Transform hpTarget = ToolsComponent.FindChildCom<Transform>(goapAgent.transform, "Hp");
        follow.SetTarget(hpTarget);
    }

    protected override void GetView()
    {
        follow = ToolsComponent.GetComponent<Follow>(transform);
        hpSlider = ToolsComponent.FindChildCom<HpSlider>(transform, "HpSlider");

        textItem = ToolsComponent.FindChildCom<Transform>(transform, "HpText");
    }

    protected override void GetData()
    {
        
    }

    public override void RefreshFixed()
    {
        
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
    }

    public void SetHp(int value)
    {
        float hpValue = (goapAgent.Hp * 1.0f) / goapAgent.NpcData.hp;
        hpSlider.SetValue(hpValue);

        HpText hpText = GetText();
        if (hpText != null)
        {
            hpText.gameObject.SetActive(true);
            hpText.SetValue(value);

            useHpTextList.Add(hpText);
        }
    }

    private HpText GetText()
    {
        if (hpTextQueue.Count > 0)
        {
            return hpTextQueue.Dequeue();
        }

        if (textItem == null)
        {
            return null;
        }

        Transform item = ToolsComponent.CloneItem(transform, textItem);
        HpText hpText = item.GetComponent<HpText>();
        hpText.SetHp(this);
        return hpText;
    }

    public void ReleaseText(HpText hpText)
    {
        hpText.Init();
        hpText.gameObject.SetActive(false);
        hpTextQueue.Enqueue(hpText);

        useHpTextList.Remove(hpText);
    }

    public void Release()
    {
        if (goapAgent == null || !goapAgent.IsAlive())
        {
            follow.SetPos(new Vector3(0, 1000, 0));
        }

        for (int i = useHpTextList.Count - 1; i >= 0; --i)
        {
            ReleaseText(useHpTextList[i]);
        }

        HpControllerPanel.instance.ReleaseHp(this);
        hpSlider.SetValue(1);
    }
}