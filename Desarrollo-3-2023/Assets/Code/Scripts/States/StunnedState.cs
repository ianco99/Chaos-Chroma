using Code.Scripts.SOs.States;

namespace Patterns.FSM
{
    /// <summary>
    /// Handler for stunned state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StunnedState<T> : TimerTransitionState<T>
    {
        public StunnedState(T id, T nextStateID, StunnedSettings settings) : base(id, nextStateID, settings.timerSettings)
        {
        }
    }
}
