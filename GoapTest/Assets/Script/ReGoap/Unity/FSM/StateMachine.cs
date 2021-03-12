using ReGoap.Unity.FSMExample.FSM;
using System;
using System.Collections.Generic;
using UnityEngine;

// simple FSM, feel free to use this or your own or unity animator's behaviour or anything you like with ReGoap
namespace ReGoap.Unity.FSM
{
    public class StateMachine : MonoBehaviour
    {
        private Dictionary<Type, SmState> statesDic;
        private List<SmTransition> genericTransitions;

        public SmState CurrentState
        {
            get
            {
                return currentState;
            }
        }

        private SmState currentState;

        private MonoBehaviour initialState;

        void OnDisable()
        {
            if (CurrentState != null)
                CurrentState.Exit();
        }

        void Awake()
        {
            enabled = true;
            statesDic = new Dictionary<Type, SmState>();
            genericTransitions = new List<SmTransition>();
        }

        void Start()
        {
            initialState = GetComponent<SmsIdle>();
            foreach (var state in GetComponents<SmState>())
            {
                AddState(state);
                var monoB = (MonoBehaviour) state;
                monoB.enabled = false;
            }
            Switch(initialState.GetType());
        }

        public void AddState(SmState state)
        {
            state.Init(this);
            statesDic[state.GetType()] = state;
        }

        void FixedUpdate()
        {
            Check();
        }

        void Check()
        {
            for (var index = genericTransitions.Count - 1; index >= 0; index--)
            {
                var trans = genericTransitions[index];
                var result = trans.TransitionCheck(CurrentState);
                if (result != null)
                {
                    Switch(result);
                    return;
                }
            }
            if (CurrentState == null) return;
            for (var index = CurrentState.Transitions.Count - 1; index >= 0; index--)
            {
                var trans = CurrentState.Transitions[index];
                var result = trans.TransitionCheck(CurrentState);
                if (result != null)
                {
                    Switch(result);
                    return;
                }
            }
        }

        public void Switch(Type T)
        {
            if (CurrentState != null)
            {
                if ((CurrentState.GetType() == T)) return;
                ((MonoBehaviour) CurrentState).enabled = false;
                CurrentState.Exit();
            }

            currentState = statesDic[T];
            ((MonoBehaviour) CurrentState).enabled = true;
            CurrentState.Enter();
        }
    }
}