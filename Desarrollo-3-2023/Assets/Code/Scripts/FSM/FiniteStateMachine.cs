using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patterns.FSM
{
    public class FiniteStateMachine<T> : MonoBehaviour
    {
        private Dictionary<T, BaseState<T>> states;
        private BaseState<T> currentState;
        private bool initialized;
        public FiniteStateMachine()
        {
            states = new Dictionary<T, BaseState<T>>();
        }

        public void Init()
        {

        }

        public void AddState(BaseState<T> newState)
        {
            states.Add(newState.ID, newState);
        }

        public BaseState<T> GetState(T stateID)
        {
            if (states.ContainsKey(stateID))
                return states[stateID];
            return null;
        }

        public void SetCurrentState(BaseState<T> state)
        {
            if (currentState == state) return;

            if (currentState != null)
                currentState.OnExit();

            currentState = state;

            if (currentState != null)
                currentState.OnEnter();
        }
    }
}

