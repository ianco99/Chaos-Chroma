using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patterns.FSM
{
    public abstract class BaseState<T> : IState
    {
        public string Name { get; set; }
        public T ID { get; private set; }
        public BaseState(T id)
        {
            ID = id;
        }

        public BaseState(T id, string name) : this(id)
        {
            Name = name;
        }

        public void OnEnter()
        {

        }

        public void Update()
        {

        }

        public void FixedUpdate()
        {

        }

        public void OnExit()
        {

        }
    }
}

