using Code.Scripts.Abstracts.Character;
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
        private readonly Character patroller;
        private Transform alertTarget;
        private Transform groundCheckPoint;

        public AlertState(Rigidbody2D rb, T id, string name, Character patroller, Transform transform, AlertSettings settings, Transform groundCheckPoint) : base(id, name, settings.alertSpeed, settings.alertAcceleration,
            transform, rb)
        {
            this.patroller = patroller;
            this.settings = settings;
            this.groundCheckPoint = groundCheckPoint;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            if (patroller.IsFacingRight())
            {
                dir = patroller.transform.right;
            }
            else
            {
                dir = -patroller.transform.right;
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if(alertTarget)
            {
                dir = alertTarget.transform.position - patroller.transform.position;
            }

            Vector3 newDirection = dir.normalized;
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