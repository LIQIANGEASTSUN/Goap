using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Goap;

public class GoapStateManager : IGoal, IStatus
{
    private GoapAgent goapAgent;

    private GoapStatus worldStats = new GoapStatus();  // 外部环境状态
    private GoapStatus goalStatus = new GoapStatus();  // 要完成的目标
    
    public GoapStateManager(GoapAgent goapAgent)
    {
        this.goapAgent = goapAgent;

        SetGoal(GoapCondition.attackEnemy, true);
        SetGoal(GoapCondition.inAttackRange, true);
        SetGoal(GoapCondition.idle, true);
    }

    public void OnFrame()
    {
        UpdateStatus();
    }

    /// <summary>
    /// 设置目标
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="value"></param>
    public void SetGoal(GoapCondition condition, object value)
    {
        goalStatus.AddState(condition, value);
    }

    /// <summary>
    /// 获取目标
    /// </summary>
    /// <returns></returns>
    public GoapStatus GetGoalStatus()
    {
        return goalStatus;
    }

    public void UpdateStatus()
    {
        worldStats.AddState(GoapCondition.hasEnemy, goapAgent.HasTarget);
        worldStats.AddState(GoapCondition.inAttackRange, goapAgent.AttackManager.InAttackRange());
        worldStats.AddState(GoapCondition.hasUsableSkill, goapAgent.AttackManager.EnableAttack() != null);
    }

    public GoapStatus GetWorldStatus()
    {
        return worldStats;
    }
}
