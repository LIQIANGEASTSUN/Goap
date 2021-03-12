using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// 房间中的玩家
/// </summary>
public class RoomPlayer
{
    // 阵营
    private Camp camp = Camp.Red;
    // 营地
    private Transform camping;

    private List<NpcData> npcDataList = new List<NpcData>();

    public RoomPlayer(Camp camp)
    {
        this.camp = camp;

        npcDataList = TableTool.GetTableData<NpcData>(TableType.Npc);
    }

    public void OnFrame()
    {
        GetCamping();

        //time += Time.deltaTime;
        //if (time > 1)
        //{
        //    time = -10000;
        //    CreateUnit();
        //}

        if (camp == Camp.Blue)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                CreateUnit(10001);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                CreateUnit(10010);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                CreateUnit(10020);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                CreateUnit(10001);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                CreateUnit(10010);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                CreateUnit(10020);
            }
        }
    }

    //private float time = 0;

    private void CreateUnit()
    {
        if (camping == null)
        {
            return;
        }

        int value = UnityEngine.Random.Range(0, npcDataList.Count);
        NpcData npcData = npcDataList[value];

        CreateUnit(npcData.id);
    }

    private void CreateUnit(int npcID)
    {
        NpcData npcData = TableTool.GetTableDataRow<NpcData>(TableType.Npc, npcID);
        if (npcData == null)
        {
            return;
        }

        string extend = Enum.GetName(typeof(Camp), camp);
        string prefab = string.Format("{0}_{1}", npcData.prefab, extend);

        int randomX = UnityEngine.Random.Range(-5, 5);
        UnitCreate.Instance.Create(prefab, npcData.id, Camp, new Vector3( camping.position.x + randomX, 0, camping.position.z));
    }

    private void GetCamping()
    {
        if (camping == null)
        {
            string campingName = Enum.GetName(typeof(Camp), camp);
            GameObject campingObj = GameObject.Find(campingName);
            if (campingObj != null)
            {
                camping = campingObj.transform;
            }
        }
    }

    /// <summary>
    /// 阵营
    /// </summary>
    public Camp Camp { get { return camp; } }
}