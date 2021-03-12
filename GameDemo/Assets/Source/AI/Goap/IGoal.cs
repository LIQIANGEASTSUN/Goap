
namespace Goap
{
    public interface IGoal
    {
        /// <summary>
        /// 设置目标
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="value"></param>
        void SetGoal(GoapCondition condition, object value);

        /// <summary>
        /// 获取目标
        /// </summary>
        /// <returns></returns>
        GoapStatus GetGoalStatus();
    }
}