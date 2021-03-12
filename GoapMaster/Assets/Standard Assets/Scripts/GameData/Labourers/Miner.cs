using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// �� ��������
/// </summary>
public class Miner : Labourer
{
    public override List<GoapAction> initAction(Transform tr)
    {
        List<GoapAction> goapActionList = new List<GoapAction>();

        // ��ȡ����
        PickUpToolAction pickUpToolAction = new PickUpToolAction(tr);
        goapActionList.Add(pickUpToolAction);

        // �ڿ�ʯ
        MineOreAction mineOreAction = new MineOreAction(tr);
        goapActionList.Add(mineOreAction);

        // ������
        DropOffOreAction dropOffOreAction = new DropOffOreAction(tr);
        goapActionList.Add(dropOffOreAction);

        return goapActionList;
    }

    /**
	 * Our only goal will ever be to mine ore.
	 * The MineOreAction will be able to fulfill this goal.
	 */
    public override Dictionary<string, bool> createGoalState () {
		Dictionary<string, bool> goal = new Dictionary<string, bool>();
		goal.Add("collectOre", true);
		return goal;
	}

}

