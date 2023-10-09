using Code.Scripts.States;
using Code.SOs.Enemy;
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
        private Vector3 currentDirection;

        public AlertState(Rigidbody2D rb, T id, string name, Transform transform, AlertSettings settings) : base(id, name, settings.alertSpeed, settings.alertAcceleration,
            transform, rb)
        {
            this.patroller = transform;
            this.settings = settings;
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            currentDirection = alertTarget.position - patroller.position;
            var newDirection = currentDirection.normalized;
            dir = newDirection.x;
        }

        public void SetTarget(Transform target)
        {
            alertTarget = target;
        }
    }
}