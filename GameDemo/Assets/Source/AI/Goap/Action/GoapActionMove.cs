using UnityEngine;
using Goap;

/// <summary>
/// 移动行为
/// </summary>
public class GoapActionMove : GoapAction {

	public GoapActionMove(GoapAgent goapAgent, GoapActionManager goapActionManager) : base(goapAgent, goapActionManager)
    {
        cost = 1;
        stateEnum = StateEnum.Run;
    }

    public override void InitStatus()
    {
        base.InitStatus();

        preconditionsStatus.AddState(GoapCondition.hasEnemy, true);
        preconditionsStatus.AddState(GoapCondition.inAttackRange, false);

        effectsStatus.AddState(GoapCondition.inAttackRange, true);
    }

    public override bool CheckProceduralPrecondition()
    {
        //return goapGoal.target != null && !IsInRange();
        return true;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Run()
    {
        base.Run();

        //if (goapGoal.target == null)
        //{
        //    Fail();
        //}

        //MoveController.instance.Move(goapGoal.transform, goapGoal.target.transform.position, 1);
    }
}
