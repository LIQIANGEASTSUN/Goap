using UnityEngine;
using Goap;

public class GoapActionReset : GoapAction {

    public GoapActionReset(GoapGoal goapGoal):base(goapGoal)
    {
        cost = 30;
    }

    public override void InitStatus()
    {
        base.InitStatus();
        preconditionsStatus.AddState(GoapCondition.isTired, true);

        effectsStatus.AddState(GoapCondition.reset, true);
    }

    public override bool CheckProceduralPrecondition()
    {
        GameObject resetArea = GameObject.Find("ResetArea");
        target = resetArea.transform;
        return resetArea != null;
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

            person.Eat(-0.3f);
            person.AddHomeWork(1f);

            person.TakeReset(3);
            if (person.IsVigorFull())
            {
                Finish();
            }
        }
    }
}