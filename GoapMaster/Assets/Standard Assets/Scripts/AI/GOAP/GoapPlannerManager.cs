using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoapPlannerManager {
    private GoapAgent goapAgent = null;
    public GoapPlanner goapPlanner = new GoapPlanner();

    public List<GoapAction> availableActions = new List<GoapAction>();
    public Queue<GoapAction> currentActions = new Queue<GoapAction>();

    public GoapPlannerManager(GoapAgent goapAgent)
    {
        this.goapAgent = goapAgent;
        loadActions();
    }

    public void Update()
    {

    }

    private void loadActions()
    {
        availableActions = goapAgent.dataProvider.initAction(goapAgent.transform);
        GoapAction[] actions = availableActions.ToArray();
        Debug.Log("Found actions: " + GoapAgent.prettyPrint(actions));
    }

    public bool hasActionPlan()
    {
        return currentActions.Count > 0;
    }

    public void GetPlanner()
    {
        // GOAP planning
        // get the world state and the goal we want to plan for
        Dictionary<string, bool> worldState = goapAgent.dataProvider.getWorldState();
        Dictionary<string, bool> goal = goapAgent.dataProvider.createGoalState();

        // Plan
        Queue<GoapAction> executeActionQueue = goapPlanner.plan(goapAgent.gameObject, availableActions, worldState, goal);
        if (executeActionQueue != null && executeActionQueue.Count > 0)
        {
            // we have a plan, hooray!
            currentActions = executeActionQueue;
            goapAgent.stateMachine.clearState();
            goapAgent.stateMachine.pushState(FSMStateEnum.Perform);
        }
        else
        {
            // ugh, we couldn't get a plan
            Debug.Log("<color=orange>Failed Plan:</color>" + GoapAgent.prettyPrint(goal));
            goapAgent.stateMachine.clearState();
            goapAgent.stateMachine.pushState(FSMStateEnum.Idle);
        }

    }

}
