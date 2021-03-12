using System.Collections.Generic;
using UnityEngine;

namespace Goap
{
    public abstract class GoapGoal : MonoBehaviour
    {
        protected GoapStatus worldStats;  // 外部环境状态
        protected GoapStatus goalStatus;  // 要完成的目标

        protected List<GoapAction> goapActionList = new List<GoapAction>();

        // Use this for initialization
        protected virtual void Start()
        {
            worldStats = new GoapStatus();
            goalStatus = new GoapStatus();

            SetActions();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            UpdateStatus();
        }

        protected abstract void UpdateStatus();

        protected abstract void SetActions();

        public List<GoapAction> GetActions()
        {
            return goapActionList;
        }

        public virtual GoapStatus GetWorldStatus()
        {
            return worldStats;
        }

        protected void SetGoal(GoapCondition condition, object value)
        {
            goalStatus.AddState(condition, value);
        }

        public GoapStatus GetGoalStatus()
        {
            return goalStatus;
        }
    }
}