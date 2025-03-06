using Code.Scripts.Abstracts;
using Code.SOs.States;
using Patterns.FSM;
using UnityEngine;

namespace Code.Scripts.States
{
    /// <summary>
    /// Handler for shoot state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ShootState<T> : TimerTransitionState<T>
    {        
        private readonly IShooter shooter;

        private Transform target;

        public ShootState(T id, string name, T nextStateID, TimerSettings timerSettings, IShooter shooter) : base(id, name, nextStateID, timerSettings)
        {
            this.shooter = shooter;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            shooter.SetAim(target.position - shooter.GetTransform().position);

            shooter.Shoot();
        }

        public override void OnExit()
        {
            base.OnExit();

            shooter.GetTransform().rotation = Quaternion.identity;
        }

        /// <summary>
        /// Sets the target position for the projectile to aim at.
        /// </summary>
        /// <param name="target">The target position to aim at.</param>
        public void SetTarget(Transform target) => this.target = target;
    }
}