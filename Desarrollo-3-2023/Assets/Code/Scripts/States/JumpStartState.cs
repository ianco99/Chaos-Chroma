using Code.Scripts.States;
using UnityEngine;

namespace Patterns.FSM
{
    public class JumpStartState<T> : MovementState<T>
    {
        private readonly float jumpForce;
        
        public JumpStartState(T id, string name, float speed, float acceleration, Transform transform, Rigidbody2D rb, float jumpForce) : base(id, name, speed, acceleration, transform, rb)
        {
            this.jumpForce = jumpForce;
        }

        public override void OnEnter()
        {
            if (!IsGrounded())
            {
                Exit();
                return;
            }

            base.OnEnter();
            Debug.Log("Entered Jump Start");
            
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }

        public override void OnUpdate()
        {
            if (rb.velocity.y <= 0)
                Exit();
            
            base.OnUpdate();
        }
    }
}
