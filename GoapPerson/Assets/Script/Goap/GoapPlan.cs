using System.Collections.Generic;
using DataStruct.Heap;

namespace Goap
{
    public class GoapPlan
    {
        public Queue<GoapAction> Plan(List<GoapAction> goapActionList, GoapStatus worldStatus, GoapStatus goalStatus)
        {
            List<GoapAction> validActionList = new List<GoapAction>();
            validActionList.AddRange(goapActionList);
            validActionList.Sort((a, b) =>
            {
                return a.Cost.CompareTo(b.Cost);
            });

            List<GoapNode> goapNodeList = new List<GoapNode>();
            GoapNode goapNode = new GoapNode(null, null, worldStatus.Clone(), goalStatus.Clone(), 0);
            //GetAction(goapNode, goapNodeList, validActionList, goalStatus);

            Heap<GoapNode> heap = new Heap<GoapNode>();
            heap.SetHeapType(false);
            heap.Insert(goapNode);

            GoapNode resultNode = null;
            while (heap.Count() > 0)
            {
                GoapNode node = heap.DelRoot();
                if (node.IsFind())
                {
                    resultNode = node;
                    break;
                }
                CheckAction(heap, node, validActionList);
            }

            Queue<GoapAction> goapActionQueue = new Queue<GoapAction>();
            goapActionQueue = GetActionQueue(resultNode);
            return goapActionQueue;
        }

        private void CheckAction(Heap<GoapNode> heap, GoapNode node, List<GoapAction> goapActionList)
        {
            for (int i = 0; i < goapActionList.Count; i++)
            {
                GoapAction action = goapActionList[i];
                if (!action.CheckProceduralPrecondition())
                {
                    continue;
                }

                GoapStatus worldStatus = node.WorldStatus.Clone();
                GoapStatus goalStatus = node.GoapStatus.Clone();

                GoapStatus effect = action.GetEffect();
                // 要求 Action 的执行效果 action.effect，至少有一个包含在agentGoal
                if (!effect.IsAnyContainIn(goalStatus))
                {
                    continue;
                }

                GoapStatus preCondition = action.GetPreconditions();
                // 要求 action 的先决条件与不能与目标冲突
                if (preCondition.HasAnyConflict(goalStatus))
                {
                    continue;
                }

                worldStatus.AddFromStatus(effect);
                goalStatus.RemoveFromStatus(effect);
                goalStatus.AddFromStatus(preCondition);
                float cost = node.Cost + action.Cost;
                GoapNode newNode = new GoapNode(node, action, worldStatus.Clone(), goalStatus.Clone(), cost);
                heap.Insert(newNode);
            }
        }

        private Queue<GoapAction> GetActionQueue(GoapNode goapNode)
        {
            Queue<GoapAction> goapActionQueue = new Queue<GoapAction>();
            if (null == goapNode)
            {
                return goapActionQueue;
            }

            Stack<GoapAction> goapActionStack = new Stack<GoapAction>();
            while (goapNode != null && goapNode.GoapAction != null)
            {
                goapActionStack.Push(goapNode.GoapAction);
                goapNode = goapNode.ParentNode;
            }

            while (goapActionStack.Count > 0)
            {
                GoapAction goapAction = goapActionStack.Pop();
                goapActionQueue.Enqueue(goapAction);
            }

            return goapActionQueue;
        }
    }
}




/*
using System.Collections.Generic;

namespace Goap
{
    public class GoapPlan
    {
        public Queue<GoapAction> Plan(List<GoapAction> goapActionList, GoapStatus worldStatus, GoapStatus goalStatus)
        {
            List<GoapAction> validActionList = new List<GoapAction>();
            validActionList.AddRange(goapActionList);

            List<GoapNode> goapNodeList = new List<GoapNode>();
            GoapNode goapNode = new GoapNode(null, null, worldStatus, 0);
            GetAction(goapNode, goapNodeList, validActionList, goalStatus);

            Queue<GoapAction> goapActionQueue = new Queue<GoapAction>();
            if (goapNodeList.Count <= 0)
            {
                return goapActionQueue;
            }

            GoapNode heighestNode = GetHighestNode(goapNodeList);
            goapActionQueue = GetActionQueue(heighestNode);
            return goapActionQueue;
        }

        private void GetAction(GoapNode parentNode, List<GoapNode> goapNodeList, List<GoapAction> goapActionList, GoapStatus goapStatus)
        {
            for (int i = 0; i < goapActionList.Count; ++i)
            {
                GoapAction goapAction = goapActionList[i];
                if (!goapAction.CheckProceduralPrecondition())
                {
                    continue;
                }

                GoapStatus preconditions = goapAction.GetPreconditions(); // 前置条件
                GoapStatus effectsStatus = goapAction.GetEffect();        // 执行效果

                if (!preconditions.IsContainIn(parentNode.CurrentStatus))  // 不满足前置条件
                {
                    continue;
                }

                GoapStatus newStatus = parentNode.CurrentStatus.Clone();
                newStatus.AddFromStatus(effectsStatus);

                GoapNode goapNode = new GoapNode(parentNode, goapAction, newStatus, parentNode.Cost + goapAction.Cost);

                if (goapStatus.IsAnyContainIn(newStatus))
                {
                    goapNodeList.Add(goapNode);
                    continue;
                }

                List<GoapAction> actionList = Remove(goapActionList, i);
                GetAction(goapNode, goapNodeList, actionList, goapStatus);
            }
        }

        private List<GoapAction> Remove(List<GoapAction> actionList, int index)
        {
            List<GoapAction> goapActionList = new List<GoapAction>();
            for (int i = 0; i < actionList.Count; ++i)
            {
                if (i != index)
                {
                    goapActionList.Add(actionList[i]);
                }
            }

            return goapActionList;
        }

        private GoapNode GetHighestNode(List<GoapNode> goapNodeList)
        {
            GoapNode priorityNode = null;
            for (int i = 0; i < goapNodeList.Count; ++i)
            {
                if (priorityNode == null)
                {
                    priorityNode = goapNodeList[i];
                }
                else if (priorityNode.Cost > goapNodeList[i].Cost)
                {
                    priorityNode = goapNodeList[i];
                }
            }

            return priorityNode;
        }

        private Queue<GoapAction> GetActionQueue(GoapNode goapNode)
        {
            Stack<GoapAction> goapActionStack = new Stack<GoapAction>();
            while (goapNode != null && goapNode.GoapAction != null)
            {
                goapActionStack.Push(goapNode.GoapAction);
                goapNode = goapNode.ParentNode;
            }

            Queue<GoapAction> goapActionQueue = new Queue<GoapAction>();
            while (goapActionStack.Count > 0)
            {
                GoapAction goapAction = goapActionStack.Pop();
                goapActionQueue.Enqueue(goapAction);
            }

            return goapActionQueue;
        }
    }
}

*/