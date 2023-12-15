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

            List<GoapNode> goapNodeList = new List<GoapNode>();
            GoapNode goapNode = new GoapNode(null, null, worldStatus.Clone(), goalStatus.Clone(), 0);
            //GetAction(goapNode, goapNodeList, validActionList, goalStatus);

            Heap<GoapNode> heap = new Heap<GoapNode>();
            heap.SetHeapType(false);
            heap.Insert(goapNode);
            while (heap.Count() > 0)
            {
                GoapNode node = heap.DelRoot();

            }

            Queue<GoapAction> goapActionQueue = new Queue<GoapAction>();
            if (goapNodeList.Count <= 0)
            {
                return goapActionQueue;
            }

            GoapNode heighestNode = GetHighestNode(goapNodeList);
            goapActionQueue = GetActionQueue(heighestNode);
            return goapActionQueue;
        }

        private void CheckAction(GoapNode node, List<GoapAction> goapActionList)
        {
            for (int i = 0; i < goapActionList.Count; i++)
            {
                GoapAction goapAction = goapActionList[i];
                if (!goapAction.CheckProceduralPrecondition())
                {
                    continue;
                }
            }
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

                GoapNode goapNode = new GoapNode(parentNode, goapAction, newStatus, goapStatus, parentNode.Cost + goapAction.Cost);

                if (goapStatus.IsAnyContainIn(newStatus))
                {
                    goapNodeList.Add(goapNode);
                    continue;
                }

                List<GoapAction> actionList = Remove(goapActionList, i);
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