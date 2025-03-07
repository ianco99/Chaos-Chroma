using Code.SOs.States;

namespace Patterns.FSM
{
    /// <summary>
    /// Handler for cooldown state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CooldownState<T> : TimerTransitionState<T>
    {
        public float Timer => currentTimer;

        public CooldownState(T id, T nextStateID, TimerSettings settings) : base(id, nextStateID, settings)
        {
        }
    }
}
