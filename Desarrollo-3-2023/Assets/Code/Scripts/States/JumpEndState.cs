using Code.Scripts.States;
using UnityEngine;

namespace Patterns.FSM
{
    /// <summary>
    /// Handler for jump end state
    /// </summary>
    public class JumpEndState<T> : MovementState<T>
    {
        public JumpEndState(T id, string name, float speed, Transform transform, Rigidbody2D rb) : base(id, name, speed, transform, rb)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("Entered Jump End");
        }

        public override void OnUpdate()
        {
            if (IsGrounded())
                Exit();
        }
    }
}
