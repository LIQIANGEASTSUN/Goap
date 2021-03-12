using UnityEngine;
using Goap;

/// <summary>
/// 休闲行为
/// </summary>
public class GoapActionIdle : GoapAction {

    public GoapActionIdle(GoapAgent goapAgent, GoapActionManager goapActionManager) : base(goapAgent, goapActionManager)
    {
        cost = 100;
        stateEnum = StateEnum.Idle;
    }

    public override void InitStatus()
    {
        base.InitStatus();

        //preconditionsStatus.AddState();
        effectsStatus.AddState(GoapCondition.idle, true);
    }

    public override bool CheckProceduralPrecondition()
    {
        return true;
    }

    public override void Enter()
    {
        base.Enter();
        
    }

    public override void Run()
    {
        base.Run();


    }
}
