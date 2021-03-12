//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Goap;

//public class GoapActionMove : GoapAction {

//    private float speed = 3;

//    public GoapActionMove(GoapGoal goapGoal):base(goapGoal)
//    {
//        cost = 1;
//    }

//    public override void InitStatus()
//    {
//        base.InitStatus();

//        preconditionsStatus.AddState(GoapCondition.inRange, false);

//        effectsStatus.AddState(GoapCondition.inRange, true);
//    }

//    public override bool CheckProceduralPrecondition()
//    {
//        return true;
//    }

//    public override void Run()
//    {
//        base.Run();

//        if (target == null)
//        {
//            Fail();
//            return;
//        }

//        Vector3 moveDir = (target.position - goapGoal.transform.position).normalized;
//        goapGoal.transform.Translate(moveDir * speed * Time.deltaTime, Space.World);

//        float distance = Vector3.Distance(goapGoal.transform.position, target.position);
//        if (distance <= 0.5f)
//        {
//            Finish();
//        }
//    }

//}
