using System.Collections.Generic;
using Goap;

public class GoapPlanManager {

    private GoapGoal goapGoal;
    private GoapPlan goapPlan;

    private List<GoapAction> usableGoapActionList = new List<GoapAction>();

    private Queue<GoapAction> goapActionQueue = new Queue<GoapAction>();

    public GoapPlanManager(GoapGoal goapGoal)
    {
        this.goapGoal = goapGoal;
        Init();
    }

    private void Init()
    {
        goapPlan = new GoapPlan();
        usableGoapActionList = goapGoal.GetActions();
    }

    public GoapAction GetPerformerAction()
    {
        GoapAction goapAction = null;
        if (goapActionQueue.Count <= 0)
        {
            GoapStatus worldStatus = goapGoal.GetWorldStatus();
            GoapStatus goalStatus = goapGoal.GetGoalStatus();

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