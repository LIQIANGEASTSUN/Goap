using UnityEngine;
using System;

// 优先级 做饭 > 吃饭 > 休息 > 写作业 > 打篮球
namespace Goap
{
    public abstract class GoapAction
    {
        protected GoapStatus preconditionsStatus; // 前置条件
        protected GoapStatus effectsStatus;       // 执行效果

        protected GoapAgent goapAgent = null;
        protected float cost = 1;

        private GoapActionManager goapActionManager;
        protected StateEnum stateEnum;

        public GoapAction(GoapAgent goapAgent, GoapActionManager goapActionManager)
        {
            InitStatus();
            this.goapAgent = goapAgent;
            this.goapActionManager = goapActionManager;
        }

        public virtual void InitStatus()
        {
            preconditionsStatus = new GoapStatus();
            effectsStatus = new GoapStatus();
        }

        public virtual bool CheckProceduralPrecondition()
        {
            return false;
        }

        private float time = 1;
        public virtual void Run()
        {
            time += Time.deltaTime;
            if (time < 1)
            {
                return;
            }
            time = 0;
            //Debug.LogError("Run : " + this.GetType().ToString());
        }

        public GoapStatus GetPreconditions()
        {
            return preconditionsStatus;
        }

        public GoapStatus GetEffect()
        {
            return effectsStatus;
        }

        public float Cost { get { return cost; } }

        public virtual void Enter()
        {
            //Debug.LogError("Enter : " + this.GetType().ToString());
            StateBase stateBase = goapAgent.StateMachine.ChangeState(stateEnum);
            stateBase.SetAction(this);
        }

        public virtual void Finish()
        {
            //Debug.LogError("Finish : " + this.GetType().ToString());
            goapActionManager.ActionFinishCallBack(this);
        }

        public virtual void Fail()
        {
            //Debug.LogError("Fail : " + this.GetType().ToString());
            goapActionManager.ActionFailCallBack(this);
        }
    }
}