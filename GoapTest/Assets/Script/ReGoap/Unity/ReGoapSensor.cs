using ReGoap.Core;
using UnityEngine;

namespace ReGoap.Unity
{
    public class ReGoapSensor : MonoBehaviour
    {
        protected ReGoapAgent reGoapAgent;
        public virtual void Init(ReGoapAgent reGoapAgent)
        {
            this.reGoapAgent = reGoapAgent;
        }

        public virtual void UpdateSensor()
        {

        }
    }
}
