using System;

namespace Goap
{
    public class GoapNode : IComparable<GoapNode>
    {
        private GoapNode parentNode;
        private GoapAction goapAction;
        private GoapStatus worldStatus;
        private GoapStatus goapStatus;
        private float cost;

        public GoapNode(GoapNode parent, GoapAction action, GoapStatus status, GoapStatus goapStatus, float cost)
        {
            parentNode = parent;
            goapAction = action;
            worldStatus = status;
            this.goapStatus = goapStatus;
            this.cost = cost;
        }

        public GoapNode ParentNode { get { return parentNode; } }

        public GoapAction GoapAction { get { return goapAction; } }

        public GoapStatus WorldStatus { get { return worldStatus; } }

        public GoapStatus GoapStatus { get { return goapStatus; } }

        public float Cost { get { return cost; } }

        public bool IsFind()
        {
            if (null == goapAction)
            {
                return false;
            }

            GoapStatus preCondition = goapAction.GetPreconditions();
            return preCondition.IsContainIn(WorldStatus);
        }

        public int CompareTo(GoapNode other)
        {
            return this.cost.CompareTo(other.cost);
        }
    }
}