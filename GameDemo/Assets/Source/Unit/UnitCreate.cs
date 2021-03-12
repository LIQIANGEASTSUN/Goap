using UnityEngine;
using Goap;

public class UnitCreate
{
    public static readonly UnitCreate Instance = new UnitCreate();

    public void Create(string unitName, int npcID, Camp camp, Vector3 position)
    {
        AssetPool.Instance.Npc.LoadAsset<GameObject>(unitName, LoadCallBack, new object[] { npcID, unitName, camp, position}, true);
    }

    private void LoadCallBack(HandlerParam p_handleParam)
    {
        if (p_handleParam.assetObj == null)
        {
            return;
        }

        object[] paramArr = (object[])p_handleParam.paramObj;

        int npcID = (int)paramArr[0];
        string unitName = (string)paramArr[1];
        Camp camp = (Camp)paramArr[2];
        Vector3 position = (Vector3)paramArr[3];

        GameObject go = GameObject.Instantiate(p_handleParam.assetObj) as GameObject;
        go.transform.position = position;
        go.name = unitName;

        GoapAgent goapAgent = go.GetComponent<GoapAgent>();
        goapAgent.Camp = camp;
        goapAgent.NpcID = npcID;

        UnitManager.AddMonster(goapAgent);
    }
}