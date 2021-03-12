using ReGoap.Unity.FSMExample.OtherScripts;
using UnityEngine;

// the agent in this example is a villager which knows the location of trees, so seeTree is always true if there is an available  tree
namespace ReGoap.Unity.FSMExample.Sensors
{
    public class MultipleResourcesSensor : ResourceSensor
    {
        public float MinResourceValue = 1f;

        public override void UpdateSensor()
        {
            var worldState = reGoapAgent.GetWorldState();

            foreach (var pair in MultipleResourcesManager.Instance.ResourcesManagerDic)
            {
                ResourceManager resourceManager = pair.Value;

                worldState.Set("see" + resourceManager.GetResourceName(), resourceManager.GetResourcesCount() >= MinResourceValue);

                UpdateResources(resourceManager);

                Resource nearestResource = Utilities.GetNearest(transform.position, resourcesPosition);
                worldState.Set("nearest" + resourceManager.GetResourceName(), nearestResource);

                Vector3 nearestResourcePosition = (nearestResource != null ? nearestResource.GetTransform().position : Vector3.zero);
                worldState.Set("nearest" + resourceManager.GetResourceName() + "Position", nearestResourcePosition);
            }
        }
    }
}
