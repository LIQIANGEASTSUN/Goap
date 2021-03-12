using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * Plans what actions can be completed in order to fulfill a goal state.
 */
public class GoapPlanner
{
	/**
	 * Plan what sequence of actions can fulfill the goal.
	 * Returns null if a plan could not be found, or a list of the actions
	 * that must be performed, in order, to fulfill the goal.
	 */
	public Queue<GoapAction> plan(GameObject agent, List<GoapAction> availableActions, Dictionary<string, bool> worldState,  Dictionary<string, bool> goal) 
	{
		// reset the actions so we can start fresh with them
		for (int i = 0; i <availableActions.Count; ++i) {
            GoapAction goapAction = availableActions[i];
            goapAction.doReset ();
		}

		// check what actions can run using their checkProceduralPrecondition
		List<GoapAction> usableActions = new List<GoapAction> ();
        for (int i = 0; i < availableActions.Count; ++i)
        {
            GoapAction goapAction = availableActions[i];
            if (goapAction.checkProceduralPrecondition(agent))
            {
                usableActions.Add(goapAction);
            }
        }
		
		// we now have all actions that can run, stored in usableActions
		// build up the tree and record the leaf nodes that provide a solution to the goal.
		List<Node> leaves = new List<Node>();

		// build graph
		Node start = new Node (null, 0, worldState, null);
		bool success = buildGraph(start, leaves, usableActions, goal);

		if (!success) {
			// oh no, we didn't get a plan
			Debug.Log("NO PLAN");
			return null;
		}

		// get the cheapest leaf
		Node cheapest = null;
        for (int i = 0; i < leaves.Count; ++i)
        {
            Node leaf = leaves[i];
            if (cheapest == null)
            {
                cheapest = leaf;
            }
            else if (cheapest.runningCost > leaf.runningCost)
            {
                cheapest = leaf;
            }
        }

		// get its node and work back through the parents
		List<GoapAction> result = new List<GoapAction> ();
		while (cheapest != null) {
			if (cheapest.action != null) {
				result.Insert(0, cheapest.action); // insert the action in the front
			}
            cheapest = cheapest.parent;
		}
		// we now have this action list in correct order
		Queue<GoapAction> queue = new Queue<GoapAction> ();
		for (int i = 0; i < result.Count; ++i) {
            GoapAction goapAction = result[i];
            queue.Enqueue(goapAction);
		}

		// hooray we have a plan!
		return queue;
	}

	/**
	 * Returns true if at least one solution was found.
	 * The possible paths are stored in the leaves list. Each leaf has a
	 * 'runningCost' value where the lowest cost will be the best action
	 * sequence.
	 */
	private bool buildGraph (Node parentNode, List<Node> leaves, List<GoapAction> usableActions, Dictionary<string, bool> goal)
	{
		bool foundOne = false;

		// go through each action available at this node and see if we can use it here
		for (int i = 0; i < usableActions.Count; ++i) {
            GoapAction action = usableActions[i];
            // if the parent state has the conditions for this action's preconditions, we can use it here
            if (!inState(action.Preconditions, parentNode.state))
            {
                continue;
            }

            // apply the action's effects to the parent state
            Dictionary<string, bool> currentState = populateState(parentNode.state, action.Effects);
            //Debug.Log(GoapAgent.prettyPrint(currentState));
            Node node = new Node(parentNode, (parentNode.runningCost + action.cost), currentState, action);

            // 已完成目标
            if (inState(goal, currentState))
            {
                // we found a solution!
                leaves.Add(node);
                foundOne = true;
                continue;
            }

            // not at a solution yet, so test all the remaining actions and branch out the tree
            List<GoapAction> subset = actionSubset(usableActions, action);
            bool found = buildGraph(node, leaves, subset, goal);
            if (found)
            {
                foundOne = true;
            }
        }

		return foundOne;
	}

    /**
	 * Create a subset of the actions excluding the removeAction one. Creates a new set.
	 */
    private List<GoapAction> actionSubset(List<GoapAction> actions, GoapAction removeAction) {
        List<GoapAction> subset = new List<GoapAction> ();
        for (int i = 0; i <actions.Count; ++i)
        {
            GoapAction goapAction = actions[i];
            if (!goapAction.Equals(removeAction))
            {
                subset.Add(goapAction);
            }
        }
		return subset;
	}

	/**
	 * Check that all items in 'test' are in 'state'. If just one does not match or is not there
	 * then this returns false.
	 */
	private bool inState(Dictionary<string, bool> test, Dictionary<string, bool> state) {
		bool allMatch = true;

        foreach (KeyValuePair<string, bool> kv in test)
        {
            bool match = false;
            if (state.ContainsKey(kv.Key) && state[kv.Key] == kv.Value)
            {
                match = true;
                continue;
            }
            if (!match)
                allMatch = false;
        }

		return allMatch;
	}
	
	/**
	 * Apply the stateChange to the currentState
	 */
	private Dictionary<string, bool> populateState(Dictionary<string, bool> currentState, Dictionary<string, bool> stateChange) {
		Dictionary<string, bool> state = new Dictionary<string, bool>();
		// copy the KVPs over as new objects
		foreach (KeyValuePair<string, bool> kv in currentState) {
			state.Add(kv.Key, kv.Value);
		}

		foreach (KeyValuePair<string, bool> change in stateChange) {
            // if the key exists in the current state, continue
            if (state.ContainsKey(change.Key) && state[change.Key] == change.Value)
            {
                continue;
            }
            
            // update the Value
            state[change.Key] = change.Value;
        }
		return state;
	}

	/**
	 * Used for building up the graph and holding the running costs of actions.
	 */
	private class Node {
		public Node parent;
		public float runningCost;
		public Dictionary<string, bool> state;
		public GoapAction action;

		public Node(Node parent, float runningCost, Dictionary<string, bool> state, GoapAction action) {
			this.parent = parent;
			this.runningCost = runningCost;
			this.state = state;
			this.action = action;
		}
	}

}


