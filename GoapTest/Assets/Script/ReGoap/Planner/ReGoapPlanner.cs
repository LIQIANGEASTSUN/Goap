using System;
using System.Collections.Generic;
using ReGoap.Core;
using ReGoap.Unity;
using UnityEngine;

namespace ReGoap.Planner
{
    public class ReGoapPlanner
    {
        private readonly NodeManager nodeManager = new NodeManager();

        public ReGoapPlanner() { }

        public Queue<ReGoapNode> Plan(ReGoapAgent agent)
        {
            Queue<ReGoapNode> result = new Queue<ReGoapNode>();

            ReGoapGoal currentGoal = null;

            List<ReGoapGoal> possibleGoals = new List<ReGoapGoal>();
            foreach (var goal in agent.GetGoalsSet())
            {
                possibleGoals.Add(goal);
            }

            possibleGoals.Sort((x, y) => x.GetPriority().CompareTo(y.GetPriority()));

            for (int i = 0; i < possibleGoals.Count; ++i)
            {
                currentGoal = possibleGoals[i];
                ReGoapState goalState = currentGoal.GetGoalState();

                ReGoapNode reGoapNode = new ReGoapNode(agent, goalState, null, null);
                ReGoapNode leaf = nodeManager.Run(reGoapNode);
                if (leaf == null)
                {
                    currentGoal = null;
                    continue;
                }

                result = leaf.CalculatePath();
                if (result.Count == 0)
                {
                    currentGoal = null;
                    continue;
                }
                break;
            }

            return result;
        }
    }
}

/*
 

    public ReGoapGoal Plan(ReGoapAgent agent, Queue<ReGoapActionState> currentPlan = null)
        {
            goapAgent = agent;
            ReGoapGoal currentGoal = null;
            List<ReGoapGoal> possibleGoals = new List<ReGoapGoal>();
            foreach (var goal in goapAgent.GetGoalsSet())
            {
                possibleGoals.Add(goal);
            }

            possibleGoals.Sort((x, y) => x.GetPriority().CompareTo(y.GetPriority()));

            for (int i = 0; i < possibleGoals.Count; ++i)
            {
                currentGoal = possibleGoals[i];
                ReGoapState goalState = currentGoal.GetGoalState();

                goalState = goalState.Clone();

                ReGoapNode reGoapNode = new ReGoapNode(this, goalState, null, null);
                ReGoapNode leaf = nodeManager.Run(reGoapNode);
                if (leaf == null)
                {
                    currentGoal = null;
                    continue;
                }

                Queue<ReGoapActionState> result = leaf.CalculatePath();
                if (currentPlan != null && currentPlan == result)
                {
                    Debug.LogError("Enter");
                    currentGoal = null;
                    break;
                }
                if (result.Count == 0)
                {
                    currentGoal = null;
                    continue;
                }
                currentGoal.SetPlan(result);
                break;
            }

            return currentGoal;
        }

    */
