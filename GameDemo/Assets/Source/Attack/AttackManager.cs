using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Goap;

public class AttackManager {

    private GoapAgent goapAgent = null;

    public AttackManager(GoapAgent goapAgent)
    {
        this.goapAgent = goapAgent;
    }

    public Skill EnableAttack()
    {
        if (goapAgent == null || goapAgent.SkillManager == null)
        {
            return null;
        }
        return goapAgent.SkillManager.GetUseableSkill();
    }

    public bool InAttackRange()
    {
        if (goapAgent.Target == null)
        {
            return false;
        }

        float distance = Vector3.Distance(goapAgent.Target.transform.position, goapAgent.Position);

        Skill skill = EnableAttack();
        if (skill == null)
        {
            return true;
        }

        return distance <= skill.Range();
    }
}
