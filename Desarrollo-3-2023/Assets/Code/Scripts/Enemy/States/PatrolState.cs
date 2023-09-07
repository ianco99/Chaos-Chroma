using Code.Scripts.States;
using Code.SOs.Enemy;
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
        private Vector3 currentDirection;
        private int curPatrolPoint;

        public PatrolState(Rigidbody2D rb, T id, string name, Transform transform, EnemySettings settings) : base(id, name, settings.patrolSpeed, settings.acceleration,
            transform, rb)
        {
            patroller = transform;
            this.settings = settings;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("Entered Patrol");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            CheckDirection();
            
            currentDirection = settings.patrolPoints[curPatrolPoint] - patroller.position;
            var newDirection = currentDirection.normalized;
            dir = newDirection.x;
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
    }
}