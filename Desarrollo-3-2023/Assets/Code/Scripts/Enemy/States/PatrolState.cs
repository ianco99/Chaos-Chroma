using Code.Scripts.Abstracts.Character;
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
        private Character patroller;

        public PatrolState(Rigidbody2D rb, T id, string name, Transform groundCheckPoint, Character patroller, Transform transform, PatrolSettings settings) : base(id, name, settings.patrolSpeed, settings.patrolAcceleration,
            transform, rb)
        {
            this.settings = settings;
            this.groundCheckPoint = groundCheckPoint;
            this.patroller = patroller;
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

            RaycastHit2D hit = Physics2D.Raycast(groundCheckPoint.position, -groundCheckPoint.up,
                settings.groundCheckDistance, LayerMask.GetMask("Static"));
            
            if(hit)
            {
                FlipDirection();
            }
        }

        private void CheckWall()
        {
            if (!IsGrounded())
                return;

            RaycastHit2D hit = Physics2D.Raycast(groundCheckPoint.position, groundCheckPoint.right * dir, settings.wallCheckDistance, LayerMask.GetMask("Static"));
            
            if (hit && hit.transform.name != patroller.name)
            {
                FlipDirection();
            }
        }

        public void SetDirection(float newDirection)
        {
            dir.x = newDirection;
        }

        public void FlipDirection()
        {
            dir = -dir;
        }
    }
}