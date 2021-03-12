using System.Collections.Generic;
using ReGoap.Core;
using UnityEngine;
using ReGoap.Planner;

namespace ReGoap.Unity
{
    public class ReGoapAgent : MonoBehaviour
    {
        private ReGoapPlanner reGoapPlanner = new ReGoapPlanner();

        protected List<ReGoapGoal> goals;

        protected List<ReGoapAction> actions;

        protected Queue<ReGoapNode> reGoapNodeList = new Queue<ReGoapNode>();
        protected ReGoapNode reGoapNode;
        
        protected bool possibleGoalsDirty;

        public float CalculationDelay = 0.5f;

        protected float lastCalculationTime;

        protected bool startedPlanning;

        protected ReGoapState state;
        private ReGoapSensor[] sensors;

        public float SensorsUpdateDelay = 0.3f;
        private float sensorsUpdateCooldown;

        public bool IsPlanning
        {
            get { return startedPlanning; }
        }

        #region UnityFunctions
        protected virtual void Awake()
        {
            lastCalculationTime = -100;

            goals = new List<ReGoapGoal>(GetComponents<ReGoapGoal>());
            possibleGoalsDirty = true;

            actions = new List<ReGoapAction>(GetComponents<ReGoapAction>());
            for (int i = 0; i < actions.Count; ++i)
            {
                actions[i].SetAgent(this);
            }

            state = ReGoapState.Instantiate();
            sensors = GetComponents<ReGoapSensor>();
            foreach (var sensor in sensors)
            {
                sensor.Init(this);
            }
        }

        protected virtual void Start()
        {
            CalculateNewGoal(true);
        }

        protected virtual void OnDisable()
        {
            if (reGoapNode != null)
            {
                reGoapNode.action.Exit();
                reGoapNode = null;
                reGoapNodeList.Clear();
            }
        }
        #endregion

        protected virtual void Update()
        {
            UpdateSensor();

            possibleGoalsDirty = true;

            if (reGoapNode == null)
            {
                if (!IsPlanning)
                {
                    CalculateNewGoal();
                }
                return;
            }
        }

        private void UpdateSensor()
        {
            if (Time.time > sensorsUpdateCooldown)
            {
                sensorsUpdateCooldown = Time.time + SensorsUpdateDelay;
                foreach (var sensor in sensors)
                {
                    sensor.UpdateSensor();
                }
            }
        }

        protected virtual void UpdatePossibleGoals()
        {
            possibleGoalsDirty = false;
        }

        protected virtual void TryWarnActionFailure(ReGoapAction action)
        {
            if (action.IsInterruptable())
                WarnActionFailure(action);
        }

        protected virtual void CalculateNewGoal(bool forceStart = false)
        {
            if (IsPlanning)
            {
                return;
            }

            if (!forceStart && (Time.time - lastCalculationTime <= CalculationDelay))
            {
                return;
            }
            lastCalculationTime = Time.time;

            UpdatePossibleGoals();

            startedPlanning = true;

            Queue<ReGoapNode> _reGoapNodeList = reGoapPlanner.Plan(this);
            OnDonePlanning(_reGoapNodeList);
        }

        private void OnDonePlanning(Queue<ReGoapNode> _reGoapNodeList)
        {
            startedPlanning = false;

            if (_reGoapNodeList.Count <= 0) { 
                return;
            }

            if (reGoapNode != null)
            {
                reGoapNode.action.Exit();
                reGoapNode = null;
            }

            reGoapNodeList = _reGoapNodeList;

            PushAction();
        }

        public virtual void DoActionEnd(ReGoapAction thisAction)
        {
            if (reGoapNode != null && thisAction != reGoapNode.action)
                return;
            PushAction();
        }

        protected virtual void PushAction()
        {
            if (reGoapNode != null)
            {
                reGoapNode.action.Exit();
                reGoapNode = null;
            }

            if (reGoapNodeList.Count <= 0)
            {
                return;
            }

            if (reGoapNodeList.Count == 0)
            {
                CalculateNewGoal();
            }
            else
            {
                reGoapNode = reGoapNodeList.Dequeue();
                reGoapNode.action.Run( reGoapNode.actionSettings, DoActionEnd, WarnActionFailure);
            }
        }

        public virtual void WarnActionFailure(ReGoapAction thisAction)
        {
            if (reGoapNode != null && thisAction != reGoapNode.action)
            {
                Debug.LogWarning(string.Format("[GoapAgent] Action {0} warned for failure but is not current action.", thisAction));
                return;
            }
            
            CalculateNewGoal(true);
        }

        public virtual List<ReGoapGoal> GetGoalsSet()
        {
            if (possibleGoalsDirty)
                UpdatePossibleGoals();

            return goals;
        }

        public virtual List<ReGoapAction> GetActionsSet()
        {
            return actions;
        }

        public virtual ReGoapState GetWorldState()
        {
            return state;
        }
    }
}