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

        public virtual void OnEnter()
        {

        }

        public virtual void OnUpdate()
        {

        }

        public virtual void OnFixedUpdate()
        {

        }

        public virtual void OnExit()
        {

        }
    }
}

