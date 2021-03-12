using UnityEngine;
using Goap;

/// <summary>
/// 攻击行为
/// </summary>
public class GoapActionAttack : GoapAction {

	public GoapActionAttack(GoapAgent goapAgent, GoapActionManager goapActionManager) : base(goapAgent, goapActionManager)
    {
        cost = 5;
        stateEnum = StateEnum.Attack;
    }

    public override void InitStatus()
    {
        base.InitStatus();

        preconditionsStatus.AddState(GoapCondition.hasEnemy, true);
        preconditionsStatus.AddState(GoapCondition.hasUsableSkill, true);
        preconditionsStatus.AddState(GoapCondition.inAttackRange, true);

        effectsStatus.AddState(GoapCondition.attackEnemy, true);
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

        //if (goapGoal.target == null)
        //{
        //    Fail();
        //    return;
        //}

        //if (goapGoal.GetType() == typeof(Person))
        //{
        //    Person person = (Person)goapGoal;
        //    person.AddFood(1);

        //    if (person.FoodEnougth())
        //    {
        //        Finish();
        //    }
        //}
    }
}