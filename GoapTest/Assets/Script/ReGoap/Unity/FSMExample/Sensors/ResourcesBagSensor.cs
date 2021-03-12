using ReGoap.Unity.FSMExample.OtherScripts;

namespace ReGoap.Unity.FSMExample.Sensors
{
    public class ResourcesBagSensor : ReGoapSensor
    {
        private ResourcesBag resourcesBag;

        void Awake()
        {
            resourcesBag = GetComponent<ResourcesBag>();
        }

        public override void UpdateSensor()
        {
            var state = reGoapAgent.GetWorldState();
            foreach (var pair in resourcesBag.GetResources())
            {
                state.Set("hasResource" + pair.Key, pair.Value > 0);
            }
        }
    }
}
