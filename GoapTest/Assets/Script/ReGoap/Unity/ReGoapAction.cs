using System;
using System.Collections.Generic;
using ReGoap.Core;
using UnityEngine;

namespace ReGoap.Unity
{
    public class ReGoapAction : MonoBehaviour//, IReGoapAction<string, object>
    {
        protected ReGoapState preconditions;
        protected ReGoapState effects;
        public float Cost = 1;

        protected Action<ReGoapAction> doneCallback;
        protected Action<ReGoapAction> failCallback;

        protected ReGoapAgent reGoapAgent;

        protected IReGoapActionSettings settings = null;

        #region UnityFunctions
        protected virtual void Awake()
        {
            enabled = false;

            effects = ReGoapState.Instantiate();
            preconditions = ReGoapState.Instantiate();
        }
        #endregion

        public virtual bool IsActive()
        {
            return enabled;
        }

        public void SetAgent(ReGoapAgent goapAgent)
        {
            reGoapAgent = goapAgent;
        }

        public virtual bool IsInterruptable()
        {
            return true;
        }

        public virtual IReGoapActionSettings GetSettings( ReGoapState goalState)
        {
            return settings;
        }

        public virtual ReGoapState GetPreconditions(ReGoapState goalState)
        {
            return preconditions;
        }

        public virtual ReGoapState GetEffects(ReGoapState goalState)
        {
            return effects;
        }

        public virtual float GetCost()
        {
            return Cost;
        }

        public virtual bool CheckProceduralCondition()
        {
            return true;
        }

        public virtual void Run(IReGoapActionSettings settings, Action<ReGoapAction> done, Action<ReGoapAction> fail)
        {
            enabled = true;
            doneCallback = done;
            failCallback = fail;
            this.settings = settings;
        }

        public virtual void Exit()
        {
            if (gameObject != null)
                enabled = false;
        }
    }
}
