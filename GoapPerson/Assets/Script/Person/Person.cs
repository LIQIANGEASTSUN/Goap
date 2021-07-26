using Goap;
using UnityEngine;

public class Person : GoapGoal {

    private float vigor = 10;        // 精力     ：休息能加精力
    private float minVigor = 2;      // 精力下限 ：精力低于该值表示累了
    private float maxVigor = 10;     // 精力上限 ：休息足够时精力

    private float energy = 5;         // 能量     ：吃东西增加能量
    private float minEnergy = 2;     // 能量下限 ：能量低于该值表示饿了
    private float maxEnergy = 10;    // 能量上限 ：吃饱时能量

    private float homeWork = 5;       // 作业量
    private float minHomeWork = 2;   // 作业量下限
    private float maxHomeWork = 10;  // 作业量上限

    private float food = 5;          // 食物量
    private float minFood = 2;        // 食物下限
    private float maxFood = 10;      // 食物上限

    protected override void Start()
    {
        base.Start();

        GoalText.instance.SetGoapGoal(this);

        SetGoal(GoapCondition.eat, true);
        SetGoal(GoapCondition.playBall, true);
        SetGoal(GoapCondition.reset, true);
        SetGoal(GoapCondition.doHomeWork, true);
        SetGoal(GoapCondition.cooking, true);
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void SetActions()
    {
        GoapActionEat goapActionEat = new GoapActionEat(this);
        goapActionList.Add(goapActionEat);

        GoapActionPlayBall goapActionPlayBall = new GoapActionPlayBall(this);
        goapActionList.Add(goapActionPlayBall);

        GoapActionReset goapActionReset = new GoapActionReset(this);
        goapActionList.Add(goapActionReset);

        GoapActionHomeWork goapActionHomeWork = new GoapActionHomeWork(this);
        goapActionList.Add(goapActionHomeWork);

        GoapActionCook goapActionCook = new GoapActionCook(this);
        goapActionList.Add(goapActionCook);
    }

    protected override void UpdateStatus()
    {
        worldStats.AddState(GoapCondition.isTired, IsTired());

        worldStats.AddState(GoapCondition.isHungry, IsHungry());

        worldStats.AddState(GoapCondition.hasHomeWork, HasHomeWork());

        worldStats.AddState(GoapCondition.hasFood, HasFood());

        worldStats.AddState(GoapCondition.hasBall, (true));
    }

    public override GoapStatus GetWorldStatus()
    {
        return base.GetWorldStatus();
    }

    #region energy
    // 吃
    public void Eat(float value)
    {
        energy = Mathf.Clamp(energy + value * Time.deltaTime, 0, maxEnergy);
    }

    //是否饿了
    public bool IsHungry()
    {
        return energy <= minEnergy;
    }

    // 是否吃饱
    public bool IsEatFull()
    {
        return energy >= maxEnergy;
    }
    #endregion

    #region vigor
    // 休息
    public void TakeReset(float value)
    {
        vigor = Mathf.Clamp(vigor + value * Time.deltaTime, 0, maxVigor);
    }

    // 是否累了
    public bool IsTired()
    {
        return vigor <= minVigor;
    }

    // 精力充沛
    public bool IsVigorFull()
    {
        return vigor >= maxVigor;
    }
    #endregion

    #region HomeWork 
    public bool HasHomeWork()
    {
        return homeWork >= maxHomeWork;
    }

    public bool homeWorkDone()
    {
        return homeWork <= minHomeWork;
    }

    public void AddHomeWork(float value)
    {
        homeWork = Mathf.Clamp(homeWork + value * Time.deltaTime, 0, maxHomeWork);
    }
    #endregion

    #region food
    public bool HasFood()
    {
        return food >= minFood;
    }

    public bool FoodEnougth()
    {
        return food >= maxFood;
    }

    public void AddFood(float value)
    {
        food = Mathf.Clamp(food + value * Time.deltaTime, 0, maxFood);
    }
    #endregion
}