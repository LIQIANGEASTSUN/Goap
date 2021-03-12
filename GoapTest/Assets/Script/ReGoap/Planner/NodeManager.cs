using System.Collections.Generic;
using ReGoap.Core;
using UnityEngine;

namespace ReGoap.Planner
{
    public class NodeManager
    {
        public ReGoapNode Run(ReGoapNode start)
        {
            Clear();

            Enqueue(start);
            while ((Count > 0))
            {
                ReGoapNode node = Dequeue();
                if (node.IsGoal())
                {
                    return node;
                }

                List<ReGoapNode> NodeList = node.GetExpandList();
                for (int i = 0; i < NodeList.Count; ++i)
                {
                    ReGoapNode child = NodeList[i];
                    if (child.IsGoal())
                    {
                        return child;
                    }

                    Enqueue(child);
                }
            }

            return null;
        }

        #region
        private List<ReGoapNode> regoapNodeList = new List<ReGoapNode>();

        public int Count { get { return regoapNodeList.Count; } }

        public void Clear() { regoapNodeList.Clear(); }

        public void Enqueue(ReGoapNode node)
        {
            regoapNodeList.Add(node);

            regoapNodeList.Sort((a, b) =>
            {
                return (int)(a.GetCost() - b.GetCost());
            });
        }

        public ReGoapNode Dequeue()
        {
            ReGoapNode reGoapNode = regoapNodeList[0];
            regoapNodeList.RemoveAt(0);
            return reGoapNode;
        }

        public void Remove(ReGoapNode node)
        {
            regoapNodeList.Remove(node);
        }
        #endregion
    }
}