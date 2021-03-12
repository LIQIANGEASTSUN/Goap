using System.Collections.Generic;
using Goap;

/// <summary>
/// 有限状态机
/// </summary>
public class StateMachine  {

    private GoapAgent goapAgent;
    private Dictionary<StateEnum, StateBase> StateDic = new Dictionary<StateEnum, StateBase>();
    private StateBase currentState = null;

    public StateMachine(GoapAgent goapAgent)
    {
        this.goapAgent = goapAgent;

        StateDic[StateEnum.Idle] = new StateIdle(goapAgent);
        StateDic[StateEnum.Run] = new StateRun(goapAgent);
        StateDic[StateEnum.Attack] = new StateAttack(goapAgent);
    }

    public void OnFrame()
    {
        if (currentState == null)
        {
            ChangeState(StateEnum.Idle);
            return;
        }

        currentState.OnExecute();
    }

    public StateBase ChangeState(StateEnum stateEnum)
    {
        if (!StateDic.ContainsKey(stateEnum))
        {
            return currentState;
        }

        if (currentState != null)
        {
            currentState.OnExit();
        }

        currentState = StateDic[stateEnum];
        currentState.OnEnter();
        return currentState;
    }
}