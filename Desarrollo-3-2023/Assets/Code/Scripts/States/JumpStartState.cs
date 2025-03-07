using Code.Scripts.States;
using System.Collections;
using Code.SOs.States;
using UnityEngine;

namespace Patterns.FSM
{
    public class JumpStartState<T> : MovementState<T>
    {
        private AK.Wwise.Event playJump;
        private MonoBehaviour behaviourClass;
        private JumpStartSettings jumpStartSettings;

        private float originalGravScale;

        public JumpStartState(T id, MonoBehaviour coroutineContainer, string name, JumpStartSettings startSettings,
            AK.Wwise.Event playJump, Transform transform, Rigidbody2D rb) : base(id, name, startSettings.moveSettings,
            transform, rb)
        {
            this.playJump = playJump;
            jumpStartSettings = startSettings;
            behaviourClass = coroutineContainer;
        }

        public override void OnEnter()
        {
            if (!IsGrounded())
            {
                Exit();
                return;
            }

            base.OnEnter();

            behaviourClass.StartCoroutine(AddForce(transform.up * jumpStartSettings.force, ForceMode2D.Impulse));

            originalGravScale = rb.gravityScale;
            // rb.gravityScale *= 2f;
        }

        public override void OnExit()
        {
            base.OnExit();

            rb.gravityScale = originalGravScale;
        }

        public override void OnUpdate()
        {
        }

        /// <summary>
        /// Adds the given force to the object's rigidbody, using the given force mode.
        /// This coroutine waits for the next fixed update before applying the force,
        /// and also posts the jump sound event.
        /// </summary>
        /// <param name="force">The force to apply</param>
        /// <param name="mode">The force mode to use</param>
        private IEnumerator AddForce(Vector3 force, ForceMode2D mode)
        {
            yield return new WaitForFixedUpdate();
            playJump.Post(behaviourClass.gameObject);
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