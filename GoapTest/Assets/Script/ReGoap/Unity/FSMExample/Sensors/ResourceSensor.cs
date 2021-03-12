using System.Collections.Generic;
using ReGoap.Unity.FSMExample.OtherScripts;
using UnityEngine;

// the agent in this example is a villager which knows the location of trees, so seeTree is always true if there is an available  tree
namespace ReGoap.Unity.FSMExample.Sensors
{
    public class ResourceSensor : ReGoapSensor
    {
        protected Dictionary<Resource, Vector3> resourcesPosition;

        protected virtual void Awake()
        {
            resourcesPosition = new Dictionary<Resource, Vector3>();
        }

        protected virtual void UpdateResources(ResourceManager manager)
        {
            resourcesPosition.Clear();
            List<Resource> resourcesList = manager.GetResources();
            for (int index = 0; index < resourcesList.Count; index++)
            {
                var resource = resourcesList[index];
                if (resource.GetCapacity() > 0)
                    resourcesPosition[resource] = resource.GetTransform().position;
            }
        }
    }
}
