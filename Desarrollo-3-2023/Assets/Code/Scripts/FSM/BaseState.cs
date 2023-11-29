using System;

namespace Patterns.FSM
{
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

        public void Enter()
        {
            Active = true;
        }
        
        public void Exit()
        {
            Active = false;
        }
    }
}

