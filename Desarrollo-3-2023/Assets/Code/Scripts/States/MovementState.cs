using Patterns.FSM;
using UnityEngine;

namespace Code.Scripts.States
{
    /// <summary>
    /// Handler for movement state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MovementState<T> : BaseState<T>
    {
        protected readonly Rigidbody2D rb;
        protected readonly Transform transform;
        private readonly float speed;
        private readonly float accel;

        public float dir;

        public MovementState(T id, string name, float speed, float accel, Transform transform, Rigidbody2D rb) : base(id, name)
        {
            this.speed = speed;
            this.accel = accel;
            this.transform = transform;
            this.rb = rb;
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (Mathf.Approximately(dir, 0))
                Exit();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            MoveInDirection(dir);
            ClampSpeed();
        }

        /// <summary>
        /// Moves current object in given direction
        /// </summary>
        /// <param name="direction"></param>
        private void MoveInDirection(float direction)
        {
            rb.AddForce(Vector2.right * (direction * accel * Time.fixedDeltaTime), ForceMode2D.Impulse);
        }

        private void ClampSpeed()
        {
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -speed * Time.fixedDeltaTime, speed * Time.fixedDeltaTime), rb.velocity.y);
        }

        protected bool IsGrounded()
        {
            if (!transform)
                return false;

            var pos = transform.position;
            var scale = transform.localScale;
            var hit = Physics2D.Raycast(pos, Vector2.down, 2f * scale.y, ~(1 << 6 | 1 << 7 | 1 << 8));
            
            Debug.DrawRay(pos, Vector2.down * (1.1f * scale.y));
            
            return hit.collider;
        }
    }
}