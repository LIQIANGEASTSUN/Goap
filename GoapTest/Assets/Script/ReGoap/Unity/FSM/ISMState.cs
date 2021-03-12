using System;
using System.Collections.Generic;

namespace ReGoap.Unity.FSM
{
// you can inherit your FSM's transition from this, but feel free to implement your own (note: must implement ISmTransition and IComparable<ISmTransition>)
    public class SmTransition : IComparable<SmTransition>
    {
        private readonly int priority;
        private readonly Func<SmState, Type> checkFunc;

        public SmTransition(int priority, Func<SmState, Type> checkFunc)
        {
            this.priority = priority;
            this.checkFunc = checkFunc;
        }

        public Type TransitionCheck(SmState state)
        {
            return checkFunc(state);
        }

        public int GetPriority()
        {
            return priority;
        }

        public int CompareTo(SmTransition other)
        {
            return -GetPriority().CompareTo(other.GetPriority());
        }
    }
}