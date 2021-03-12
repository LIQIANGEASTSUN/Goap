using System;
using ReGoap.Core;
using ReGoap.Unity.FSMExample.OtherScripts;
using UnityEngine;

namespace ReGoap.Unity.FSMExample.Actions
{
    [RequireComponent(typeof(ResourcesBag))]
    public class AddResourceToBankAction : ReGoapAction
    {
        private ResourcesBag resourcesBag;

        protected override void Awake()
        {
            base.Awake();
            resourcesBag = GetComponent<ResourcesBag>();
        }

        public override IReGoapActionSettings GetSettings(ReGoapState goalState)
        {
            settings = null;
            foreach (var pair in goalState.GetValues())
            {
                if (pair.Key.StartsWith("collectedResource"))
                {
                    var resourceName = pair.Key.Substring(17);
                    settings = new AddResourceToBankSettings
                    {
                        ResourceName = resourceName
                    };
                    break;
                }
            }
            return settings;
        }

        public override ReGoapState GetEffects(ReGoapState goalState)
        {
            effects.Clear();

            foreach (var pair in goalState.GetValues())
            {
                if (pair.Key.StartsWith("collectedResource"))
                {
                    var resourceName = pair.Key.Substring(17);
                    effects.Set("collectedResource" + resourceName, true);
                    break;
                }
            }

            return effects;
        }

        public override ReGoapState GetPreconditions(ReGoapState goalState)
        {
            ReGoapState reGoapState = reGoapAgent.GetWorldState();
            Vector3? bankPosition = reGoapState.Get("nearestBankPosition") as Vector3?;

            preconditions.Clear();
            preconditions.Set("isAtPosition", bankPosition);

            foreach (var pair in goalState.GetValues())
            {
                if (pair.Key.StartsWith("collectedResource"))
                {
                    var resourceName = pair.Key.Substring(17);
                    preconditions.Set("hasResource" + resourceName, true);
                    break;
                }
            }

            return preconditions;
        }


        public override void Run(IReGoapActionSettings settings, Action<ReGoapAction> done, Action<ReGoapAction> fail)
        {
            base.Run(settings, done, fail);
            this.settings = (AddResourceToBankSettings) settings;

            ReGoapState reGoapState = reGoapAgent.GetWorldState();

            Bank bank = reGoapState.Get("nearestBank") as Bank;
            if (bank != null && bank.AddResource(resourcesBag, ((AddResourceToBankSettings) settings).ResourceName))
            {
                done(this);
            }
            else
            {
                fail(this);
            }
        }
    }

    public class AddResourceToBankSettings : IReGoapActionSettings
    {
        public string ResourceName;
    }
}