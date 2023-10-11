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
        private Vector3 currentDirection;

        public AlertState(Rigidbody2D rb, T id, string name, Transform transform, AlertSettings settings) : base(id, name, settings.alertSpeed, settings.alertAcceleration,
            transform, rb)
        {
            patroller = transform;
            this.settings = settings;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            currentDirection = alertTarget.position - patroller.position;
            Vector3 newDirection = currentDirection.normalized;
            dir.x = newDirection.x;
        }

        public void SetTarget(Transform target)
        {
            alertTarget = target;
        }
    }
}