using UnityEngine;
using Goap;

public class GoapActionPlayBall : GoapAction {

    public GoapActionPlayBall(GoapGoal goapGoal) : base(goapGoal)
    {
        cost = 50;
    }

    public override void InitStatus()
    {
        base.InitStatus();

        preconditionsStatus.AddState(GoapCondition.hasBall, true);

        effectsStatus.AddState(GoapCondition.playBall, true);
    }

    public override bool CheckProceduralPrecondition()
    {
        GameObject basketball = GameObject.Find("Basketball");
        target = basketball.transform;
        return basketball != null;
    }

    public override void Run()
    {
        if (!IsInRange())
        {
            MoveController.instance.Move(goapGoal.transform, target.position, 6);
            return;
        }
        base.Run();

        if (target == null)
        {
            Fail();
            return;
        }

        if (goapGoal.GetType() == typeof(Person))
        {
            Person person = (Person)goapGoal;
            person.TakeReset(-1);

            if (person.IsHungry() || person.IsTired())
            {
                Finish();
            }
        }
    }

}
