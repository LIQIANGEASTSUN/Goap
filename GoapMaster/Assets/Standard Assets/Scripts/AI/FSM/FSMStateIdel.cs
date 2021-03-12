using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMStateIdel : FSMStateBase {

	public FSMStateIdel( GoapAgent goapAgent) :base( FSMStateEnum.Idle, goapAgent)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void Update()
    {
        base.Update();

        goapAgent.goapPlannerManager.GetPlanner();
    }

}
