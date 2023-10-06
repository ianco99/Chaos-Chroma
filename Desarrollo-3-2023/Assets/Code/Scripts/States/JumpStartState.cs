using Code.Scripts.States;
using System;
using System.Collections;
using UnityEngine;

namespace Patterns.FSM
{
    public class JumpStartState<T> : MovementState<T>
    {
        private MonoBehaviour behaviourClass;
        private readonly float jumpForce;
        
        public JumpStartState(T id, MonoBehaviour coroutineContainer, string name, float speed, float acceleration, Transform transform, Rigidbody2D rb, float jumpForce) : base(id, name, speed, acceleration, transform, rb)
        {
            this.jumpForce = jumpForce;
            this.behaviourClass = coroutineContainer;
        }

        public override void OnEnter()
        {
            if (!IsGrounded())
            {
                Exit();
                return;
            }

            base.OnEnter();

            behaviourClass.StartCoroutine(AddForce(transform.up * jumpForce, ForceMode2D.Impulse));
        }

        private IEnumerator AddForce(Vector3 force, ForceMode2D mode)
        {
            yield return new WaitForFixedUpdate();
            rb.AddForce(force, mode);
        }

        public override void OnFixedUpdate()
        {
            if (rb.velocity.y < 0)
                Exit();
            
            base.OnFixedUpdate();
        }
    }
}
