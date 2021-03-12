using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMStatePerform : FSMStateBase {

	public FSMStatePerform(GoapAgent goapAgent) :base (FSMStateEnum.Perform, goapAgent)
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

        // perform the action
        if (!goapAgent.goapPlannerManager.hasActionPlan())
        {
            // no actions to perform
            Debug.Log("<color=red>Done actions</color>");
            //////fsm.popState();
            goapAgent.stateMachine.clearState();
            goapAgent.stateMachine.pushState(FSMStateEnum.Idle);
            goapAgent.dataProvider.actionsFinished();
            return;
        }

        GoapAction action = goapAgent.goapPlannerManager.currentActions.Peek();
        if (action.isDone())
        {
            // the action is done. Remove it so we can perform the next one
            goapAgent.goapPlannerManager.currentActions.Dequeue();
        }

        if (goapAgent.goapPlannerManager.hasActionPlan())
        {
            // perform the next action
            action = goapAgent.goapPlannerManager.currentActions.Peek();
            bool inRange = action.requiresInRange() ? action.isInRange() : true;

            if (inRange)
            {
                // we are in range, so perform the action
                bool success = action.perform(goapAgent.gameObject);

                if (!success)
                {
                    // action failed, we need to plan again
                    goapAgent.stateMachine.popState();
                    goapAgent.stateMachine.pushState(FSMStateEnum.Idle);
                    goapAgent.dataProvider.planAborted(action);
                }
            }
            else
            {
                // we need to move there first
                // push moveTo state
                goapAgent.stateMachine.pushState(FSMStateEnum.Move);
            }
        }
        else
        {
            // no actions left, move to Plan state
            //goapAgent.stateMachine.popState(FSMStateEnum.None);
            goapAgent.stateMachine.clearState();
            goapAgent.stateMachine.pushState(FSMStateEnum.Idle);
            goapAgent.dataProvider.actionsFinished();
        }

    }


}
