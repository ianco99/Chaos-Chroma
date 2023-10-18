using Code.Scripts.States;
using UnityEngine;

namespace Patterns.FSM
{
    /// <summary>
    /// Handler for movement state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AlertState<T> : MovementState<T>
    {
        private readonly AlertSettings settings;
        private readonly Transform patroller;
        private Transform alertTarget;
        private Transform groundCheckPoint;
        private Vector3 currentDirection;

        public AlertState(Rigidbody2D rb, T id, string name, Transform transform, AlertSettings settings, Transform groundCheckPoint) : base(id, name, settings.alertSpeed, settings.alertAcceleration,
            transform, rb)
        {
            patroller = transform;
            this.settings = settings;
            this.groundCheckPoint = groundCheckPoint;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (alertTarget)
                currentDirection = alertTarget.position - patroller.position;
            else
            {
                currentDirection = patroller.right;
            }

            Vector3 newDirection = currentDirection.normalized;
            dir.x = newDirection.x;

            CheckGround();
        }

        private void CheckGround()
        {
            if (!IsGrounded())
                return;

            if (!Physics2D.Raycast(groundCheckPoint.position, -groundCheckPoint.up, settings.groundCheckDistance, LayerMask.GetMask("Default")))
            {
                base.speed = 0;
            }
            else
            {
                base.speed = settings.alertSpeed;
            }
        }

        public void SetTarget(Transform target)
        {
            alertTarget = target;
        }
    }
}