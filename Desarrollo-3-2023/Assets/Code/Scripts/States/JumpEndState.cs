using Code.Scripts.States;
using UnityEngine;

namespace Patterns.FSM
{
    /// <summary>
    /// Handler for jump end state
    /// </summary>
    public class JumpEndState<T> : MovementState<T>
    {
        private float gravMultiplier;

        public JumpEndState(T id, string name, float speed, float acceleration, Transform transform, Rigidbody2D rb, float gravMultiplier) : base(id, name, speed, acceleration, transform, rb)
        {
            this.gravMultiplier = gravMultiplier;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            rb.gravityScale *= gravMultiplier;
        }

        public override void OnExit()
        {
            base.OnExit();
            rb.gravityScale /= gravMultiplier;
        }

        public override void OnUpdate()
        {
            if (IsGrounded())
                Exit();
        }
    }
}
