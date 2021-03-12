using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// éÔ·ò £¨»ñÈ¡Ä¾²ñÉÕ»ð£©
/// </summary>
public class WoodCutter : Labourer
{
    public override List<GoapAction> initAction(Transform tr)
    {
        List<GoapAction> goapActionList = new List<GoapAction>();

        // ¼ñÄ¾²ñ
        ChopFirewoodAction chopFireWoodAction = new ChopFirewoodAction(tr);
        goapActionList.Add(chopFireWoodAction);

        // ¼ñÄ¾²ñ
        DropOffFirewoodAction dropOffFireWoodAction = new DropOffFirewoodAction(tr);
        goapActionList.Add(dropOffFireWoodAction);

        PickUpToolAction pickUpToolAction = new PickUpToolAction(tr);
        goapActionList.Add(pickUpToolAction);

        return goapActionList;
    }

    /**
	 * Our only goal will ever be to chop logs.
	 * The ChopFirewoodAction will be able to fulfill this goal.
	 */
    public override Dictionary<string, bool> createGoalState () {
		Dictionary<string, bool> goal = new Dictionary<string, bool>();
		goal.Add("collectFirewood", true);
		return goal;
	}
}