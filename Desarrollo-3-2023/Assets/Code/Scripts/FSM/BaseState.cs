using System;

namespace Patterns.FSM
{
    /// <summary>
    /// Base class for states
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseState<T> : IState
    {
        public event Action onEnter;
        public event Action onExit;

        public string Name { get; set; }
        public T ID { get; private set; }
        public bool Active { get; private set; }

        protected BaseState(T id)
        {
            ID = id;
        }

        protected BaseState(T id, string name) : this(id)
        {
            Name = name;
        }

        public virtual void OnEnter()
        {
            onEnter?.Invoke();
        }

        public virtual void OnUpdate()
        {
        }

        public virtual void OnFixedUpdate()
        {
        }

        public virtual void OnExit()
        {
            onExit?.Invoke();
        }

        /// <summary>
        /// Activates the state by setting the Active flag to true.
        /// </summary>
        public void Enter()
        {
            Active = true;
        }

        /// <summary>
        /// Deactivates the state by setting the Active flag to false.
        /// </summary>
        public void Exit()
        {
            Active = false;
        }
    }
}