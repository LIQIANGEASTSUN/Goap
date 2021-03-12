using Goap;

public class StateRun : StateBase {

	public StateRun(GoapAgent goapAgent) :base(StateEnum.Run, goapAgent)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExecute()
    {
        base.OnExecute();

        if (!goapAgent.HasTarget)
        {
            if (goapAction != null)
            {
                goapAction.Fail();
            }
            return;
        }

        goapAgent.LookTarget();
        float distance = MoveController.instance.Move(goapAgent.transform, goapAgent.Target.transform.position, goapAgent.NpcData.speed);
        if (distance <= 3)
        {
            if (goapAction != null)
            {
                goapAction.Finish();
            }
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}