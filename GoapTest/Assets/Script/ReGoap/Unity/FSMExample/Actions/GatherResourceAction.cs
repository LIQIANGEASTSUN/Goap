using System;
using ReGoap.Core;
using ReGoap.Unity.FSMExample.OtherScripts;
using UnityEngine;
using System.Collections.Generic;

namespace ReGoap.Unity.FSMExample.Actions
{
    public class GatherResourceAction : ReGoapAction
    {
        public float TimeToGather = 0.5f;
        public float ResourcePerAction = 1f;
        protected ResourcesBag resourcesBag;
        protected Vector3? resourcePosition;
        protected Resource resource;

        private float gatherCooldown;

        protected override void Awake()
        {
            base.Awake();

            resourcesBag = GetComponent<ResourcesBag>();
        }

        protected virtual string GetNeededResourceFromGoal(ReGoapState goalState)
        {
            foreach (var pair in goalState.GetValues())
            {
                if (pair.Key.StartsWith("hasResource"))
                {
                    string needResource = pair.Key.Substring(11);
                    return needResource;
                }
            }
            return null;
        }

        public override ReGoapState GetPreconditions(ReGoapState goalState)
        {
            var newNeededResourceName = GetNeededResourceFromGoal(goalState);
            preconditions.Clear();
            if (newNeededResourceName != null)
            {
                ReGoapState reGoapState = reGoapAgent.GetWorldState();

                Resource wantedResource = reGoapState.Get("nearest" + newNeededResourceName) as Resource;
                if (wantedResource != null)
                {
                    Vector3 resourcePosition = (Vector3)reGoapState.Get(string.Format("nearest{0}Position", newNeededResourceName));
                    preconditions.Set("isAtPosition", resourcePosition);
                }
            }
            return preconditions;
        }

        public override ReGoapState GetEffects(ReGoapState goalState)
        {
            var newNeededResourceName = GetNeededResourceFromGoal(goalState);
            effects.Clear();
            if (newNeededResourceName != null)
            {
                ReGoapState reGoapState = reGoapAgent.GetWorldState();

                var wantedResource = reGoapState.Get("nearest" + newNeededResourceName) as Resource;
                if (wantedResource != null)
                {
                    effects.Set("hasResource" + newNeededResourceName, true);
                }
            }
            return effects;
        }

        public override IReGoapActionSettings GetSettings( ReGoapState goalState)
        {
            var newNeededResourceName = GetNeededResourceFromGoal(goalState);
            GatherResourceSettings wantedSettings = null;
            if (newNeededResourceName != null)
            {
                ReGoapState reGoapState = reGoapAgent.GetWorldState();

                Resource wantedResource = reGoapState.Get("nearest" + newNeededResourceName) as Resource;
                if (wantedResource != null)
                {
                    wantedSettings = new GatherResourceSettings
                    {
                        ResourcePosition = (Vector3)reGoapState.Get(string.Format("nearest{0}Position", newNeededResourceName)),
                        Resource = wantedResource
                    };
                }
            }
            return wantedSettings;
        }

        public override bool CheckProceduralCondition()
        {
            return base.CheckProceduralCondition() && resourcesBag != null;
        }

        public override void Run(IReGoapActionSettings settings, Action<ReGoapAction> done, Action<ReGoapAction> fail)
        {
            base.Run(settings, done, fail);

            var thisSettings = (GatherResourceSettings)settings;
            resourcePosition = thisSettings.ResourcePosition;
            resource = thisSettings.Resource;

            if (resource == null || resource.GetCapacity() < ResourcePerAction)
            {
                failCallback(this);
            }
            else
            {
                gatherCooldown = Time.time + TimeToGather;
            }
        }

        protected void Update()
        {
            if (resource == null || resource.GetCapacity() < ResourcePerAction)
            {
                failCallback(this);
                return;
            }
            if (Time.time > gatherCooldown)
            {
                gatherCooldown = float.MaxValue;
                resource.RemoveResource(ResourcePerAction);
                resourcesBag.AddResource(resource.GetName(), ResourcePerAction);
                doneCallback(this);
            }
        }
    }

    class GatherResourceSettings : IReGoapActionSettings
    {
        public Vector3? ResourcePosition;
        public Resource Resource;
    }
}