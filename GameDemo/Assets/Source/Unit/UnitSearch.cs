using Goap;
using System.Collections.Generic;
using UnityEngine;

public class UnitSearch  {
    public static readonly UnitSearch Instance = new UnitSearch();

	public GoapAgent Search(GoapAgent goapAgent, CampRelations campRelations)
    {
        List<GoapAgent> monsterList = UnitManager.MonsterList;

        Camp selfCamp = goapAgent.Camp;
        GoapAgent target = null;
        float distance = 0;
        for (int i = monsterList.Count - 1; i >= 0; --i)
        {
            GoapAgent monsterAgent = monsterList[i];
            if (monsterAgent == null || !monsterAgent.IsAlive())
            {
                monsterList.RemoveAt(i);
                continue;
            }

            Camp camp = monsterAgent.Camp;
            CampRelations relations = UnitCampRelations.GetRelations(selfCamp, camp);
            if ((campRelations & relations) != campRelations)
            {
                continue;
            }

            if (target == null)
            {
                target = monsterAgent;
                distance = Vector3.Distance(goapAgent.transform.position, target.transform.position);
                continue;
            }

            float dis = Vector3.Distance(goapAgent.transform.position, monsterAgent.transform.position);
            if (dis < distance)
            {
                target = monsterAgent;
                distance = dis;
            }
        }

        return target;
    }
}