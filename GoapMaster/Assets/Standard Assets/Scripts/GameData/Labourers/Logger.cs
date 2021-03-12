using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ��ľ�� ���������õ�ԭľ��
/// </summary>
public class Logger : Labourer
{

    public override List<GoapAction> initAction(Transform tr)
    {
        List<GoapAction> goapActionList = new List<GoapAction>();

        // ����
        ChopTreeAction chopTreeAction = new ChopTreeAction(tr);
        goapActionList.Add(chopTreeAction);

        // ����ľͷ
        DropOffLogsAction dropOffLogsAction = new DropOffLogsAction(tr);
        goapActionList.Add(dropOffLogsAction);

        // ��ȡ����
        PickUpToolAction pickUpToolAction = new PickUpToolAction(tr);
        goapActionList.Add(pickUpToolAction);

        return goapActionList;
    }

    /**
	 * Our only goal will ever be to chop trees.
	 * The ChopTreeAction will be able to fulfill this goal.
	 */
    public override Dictionary<string, bool> createGoalState () {
		Dictionary<string, bool> goal = new Dictionary<string, bool>();
		
		goal.Add("collectLogs", true);
		return goal;
	}

}

