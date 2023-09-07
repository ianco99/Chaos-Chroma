using Code.Scripts.States;
using UnityEngine;

namespace Patterns.FSM
{
    public class JumpStartState<T> : MovementState<T>
    {
        private float jumpForce;
        
        public JumpStartState(T id, string name, float speed, Transform transform, Rigidbody2D rb, float jumpForce) : base(id, name, speed, transform, rb)
        {
            this.jumpForce = jumpForce;
        }

        public override void OnEnter()
        {
            if (!IsGrounded())
                Exit();
            
            base.OnEnter();
            Debug.LogError("Entered Jump Start");
            
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }

        public override void OnUpdate()
        {
            if (rb.velocity.y <= 0)
                Exit();
        }
    }
}
