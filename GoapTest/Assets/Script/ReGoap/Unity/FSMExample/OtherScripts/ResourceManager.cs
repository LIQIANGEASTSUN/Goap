using System.Collections.Generic;
using UnityEngine;

namespace ReGoap.Unity.FSMExample.OtherScripts
{ // one resourcemanager per type
    public class ResourceManager : MonoBehaviour
    {
        private List<Resource> resources;
        public string ResourceName;

        #region UnityFunctions
        protected virtual void Awake()
        {
            resources = new List<Resource>();
        }
        #endregion

        #region IResourceManager
        public virtual string GetResourceName()
        {
            return ResourceName;
        }

        public virtual int GetResourcesCount()
        {
            return resources.Count;
        }

        public virtual List<Resource> GetResources()
        {
            return resources;
        }

        public void AddResource(Resource resource)
        {
            resources.Add(resource);
        }

        #endregion
    }
}