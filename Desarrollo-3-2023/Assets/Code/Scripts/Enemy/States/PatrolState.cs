using Code.Scripts.States;
using Code.SOs.Enemy;
using System;
using UnityEngine;

namespace Patterns.FSM
{
    /// <summary>
    /// Handler for state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PatrolState<T> : MovementState<T>
    {
        private readonly PatrolSettings settings;
        private Transform groundCheckPoint;

        public PatrolState(Rigidbody2D rb, T id, string name, Transform groundCheckPoint, Transform transform, PatrolSettings settings) : base(id, name, settings.patrolSpeed, settings.patrolAcceleration,
            transform, rb)
        {
            this.settings = settings;
            this.groundCheckPoint = groundCheckPoint;
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            CheckGround();
            CheckWall();
             
        }

        private void CheckGround()
        {
            if (!IsGrounded())
                return;

            if(!Physics2D.Raycast(groundCheckPoint.position, -groundCheckPoint.up, settings.groundCheckDistance, LayerMask.GetMask("Default")))
            {
                FlipDirection();
            }
        }

        private void CheckWall()
        {
            if (!IsGrounded())
                return;

            if (Physics2D.Raycast(groundCheckPoint.position, groundCheckPoint.right * dir, settings.wallCheckDistance, LayerMask.GetMask("Default")))
            {
                FlipDirection();
            }
        }

        public void SetDirection(float newDirection)
        {
            dir = newDirection;
        }

        public void FlipDirection()
        {
            dir = -dir;
        }
    }
}