using UnityEngine;
using System;

// 优先级 做饭 > 吃饭 > 休息 > 写作业 > 打篮球
namespace Goap
{
    public abstract class GoapAction
    {
        protected GoapStatus preconditionsStatus; // 前置条件
        protected GoapStatus effectsStatus;       // 执行效果

        protected GoapGoal goapGoal;
        protected Transform target;

        protected float cost = 1;

        protected Action<GoapAction> finisCallBack;
        protected Action<GoapAction> failCallBack;

        public GoapAction(GoapGoal goapGoal)
        {
            InitStatus();
            this.goapGoal = goapGoal;
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

        public virtual void Run()
        {

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

        protected virtual bool IsInRange()
        {
            float distance = Vector3.Distance(target.position, goapGoal.transform.position);
            return distance < 1;
        }

        protected void Finish()
        {
            target = null;

            Debug.LogError("Finish : " + this.GetType().ToString() + "    " + Time.realtimeSinceStartup);
            if (finisCallBack != null)
            {
                finisCallBack(this);
            }
        }

        protected void Fail()
        {
            target = null;
            Debug.LogError("Fail : " + this.GetType().ToString() + "    " + Time.realtimeSinceStartup);

            if (failCallBack != null)
            {
                failCallBack(this);
            }
        }

        public void SetCallBack(Action<GoapAction> finssCallBack, Action<GoapAction> failCallBack)
        {
            Debug.LogError("Enter : " + this.GetType().ToString() + "    " + Time.realtimeSinceStartup);
            this.finisCallBack = finssCallBack;
            this.failCallBack = failCallBack;
        }
    }
}
