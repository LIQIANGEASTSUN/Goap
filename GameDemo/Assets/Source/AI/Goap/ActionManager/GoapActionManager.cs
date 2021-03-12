using System.Collections.Generic;
using Goap;

public class GoapActionManager : IAction
{
    private GoapAgent goapAgent;
    private GoapAction currentGoapAction = null;
    private List<GoapAction> goapActionList = new List<GoapAction>();

    public GoapActionManager(GoapAgent goapAgent)
    {
        this.goapAgent = goapAgent;

        GoapActionIdle goapActionIdle = new GoapActionIdle(this.goapAgent, this);
        SetActions(goapActionIdle);

        GoapActionMove goapActionMove = new GoapActionMove(this.goapAgent, this);
        SetActions(goapActionMove);

        GoapActionAttack goapActionAttack = new GoapActionAttack(this.goapAgent, this);
        SetActions(goapActionAttack);
    }

    public void SetActions(GoapAction goapAction)
    {
        goapActionList.Add(goapAction);
    }

    public List<GoapAction> GetActions()
    {
        return goapActionList;
    }

    public void OnFrame()
    {
        if (currentGoapAction != null)
        {
            currentGoapAction.Run();
        }
        else
        {
            ChangeAction();
        }
    }

    public void ActionFinishCallBack(GoapAction goapAction)
    {
        EndAction(goapAction);
    }

    public void ActionFailCallBack(GoapAction goapAction)
    {
        EndAction(goapAction);
    }

    private void EndAction(GoapAction goapAction)
    {
        if (currentGoapAction != goapAction)
        {
            return;
        }

        currentGoapAction = null;
        ChangeAction();
    }

    private void ChangeAction()
    {
        currentGoapAction = goapAgent.GoapPlanManager.GetPerformerAction();
        if (currentGoapAction != null)
        {
            currentGoapAction.Enter();
        }
    }
}