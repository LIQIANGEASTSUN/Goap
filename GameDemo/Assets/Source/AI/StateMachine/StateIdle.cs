using Goap;

public class StateIdle : StateBase{

	public StateIdle(GoapAgent goapAgent) :base(StateEnum.Idle, goapAgent)
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExecute()
    {
        base.OnExecute();

        if (goapAgent != null && !goapAgent.HasTarget)
        {
            Skill skill = goapAgent.AttackManager.EnableAttack();
            if (skill != null)
            {
                SkillData skillData = skill.SkillData;
                goapAgent.Target = UnitSearch.Instance.Search(goapAgent, (CampRelations)skillData.attackType);
            }
        }

        if (goapAgent.HasTarget)
        {
            if (goapAction != null)
            {
                goapAction.Fail();
            }
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
