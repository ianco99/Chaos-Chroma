namespace Patterns.FSM
{
    public interface IState
    {
        public void OnEnter();
        public void Update();
        public void OnExit();

    }
}
