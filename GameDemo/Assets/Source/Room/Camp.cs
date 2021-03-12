/// <summary>
/// 阵营
/// </summary>
public enum Camp
{
    /// <summary>
    /// 红方
    /// </summary>
    Red  = 1 << 0,

    /// <summary>
    /// 蓝方
    /// </summary>
    Blue = 1 << 1,
}

/// <summary>
/// 阵营关系
/// </summary>
public enum CampRelations
{
    /// <summary>
    /// 敌对
    /// </summary>
    Enemy = 1 << 0,

    /// <summary>
    /// 友好
    /// </summary>
    Friend = 1 << 1,
}

public static class UnitCampRelations
{
    public static CampRelations GetRelations(Camp a, Camp b)
    {
        if ((a & b) == a)
        {
            return CampRelations.Friend;
        }

        return CampRelations.Enemy;
    }
}