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
            rb.AddForce(pushDirection * force, ForceMode2D.Force);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

        }

        public void SetDirection(Vector2 newDirection)
        {
            pushDirection = newDirection;
        }

        public void ResetState()
        {
            base.currentTimer = 0;
            rb.AddForce(pushDirection * force, ForceMode2D.Impulse);
        }
    }
}