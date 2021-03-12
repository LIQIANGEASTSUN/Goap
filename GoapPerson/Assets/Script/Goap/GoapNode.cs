namespace Goap
{
    public class GoapNode
    {
        private GoapNode parentNode;
        private GoapAction goapAction;
        private GoapStatus currentStatus;
        private float cost;

        public GoapNode(GoapNode parent, GoapAction action, GoapStatus status, float cost)
        {
            parentNode = parent;
            goapAction = action;
            currentStatus = status;
            this.cost = cost;
        }

        public GoapNode ParentNode { get { return parentNode; } }

        public GoapAction GoapAction { get { return goapAction; } }

        public GoapStatus CurrentStatus { get { return currentStatus; } }

        public float Cost { get { return cost; } }
    }
}