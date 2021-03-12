public enum GoapCondition  {

    none            = 0,

    #region 条件、状态
    /// <summary>
    /// 有敌人
    /// </summary>
    hasEnemy       = 1,

    /// <summary>
    /// 有可用技能
    /// </summary>
    hasUsableSkill       = 2,

    /// <summary>
    /// 在攻击范围内
    /// </summary>
    inAttackRange        = 3,

    #endregion


    #region  行为目标
    /// <summary>
    /// 攻击敌人
    /// </summary>
    attackEnemy = 10001,

    /// <summary>
    /// 休闲
    /// </summary>
    idle        = 10002,

    #endregion
}