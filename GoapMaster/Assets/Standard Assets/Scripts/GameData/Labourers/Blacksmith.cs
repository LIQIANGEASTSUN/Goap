using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 铁匠（制造工具）
/// </summary>
public class Blacksmith : Labourer
{

    public override List<GoapAction> initAction(Transform tr)
    {
        List<GoapAction> goapActionList = new List<GoapAction>();

        // 锻造工具
        ForgeToolAction fogeToolAction = new ForgeToolAction(tr);
        goapActionList.Add(fogeToolAction);

        // 放下工具
        DropOffToolsAction dropOffToolsAction = new DropOffToolsAction(tr);
        goapActionList.Add(dropOffToolsAction);

        // 获取铁
        PickUpOreAction pickUpOreAction = new PickUpOreAction(tr);
        goapActionList.Add(pickUpOreAction);

        return goapActionList;
    }


    /**
	 * Our only goal will ever be to make tools.
	 * The ForgeTooldAction will be able to fulfill this goal.
	 */
    public override Dictionary<string, bool> createGoalState () {
		Dictionary<string, bool> goal = new Dictionary<string, bool>();
		goal.Add("collectTools", true);

		return goal;
	}
}

