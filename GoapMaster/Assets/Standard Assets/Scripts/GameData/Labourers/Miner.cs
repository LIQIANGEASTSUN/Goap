using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 矿工 （挖铁矿）
/// </summary>
public class Miner : Labourer
{
    public override List<GoapAction> initAction(Transform tr)
    {
        List<GoapAction> goapActionList = new List<GoapAction>();

        // 获取工具
        PickUpToolAction pickUpToolAction = new PickUpToolAction(tr);
        goapActionList.Add(pickUpToolAction);

        // 挖矿石
        MineOreAction mineOreAction = new MineOreAction(tr);
        goapActionList.Add(mineOreAction);

        // 放下铁
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

