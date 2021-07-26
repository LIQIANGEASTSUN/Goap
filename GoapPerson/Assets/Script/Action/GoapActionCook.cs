using UnityEngine;
using Goap;

public class GoapActionCook : GoapAction {

    public GoapActionCook(GoapGoal goapGoal) : base(goapGoal)
    {
        cost = 10;
    }

    public override void InitStatus()
    {
        base.InitStatus();

        preconditionsStatus.AddState(GoapCondition.hasFood, false);

        effectsStatus.AddState(GoapCondition.cooking, true);
    }

    public override bool CheckProceduralPrecondition()
    {
        GameObject cooking = GameObject.Find("Cooking");
        target = cooking.transform;
        return cooking != null;
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
            person.AddFood(1);
            person.Eat(-0.3f);
            person.AddHomeWork(1f);
            if (person.FoodEnougth())
            {
                Finish();
            }
        }
    }
}
