using System.Collections.Generic;
using Goap;

public class GoapPlanManager {
    private GoapAgent goapAgent;
    private GoapPlan goapPlan;

    public GoapPlanManager(GoapAgent goapAgent)
    {
        this.goapAgent = goapAgent;
        Init();
    }

    private void Init()
    {
        goapPlan = new GoapPlan();
    }

    public GoapAction GetPerformerAction()
    {
        Queue<GoapAction> goapActionQueue = new Queue<GoapAction>();
        GoapAction goapAction = null;
        if (goapActionQueue.Count <= 0)
        {
            GoapStatus worldStatus = goapAgent.GoapStateManager.GetWorldStatus();
            GoapStatus goalStatus = goapAgent.GoapStateManager.GetGoalStatus();

            List<GoapAction> usableGoapActionList = goapAgent.GoapActionManager.GetActions();
            goapActionQueue = goapPlan.Plan(usableGoapActionList, worldStatus, goalStatus);
        }

        if (goapActionQueue.Count <= 0)
        {
            return goapAction;
        }

        goapAction = goapActionQueue.Dequeue();
        return goapAction;
    }
}