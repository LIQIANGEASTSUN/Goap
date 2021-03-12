using UnityEngine;
using Goap;

public class GoapActionHomeWork : GoapAction {

	public GoapActionHomeWork(GoapGoal goapGoal):base(goapGoal)
    {
        cost = 40;
    }

    public override void InitStatus()
    {
        base.InitStatus();

        preconditionsStatus.AddState(GoapCondition.hasHomeWork, true);

        effectsStatus.AddState(GoapCondition.doHomeWork, true);
    }

    public override bool CheckProceduralPrecondition()
    {
        GameObject homeWork = GameObject.Find("HomeWork");
        target = homeWork.transform;
        return homeWork != null;
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
            person.AddHomeWork(-1f);
            if (person.homeWorkDone())
            {
                Finish();
            }
        }
    }

}
