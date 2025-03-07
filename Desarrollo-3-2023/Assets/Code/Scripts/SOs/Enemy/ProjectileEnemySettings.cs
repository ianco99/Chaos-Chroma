using Code.Scripts.SOs.States;
using Code.SOs.States;
using UnityEngine;

namespace Code.SOs.Enemy
{
    /// <summary>
    /// Settings for the projectile enemy
    /// </summary>
    [CreateAssetMenu(fileName = "ProjectileEnemySettings", menuName = "ScriptableObjects/ProjectileEnemySettings")]
    public class ProjectileEnemySettings : BaseEnemySettings
    {
        [Header("Patrol")] public PatrolSettings patrolSettings;

        [Header("Alert")] public AlertSettings alertSettings;

        [Header("Attack")] public AttackStartSettings attackStartSettings;

        [Header("Damaged")] public DamagedSettings damagedSettings;
        
        [Header("Shoot")] public TimerSettings shootTimerSettings;
        
        [Header("Death")] public TimerSettings deathTimerSettings;
    }
}