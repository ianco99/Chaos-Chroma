namespace Patterns.FSM
{
    public interface IState
    {
        public void OnEnter();
        public void OnUpdate();
        public void OnFixedUpdate();
        public void OnExit();

    }
}
