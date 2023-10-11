using Code.Scripts.States;
using UnityEngine;

namespace Patterns.FSM
{
    public class GodState<T> : MovementState<T>
    {
        public GodState(T id, string name, float speed, float accel, Transform transform, Rigidbody2D rb) : base(id, name, speed, accel, transform, rb)
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
            transform.Translate(dir * (Time.deltaTime * speed));
        }

        public void Toggle()
        {
            if (Active)
                Exit();
            else
                Enter();
        }
    }
}
