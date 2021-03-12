using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMStateMove : FSMStateBase {

    public FSMStateMove(GoapAgent goapAgent) : base(FSMStateEnum.Move, goapAgent)
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

        // move the game object
        GoapAction action = goapAgent.goapPlannerManager.currentActions.Peek();
        if (action.requiresInRange() && action.target == null)
        {
            Debug.Log("<color=red>Fatal error:</color> Action requires a target but has none. Planning failed. You did not assign the target in your Action.checkProceduralPrecondition()");
            /////fsm.popState(); // move
            /////fsm.popState(); // perform
            goapAgent.stateMachine.clearState();
            goapAgent.stateMachine.pushState(FSMStateEnum.Idle);
            return;
        }

        // get the agent to move itself
        if (goapAgent.dataProvider.moveAgent(action))
        {
            goapAgent.stateMachine.popState();
        }
    }

}
