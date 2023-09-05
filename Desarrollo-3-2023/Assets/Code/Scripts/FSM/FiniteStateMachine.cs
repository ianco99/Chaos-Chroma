using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patterns.FSM
{
    public class FiniteStateMachine<T>
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
            if (!initialized)
                initialized = true;
        }

        public void Update()
        {
            if(initialized)
            {
                currentState.OnUpdate();
            }
            else
            {
                throw new System.Exception("FSM not initialized");
            }
        }

        public void FixedUpdate()
        {
            if (initialized)
            {
                currentState.OnFixedUpdate();
            }
            else
            {
                throw new System.Exception("FSM not initialized");
            }
        }

        /// <summary>
        /// Adds newState to fsm's dictionary
        /// </summary>
        /// <param name="newState"></param>
        public void AddState(BaseState<T> newState)
        {
            states.Add(newState.ID, newState);
        }

        /// <summary>
        /// Gets state from fsm's dictionary
        /// </summary>
        /// <param name="stateID"></param>
        /// <returns></returns>
        public BaseState<T> GetState(T stateID)
        {
            if (states.ContainsKey(stateID))
                return states[stateID];
            return null;
        }

        /// <summary>
        /// Gets current state
        /// </summary>
        /// <returns></returns>
        public BaseState<T> GetCurrentState()
        {
            if (currentState != null)
                return currentState;

            return null;
        }

        /// <summary>
        /// Sets current state, running OnExit() methods from previous and OnEnter() from new one
        /// </summary>
        /// <param name="state"></param>
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

