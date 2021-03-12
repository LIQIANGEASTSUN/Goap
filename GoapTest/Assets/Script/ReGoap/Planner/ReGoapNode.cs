using System.Collections.Generic;
using ReGoap.Core;
using ReGoap.Unity;

namespace ReGoap.Planner
{
    public class ReGoapNode
    {
        private ReGoapAgent reGoapAgent;
        private ReGoapNode parentNode;
        public ReGoapAction action;
        public IReGoapActionSettings actionSettings;

        private ReGoapState agentReGoapState;
        private float g;
        private float h;
        private float cost;

        private readonly List<ReGoapNode> expandList = new List<ReGoapNode>();

        public ReGoapNode(ReGoapAgent agent, ReGoapState newGoalState, ReGoapNode parent, ReGoapAction action)
        {
            Init(agent, newGoalState, parent, action);
        }

        private void Init(ReGoapAgent agent, ReGoapState newGoalState, ReGoapNode parent, ReGoapAction action)
        {
            expandList.Clear();

            ReGoapState goal = null;

            this.reGoapAgent = agent;
            this.parentNode = parent;
            this.action = action;
            if (action != null)
                actionSettings = action.GetSettings( newGoalState);

            if (parentNode != null)
            {
                agentReGoapState = parentNode.GetState().Clone();
                g = parentNode.GetPathCost();
            }
            else
            {
                ReGoapState reGoapState = agent.GetWorldState();
                agentReGoapState = reGoapState.Clone();
            }

            if (action != null)
            {
                // create a new instance of the goal based on the paren't goal
                goal = ReGoapState.Instantiate(newGoalState);

                var preconditions = action.GetPreconditions(goal);
                var effects = action.GetEffects(goal);
                // adding the action's effects to the current node's state
                agentReGoapState.AddFromState(effects);
                // addding the action's cost to the node's total cost
                g += action.GetCost();
                // add all preconditions of the current action to the goal
                goal.AddFromState(preconditions);
                // removes from goal all the conditions that are now fullfiled in the node's state
                goal.ReplaceWithMissingDifference(agentReGoapState);
            }
            else
            {
                goal = newGoalState.MissingDifference(agentReGoapState);
            }
            h = goal.Count;
            cost = g + h ;

            //Expand(goal);

            expandList.Clear();

            List<ReGoapAction> actionsList = reGoapAgent.GetActionsSet();
            for (var index = actionsList.Count - 1; index >= 0; index--)
            {
                ReGoapAction possibleAction = actionsList[index];

                if (!possibleAction.CheckProceduralCondition())  // 执行条件不满足排除掉
                {
                    continue;
                }

                ReGoapState precond = possibleAction.GetPreconditions(goal);
                ReGoapState effects = possibleAction.GetEffects(goal);

                if (!ReGoapState.HasAny(effects, goal)) // any effect is the current goal
                {
                    continue;
                }

                if (!ReGoapState.HasAnyConflict(precond, goal))
                {
                    ReGoapNode reGoapNode = new ReGoapNode(reGoapAgent, goal, this, possibleAction);
                    expandList.Add(reGoapNode);
                }
            }
        }

        public float GetPathCost()
        {
            return g;
        }

        public ReGoapState GetState()
        {
            return agentReGoapState;
        }

        public List<ReGoapNode> GetExpandList()
        {
            return expandList;
        }

        public Queue<ReGoapNode> CalculatePath()
        {
            Queue<ReGoapNode> result = new Queue<ReGoapNode>();
            ReGoapNode currentNode = this;
            while (currentNode.GetParent() != null)
            {
                result.Enqueue(currentNode);
                currentNode = currentNode.GetParent();
            }
            return result;
        }

        public ReGoapNode GetParent() { return parentNode; }

        public float GetCost() { return cost; }

        public bool IsGoal() {  return h <= 0;  }
    }
}

