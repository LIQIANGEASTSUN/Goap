using System.Collections.Generic;
using UnityEngine;

namespace ReGoap.Unity.FSMExample.OtherScripts
{
    public class MultipleResourcesManager : MonoBehaviour
    {
        public static MultipleResourcesManager Instance;

        public Dictionary<string, ResourceManager> ResourcesManagerDic;

        void Awake()
        {
            Instance = this;
            var childResources = GetComponentsInChildren<Resource>();
            ResourcesManagerDic = new Dictionary<string, ResourceManager>(childResources.Length);
            foreach (var resource in childResources)
            {
                if (!ResourcesManagerDic.ContainsKey(resource.GetName()))
                {
                    ResourceManager manager = gameObject.AddComponent<ResourceManager>();
                    manager.ResourceName = resource.GetName();
                    ResourcesManagerDic[resource.GetName()] = manager;
                }
                ResourcesManagerDic[resource.GetName()].AddResource(resource);
            }
        }
    }
}
