using Code.Scripts.States;
using Code.SOs.Enemy;
using System;
using UnityEngine;

namespace Patterns.FSM
{
    /// <summary>
    /// Handler for movement state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PatrolState<T> : MovementState<T>
    {
        private readonly EnemySettings settings;
        private readonly Transform patroller;
        private Transform groundCheckPoint;
        private int curPatrolPoint;

        public PatrolState(Rigidbody2D rb, T id, string name, Transform groundCheckPoint, Transform transform, EnemySettings settings) : base(id, name, settings.patrolSpeed, settings.acceleration,
            transform, rb)
        {
            patroller = transform;
            this.settings = settings;
            this.groundCheckPoint = groundCheckPoint;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            dir = -1f;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            //CheckDirection();
            CheckGround();
            CheckWall();
             
        }

        /// <summary>
        /// Update direction if reached patrol point
        /// </summary>
        private void CheckDirection()
        {
            if (!(Mathf.Abs(patroller.position.x - settings.patrolPoints[curPatrolPoint].x) < .5f)) return;
            
            curPatrolPoint++;
                
            if (curPatrolPoint >= settings.patrolPoints.Length)
                curPatrolPoint = 0;
        }

        private void CheckGround()
        {
            if (!IsGrounded())
                return;

            if(!Physics2D.Raycast(groundCheckPoint.position, Vector2.down, settings.groundCheckDistance))
            {
                Debug.Log("Ground not found");
                FlipDirection();
            }
        }

        private void CheckWall()
        {
            if (!IsGrounded())
                return;

            if (Physics2D.Raycast(groundCheckPoint.position, groundCheckPoint.right * dir, settings.wallCheckDistance))
            {
                Debug.Log("Wall found");
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