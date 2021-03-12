public enum GoapCondition  {

    none            = 0,

    #region 条件、状态
    /// <summary>
    /// 有食物
    /// </summary>
    hasFood         = 1,
    /// <summary>
    /// 饿了
    /// </summary>
    isHungry        = 2,

    /// <summary>
    /// 有球
    /// </summary>
    hasBall         = 5,

    /// <summary>
    /// 累了
    /// </summary>
    isTired         = 6,

    /// <summary>
    /// 家庭作业
    /// </summary>
    hasHomeWork     = 8,
    #endregion


    #region  行为目标
    /// <summary>
    /// 吃东西
    /// </summary>
    eat             = 10000,

    /// <summary>
    /// 打球
    /// </summary>
    playBall        = 10001,

    /// <summary>
    /// 休息
    /// </summary>
    reset           = 10002,

    /// <summary>
    /// 写作业
    /// </summary>
    doHomeWork      = 10003,

    /// <summary>
    /// 做饭
    /// </summary>
    cooking         = 10004,

    #endregion

}
