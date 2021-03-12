using System;
using System.Collections.Generic;
using System.Linq;
using ReGoap.Core;
using ReGoap.Planner;
using UnityEngine;

// generic goal, should inherit this to do your own goal
namespace ReGoap.Unity
{
    public class ReGoapGoal : MonoBehaviour  //, IReGoapGoal<string, object>
    {
        private float Priority = 1;

        protected ReGoapState reGoapState;

        #region UnityFunctions
        protected virtual void Awake()
        {
            reGoapState = ReGoapState.Instantiate();
        }
        #endregion

        #region IReGoapGoal
        public virtual float GetPriority()
        {
            return Priority;
        }

        public virtual ReGoapState GetGoalState()
        {
            return reGoapState;
        }
        #endregion
    }
}