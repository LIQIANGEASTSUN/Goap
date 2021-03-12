using System.Collections.Generic;

namespace Goap
{
    public interface IAction
    {
        void SetActions(GoapAction goapAction);
        List<GoapAction> GetActions();
    }
}