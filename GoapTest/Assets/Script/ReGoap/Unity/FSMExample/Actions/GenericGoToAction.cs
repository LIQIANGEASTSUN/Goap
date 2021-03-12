using System;
using ReGoap.Core;
using ReGoap.Unity.FSMExample.FSM;
using UnityEngine;

namespace ReGoap.Unity.FSMExample.Actions
{ // you could also create a generic ExternalGoToAction : GenericGoToAction
//  which let you add effects / preconditions from some source (Unity, external file, etc.)
//  and then add multiple ExternalGoToAction to your agent's gameobject's behaviours
// you can use this without any helper class by having the actions that need to move to a position
//  or transform to have a precondition isAtPosition
    [RequireComponent(typeof(SmsGoTo))]
    public class GenericGoToAction : ReGoapAction
    {
        // sometimes a Transform is better (moving target), sometimes you do not have one (last target position)
        //  but if you're using multi-thread approach you can't use a transform or any unity's API
        protected SmsGoTo smsGoto;

        protected override void Awake()
        {
            base.Awake();

            smsGoto = GetComponent<SmsGoTo>();
        }

        public override void Run(IReGoapActionSettings settings, Action<ReGoapAction> done, Action<ReGoapAction> fail)
        {
            base.Run( settings, done, fail);

            GenericGoToSettings localSettings = (GenericGoToSettings) settings;

            if (localSettings.ObjectivePosition.HasValue)
                smsGoto.GoTo(localSettings.ObjectivePosition, OnDoneMovement, OnFailureMovement);
            else
                failCallback(this);
        }

        public override ReGoapState GetEffects(ReGoapState goalState)
        {
            var goalWantedPosition = GetWantedPositionFromState(goalState);
            if (goalWantedPosition.HasValue)
            {
                effects.Set("isAtPosition", goalWantedPosition);
            }
 
            return base.GetEffects(goalState);
        }

        private Vector3? GetWantedPositionFromState(ReGoapState state)
        {
            Vector3? result = null;
            if (state != null)
            {
                result = state.Get("isAtPosition") as Vector3?;
            }
            return result;
        }

        public override IReGoapActionSettings GetSettings( ReGoapState goalState)
        {
            settings = new GenericGoToSettings
            {
                ObjectivePosition = GetWantedPositionFromState(goalState)
            };
            return base.GetSettings( goalState);
        }

        // if you want to calculate costs use a non-dynamic/generic goto action
        public override float GetCost()
        {
            return base.GetCost() + Cost;
        }

        protected virtual void OnFailureMovement()
        {
            if (failCallback != null)
            {
                failCallback(this);
            }
        }

        protected virtual void OnDoneMovement()
        {
            if (doneCallback != null)
            {
                doneCallback(this);
            }
        }
    }

    public struct GenericGoToSettings : IReGoapActionSettings
    {
        public Vector3? ObjectivePosition;
    }
}