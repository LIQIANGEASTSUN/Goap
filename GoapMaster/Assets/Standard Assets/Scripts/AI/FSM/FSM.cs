using System.Collections.Generic;
using UnityEngine;
using System.Collections;

/**
 * Stack-based Finite State Machine.
 * Push and pop states to the FSM.
 * 
 * States should push other states onto the stack 
 * and pop themselves off.
 */
using System;


public class FSM {
    private Dictionary<FSMStateEnum, FSMStateBase> FSMStateDic = new Dictionary<FSMStateEnum, FSMStateBase>();

    private Stack<FSMStateBase> stateBaseStack = new Stack<FSMStateBase>();

    public FSM(GoapAgent goapAgent)
    {
        FSMStateDic[FSMStateEnum.Idle] = new FSMStateIdel(goapAgent);
        FSMStateDic[FSMStateEnum.Move] = new FSMStateMove(goapAgent);
        FSMStateDic[FSMStateEnum.Perform] = new FSMStatePerform(goapAgent);
    }

    public void pushState(FSMStateEnum fsmStateEnum)
    {
        FSMStateBase fsmStateBase = null;
        if (FSMStateDic.TryGetValue(fsmStateEnum, out fsmStateBase))
        {
            stateBaseStack.Push(fsmStateBase);
        }
    }

    public void popState()
    {
        stateBaseStack.Pop();
    }

    public void clearState()
    {
        stateBaseStack.Clear();
    }

    public void Update()
    {
        if (stateBaseStack.Count <= 0)
        {
            return;
        }
        FSMStateBase fsmStateBase = stateBaseStack.Peek();
        if (fsmStateBase != null)
        {
            fsmStateBase.Update();
        }
    }
}

public enum FSMStateEnum
{
    None = 0,
    Idle = 1,
    Move = 2,
    Perform = 3,
}