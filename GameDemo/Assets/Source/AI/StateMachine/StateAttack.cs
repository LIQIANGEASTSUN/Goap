using Goap;
using UnityEngine;

public class StateAttack : StateBase {
    private Skill skill = null;
    private float endTime = 0;

    public StateAttack(GoapAgent goapAgent) :base(StateEnum.Attack, goapAgent)
    {

    }

    public override void OnEnter()
    {
        skill = goapAgent.AttackManager.EnableAttack();

        base.OnEnter();
        float animationLength = goapAgent.AnimationManager.GetAnimationLength(GetAnimationName());
        endTime = Time.realtimeSinceStartup + animationLength;

        skill.Reset();
        goapAgent.AttackUseSkill = skill;
    }

    public override void OnExecute()
    {
        base.OnExecute();

        if (endTime > Time.realtimeSinceStartup)
        {
            return;
        }

        ResetTime();
        if (goapAction != null)
        {
            goapAction.Finish();
        }

        DamagerController.Damage(goapAgent, goapAgent.Target, skill.SkillData.id);
    }

    public override void OnExit()
    {
        base.OnExit();
        ResetTime();
    }

    private void ResetTime()
    {
        endTime = Time.realtimeSinceStartup + int.MaxValue; // 放置当前还没推出，下一帧又执行
    }

    protected override string GetAnimationName()
    {
        if (skill.Index == 0)
        {
            return base.GetAnimationName();
        }
        string animName = string.Format("{0}{1}", base.GetAnimationName(), skill.Index);
        return animName;
    }
}