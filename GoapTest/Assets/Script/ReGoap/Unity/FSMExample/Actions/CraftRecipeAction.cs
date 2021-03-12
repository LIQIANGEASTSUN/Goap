using System;
using ReGoap.Core;
using ReGoap.Unity.FSMExample.OtherScripts;
using UnityEngine;

namespace ReGoap.Unity.FSMExample.Actions
{
    [RequireComponent(typeof(ResourcesBag))]
    public class CraftRecipeAction : ReGoapAction
    {
        public ScriptableObject RawRecipe;
        private IRecipe recipe;
        private ResourcesBag resourcesBag;

        protected override void Awake()
        {
            base.Awake();
            recipe = RawRecipe as IRecipe;
            if (recipe == null)
                throw new UnityException("[CraftRecipeAction] The rawRecipe ScriptableObject must implement IRecipe.");
            resourcesBag = GetComponent<ResourcesBag>();

            // could implement a more flexible system that handles dynamic resources's count
            foreach (var pair in recipe.GetNeededResources())
            {
                preconditions.Set("hasResource" + pair.Key, true);   // 添加前置条件
            }

            effects.Set("hasResource" + recipe.GetCraftedResource(), true); // 添加执行结果
        }

        public override ReGoapState GetPreconditions(ReGoapState goalState)
        {
            ReGoapState reGoapState = reGoapAgent.GetWorldState();

            preconditions.Set("isAtPosition", reGoapState.Get("nearestWorkstationPosition") as Vector3?);
            return preconditions;
        }

        public override void Run(IReGoapActionSettings settings, Action<ReGoapAction> done, Action<ReGoapAction> fail)
        {
            base.Run(settings, done, fail);

            ReGoapState reGoapState = reGoapAgent.GetWorldState();

            var workstation = reGoapState.Get("nearestWorkstation") as Workstation;
            if (workstation != null && workstation.CraftResource(resourcesBag, recipe))
            {
                done(this);
            }
            else
            {
                fail(this);
            }
        }
    }
}
