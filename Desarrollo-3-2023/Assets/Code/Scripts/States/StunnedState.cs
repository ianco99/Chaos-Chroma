using Code.Scripts.SOs.States;

namespace Patterns.FSM
{
    public class StunnedState<T> : TimerTransitionState<T>
    {
        public StunnedState(T id, T nextStateID, StunnedSettings settings) : base(id, nextStateID, settings.timerSettings)
        {
        }
    }
}
