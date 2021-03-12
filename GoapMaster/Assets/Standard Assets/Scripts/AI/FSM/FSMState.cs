using UnityEngine;
using System.Collections;

public delegate void FSMState(FSM fsm, GameObject gameObject);

public abstract class FSMStateBase
{
    protected FSMStateEnum fsmStateEnum = FSMStateEnum.None;
    protected GoapAgent goapAgent = null;
    public FSMStateBase(FSMStateEnum fsmStateEnum, GoapAgent goapAgent)
    {
        this.fsmStateEnum = fsmStateEnum;
        this.goapAgent = goapAgent;
    }

    public virtual void OnEnter()
    {
        Debug.LogError("OnEenter : " + fsmStateEnum);
    }

    public virtual void OnExit()
    {
        Debug.LogError("OnExit : " + fsmStateEnum);
    }

    public virtual void Update()
    {
        //Debug.LogError("Update : " + fsmStateEnum);
    }
}
