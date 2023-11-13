using UnityEngine;

namespace Patterns.FSM
{
    /// <summary>
    /// Handler for attack state
    /// </summary>
    public class KnockBackBlockState<T> : BlockState<T>
    {
        private readonly Rigidbody2D rb;
        private float force;
        private float maxTime = 2.0f;
        private float currentTime = 0.0f;
        private Vector2 pushDirection = Vector2.right;

        public KnockBackBlockState(T id, string name, Damageable damageable, Rigidbody2D rb, float force) : base(id, name, damageable)
        {
            this.rb = rb;
            this.force = force;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            ResetState();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            currentTime += Time.deltaTime;
        }

        public void SetDirection(Vector2 newDirection)
        {
            pushDirection = newDirection;
        }

        public void SetForce(float newForce)
        {
            force = newForce;
        }

        public void ResetState()
        {
            rb.AddForce(pushDirection * force, ForceMode2D.Impulse);
            currentTime = 0.0f;
        }

        public bool FinishedTimer() => currentTime >= maxTime;
    }
}
