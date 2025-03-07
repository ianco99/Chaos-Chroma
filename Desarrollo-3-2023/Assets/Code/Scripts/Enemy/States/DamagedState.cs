using Code.SOs.States;
using UnityEngine;

namespace Patterns.FSM
{
    /// <summary>
    /// Handler for state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DamagedState<T> : TimerTransitionState<T>
    {
        private DamagedSettings damagedSettings;
        private Rigidbody2D rb;
        private Vector2 pushDirection = Vector2.right;
        
        public DamagedState( T id, string name, T nextStateID, DamagedSettings settings, Rigidbody2D rb) : base(id, name, nextStateID, settings.timerSettings)
        {
            this.rb = rb;
            damagedSettings = settings;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            ResetState();
        }

        /// <summary>
        /// Sets the direction of the push force that is applied to the character
        /// when this state is entered.
        /// </summary>
        /// <param name="newDirection">The new direction to set the push force to.</param>
        public void SetDirection(Vector2 newDirection)
        {
            pushDirection = newDirection;
        }

        /// <summary>
        /// Resets the state by resetting the timer and applying a force
        /// to the character in the direction specified by <see cref="pushDirection"/>.
        /// </summary>
        private void ResetState()
        {
            currentTimer = 0;
            rb.AddForce(pushDirection * damagedSettings.force, ForceMode2D.Impulse);
        }
    }
}