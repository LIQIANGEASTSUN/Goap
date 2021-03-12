using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 伐木工 （砍树，得到原木）
/// </summary>
public class Logger : Labourer
{

    public override List<GoapAction> initAction(Transform tr)
    {
        List<GoapAction> goapActionList = new List<GoapAction>();

        // 砍树
        ChopTreeAction chopTreeAction = new ChopTreeAction(tr);
        goapActionList.Add(chopTreeAction);

        // 放下木头
        DropOffLogsAction dropOffLogsAction = new DropOffLogsAction(tr);
        goapActionList.Add(dropOffLogsAction);

        // 获取工具
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

