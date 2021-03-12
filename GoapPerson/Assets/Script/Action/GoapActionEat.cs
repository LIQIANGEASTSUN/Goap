using UnityEngine;
using Goap;

public class GoapActionEat : GoapAction {

	public GoapActionEat(GoapGoal goapGoal):base(goapGoal)
    {
        cost = 20;
    }

    public override void InitStatus()
    {
        base.InitStatus();

        preconditionsStatus.AddState(GoapCondition.hasFood, true);
        preconditionsStatus.AddState(GoapCondition.isHungry , true);

        effectsStatus.AddState(GoapCondition.eat, true);
    }

    public override bool CheckProceduralPrecondition()
    {
        GameObject food = GameObject.Find("Food");
        target = food.transform;
        return food != null;
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
            if (!person.IsEatFull())
            {
                person.Eat(3);
                person.AddFood(-3);
            }
            else
            {
                Finish();
            }

            if (!person.HasFood())
            {
                Fail();
            }
        }
    }
}
