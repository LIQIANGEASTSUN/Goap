using UnityEngine;
using System.Collections.Generic;
using Goap;

public class StateBase  {

    protected StateEnum stateEnum = StateEnum.None;
    protected GoapAgent goapAgent;
    protected GoapAction goapAction;

    protected static Dictionary<StateEnum, string> stateAnimationDic = new Dictionary<StateEnum, string>();

    static StateBase()
    {
        stateAnimationDic[StateEnum.Idle] = "Idle";
        stateAnimationDic[StateEnum.Run] = "Run";
        stateAnimationDic[StateEnum.Attack] = "Attack";
    }

    public StateBase(StateEnum stateEnum, GoapAgent goapAgent)
    {
        this.stateEnum = stateEnum;
        this.goapAgent = goapAgent;
    }

    public void SetAction(GoapAction goapAction)
    {
        this.goapAction = goapAction;
    }

    /// <summary>
    /// 进入状态
    /// </summary>
    public virtual void OnEnter()
    {
        //Debug.LogError("OnEnter :" + stateEnum + "   " + goapAgent.GetInstanceID());
        PlayAnimation();
    }

    /// <summary>
    /// 执行状态
    /// </summary>
    public virtual void OnExecute()
    {
     
    }

    /// <summary>
    /// 退出状态
    /// </summary>
    public virtual void OnExit()
    {
        //Debug.LogError("OnExit :" + stateEnum);
        goapAction = null;
    }

    protected virtual string GetAnimationName()
    {
        return stateAnimationDic[stateEnum];
    }

    private void PlayAnimation()
    {
        string animName = GetAnimationName();

        //Debug.LogError("PlayAnimation : " + animName + "     " + goapAgent.name);
        goapAgent.AnimationManager.CrossFade(animName, 0, false, true);
    }
}