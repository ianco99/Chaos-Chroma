using Code.Scripts.States;
using UnityEngine;

namespace Code.Scripts.Enemy.States
{
    public class PatrolState<T> : MovementState<T>
    {
        private EnemySettings settings;
        private Transform patroller;
        private Vector3 currentDirection;

        public PatrolState(Rigidbody2D rb, T id, string name, float speed, Transform transform, EnemySettings settings) : base(id, name, speed,
            transform, rb)
        {
            patroller = transform;
            this.settings = settings;
        }

        // public PatrolState(Transform patroller, EnemySettings settings, T id, string name) : base(id, name)
        // {
        //     this.patroller = patroller;
        //     this.settings = settings;
        // }

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("Entered Patrol");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            currentDirection = settings.patrolPoints[0] - patroller.position;
            var newDirection = Vector3.Scale(currentDirection.normalized, settings.patrolSpeed);
            dir = newDirection.x;
            // patroller.Translate(newDirection * Time.deltaTime);
            //rb.AddForce(newForce * Time.deltaTime, ForceMode2D.Impulse);
        }
    }
}