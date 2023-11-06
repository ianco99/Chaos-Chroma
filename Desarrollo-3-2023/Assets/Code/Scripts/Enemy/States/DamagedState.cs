using Code.Scripts.States;
using Code.SOs.Enemy;
using System;
using UnityEngine;

namespace Patterns.FSM
{
    /// <summary>
    /// Handler for state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DamagedState<T> : TimerTransitionState<T>
    {
        private float force;
        private Rigidbody2D rb;
        private Vector2 pushDirection = Vector2.right;
        public DamagedState( T id, string name, T nextStateID, float maxTime, float force, Rigidbody2D rb) : base(id, name, nextStateID, maxTime)
        {
            this.rb = rb;
            this.force = force;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            ResetState();
        }

        public void SetDirection(Vector2 newDirection)
        {
            pushDirection = newDirection;
        }

        public void ResetState()
        {
            currentTimer = 0;
            rb.AddForce(pushDirection * force, ForceMode2D.Impulse);
        }
    }
}