namespace Patterns.FSM
{
    public abstract class BaseState<T> : IState
    {
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

        public void Enter()
        {
            Active = true;
        }
        
        protected void Exit()
        {
            Active = false;
        }
    }
}

