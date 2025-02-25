using Code.Scripts.Attack;
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
        private readonly ProjectileLauncher projectileLauncher;
        
        private Transform target;

        public ShootState(T id, string name, T nextStateID, TimerSettings timerSettings, ProjectileLauncher projectileLauncher) : base(id, name, nextStateID, timerSettings)
        {
            this.projectileLauncher = projectileLauncher;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            projectileLauncher.Shoot();
        }

        public override void OnExit()
        {
            base.OnExit();
            
            projectileLauncher.transform.rotation = Quaternion.identity;
        }
        
        public override void OnUpdate()
        {
            base.OnUpdate();
            
            projectileLauncher.SetAim(target.position - projectileLauncher.transform.position);
        }
        
        /// <summary>
        /// Sets the target position for the projectile to aim at.
        /// </summary>
        /// <param name="target">The target position to aim at.</param>
        public void SetTarget(Transform target) => this.target = target;
    }
}