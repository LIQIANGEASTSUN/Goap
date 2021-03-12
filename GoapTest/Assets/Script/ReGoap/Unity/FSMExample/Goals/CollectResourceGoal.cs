using UnityEngine;

namespace ReGoap.Unity.FSMExample.Goals
{
    public class CollectResourceGoal : ReGoapGoal
    {
        public string ResourceName;

        protected override void Awake()
        {
            base.Awake();
            reGoapState.Set("collectedResource" + ResourceName, true);
        }
    }
}