using Code.Scripts.States;
using Code.SOs.States;
using UnityEngine;

namespace Patterns.FSM
{
    public class GodState<T> : MovementState<T>
    {
        public GodState(T id, string name, GodSettings settings, Transform transform, Rigidbody2D rb) : base(id, name,
            settings.moveSettings, transform, rb)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();

            rb.isKinematic = true;
        }

        public override void OnExit()
        {
            base.OnExit();

            rb.isKinematic = false;
        }

        public override void OnUpdate()
        {
            transform.Translate(dir * (Time.deltaTime * settings.speed));
        }

        /// <summary>
        /// Toggles the state between active and inactive.
        /// When active, the state is entered and the god mode is activated.
        /// When inactive, the state is exited and the god mode is deactivated.
        /// </summary>
        public void Toggle()
        {
            if (Active)
                Exit();
            else
                Enter();
        }
    }
}