/*
using System.Collections.Generic;
using ReGoap.Core;
using ReGoap.Unity;

namespace ReGoap.Planner
{
    public class ReGoapNode
    {
        private ReGoapAgent reGoapAgent;
        private ReGoapNode parentNode;
        private ReGoapAction action;
        private IReGoapActionSettings actionSettings;

        private ReGoapState agentReGoapState;
        private float g;
        private float h;
        private float cost;

        private readonly List<ReGoapNode> expandList = new List<ReGoapNode>();

        public ReGoapNode(ReGoapAgent agent, ReGoapState newGoalState, ReGoapNode parent, ReGoapAction action)
        {
            Init(agent, newGoalState, parent, action);
        }

        private void Init(ReGoapAgent agent, ReGoapState newGoalState, ReGoapNode parent, ReGoapAction action)
        {
            expandList.Clear();

            ReGoapState goal = null;

            this.reGoapAgent = agent;
            this.parentNode = parent;
            this.action = action;
            if (action != null)
                actionSettings = action.GetSettings( newGoalState);

            if (parentNode != null)
            {
                agentReGoapState = parentNode.GetState().Clone();
                g = parentNode.GetPathCost();
            }
            else
            {
                ReGoapState reGoapState = agent.GetWorldState();
                agentReGoapState = reGoapState.Clone();
            }

            if (action != null)
            {
                // create a new instance of the goal based on the paren't goal
                goal = ReGoapState.Instantiate(newGoalState);

                var preconditions = action.GetPreconditions(goal);
                var effects = action.GetEffects(goal);
                // adding the action's effects to the current node's state
                agentReGoapState.AddFromState(effects);
                // addding the action's cost to the node's total cost
                g += action.GetCost();
                // add all preconditions of the current action to the goal
                goal.AddFromState(preconditions);
                // removes from goal all the conditions that are now fullfiled in the node's state
                goal.ReplaceWithMissingDifference(agentReGoapState);
            }
            else
            {
                goal = newGoalState.MissingDifference(agentReGoapState);
            }
            h = goal.Count;
            cost = g + h ;

            //Expand(goal);

            expandList.Clear();

            List<ReGoapAction> actionsList = reGoapAgent.GetActionsSet();
            for (var index = actionsList.Count - 1; index >= 0; index--)
            {
                ReGoapAction possibleAction = actionsList[index];

                if (!possibleAction.CheckProceduralCondition())  // 执行条件不满足排除掉
                {
                    continue;
                }

                ReGoapState precond = possibleAction.GetPreconditions(goal);
                ReGoapState effects = possibleAction.GetEffects(goal);

                if (!ReGoapState.HasAny(effects, goal)) // any effect is the current goal
                {
                    continue;
                }

                if (!ReGoapState.HasAnyConflict(precond, goal))
                {
                    ReGoapNode reGoapNode = new ReGoapNode(reGoapAgent, goal, this, possibleAction);
                    expandList.Add(reGoapNode);
                }
            }
        }

        public float GetPathCost()
        {
            return g;
        }

        public ReGoapState GetState()
        {
            return agentReGoapState;
        }

        public List<ReGoapNode> GetExpandList()
        {
            return expandList;
        }

        private List<ReGoapNode> Expand(ReGoapState goal)
        {
            expandList.Clear();

            List<ReGoapAction> actionsList = reGoapAgent.GetActionsSet();
            for (var index = actionsList.Count - 1; index >= 0; index--)
            {
                ReGoapAction possibleAction = actionsList[index];

                if (!possibleAction.CheckProceduralCondition())  // 执行条件不满足排除掉
                {
                    continue;
                }

                ReGoapState precond = possibleAction.GetPreconditions(goal);
                ReGoapState effects = possibleAction.GetEffects(goal);

                if (!ReGoapState.HasAny(effects, goal)) // any effect is the current goal
                {
                    continue;
                }

                if (!ReGoapState.HasAnyConflict(precond, goal))
                {
                    ReGoapNode reGoapNode = new ReGoapNode(reGoapAgent, goal, this, possibleAction);
                    expandList.Add(reGoapNode);
                }
            }
            return expandList;
        }

        public Queue<ReGoapActionState> CalculatePath()
        {
            Queue<ReGoapActionState> result = new Queue<ReGoapActionState>();
            ReGoapNode currentNode = this;
            while (currentNode.GetParent() != null)
            {
                result.Enqueue(new ReGoapActionState(currentNode.action, currentNode.actionSettings));
                currentNode = currentNode.GetParent();
            }
            return result;
        }

        public ReGoapNode GetParent() { return parentNode; }

        public float GetCost() { return cost; }

        public bool IsGoal() {  return h <= 0;  }
    }
}

    */
