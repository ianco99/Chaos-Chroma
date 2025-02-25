using Code.SOs.States;
using kuznickiEventChannel;
using UnityEngine;

namespace Code.SOs.Enemy
{
    [CreateAssetMenu(fileName = "ProjectileEnemySettings", menuName = "ScriptableObjects/ProjectileEnemySettings")]
    public class ProjectileEnemySettings : BaseEnemySettings
    {
        [Header("Patrol")] public PatrolSettings patrolSettings;

        [Header("Alert")] public AlertSettings alertSettings;

        [Header("Attack")] public AttackStartSettings attackStartSettings;

        [Header("Damaged")] public DamagedSettings damagedSettings;
        
        [Header("Shoot")] public TimerSettings shootTimerSettings;
        
        [Header("Death")] public TimerSettings deathTimerSettings;

        [Header("Field of View")] public float viewRadius;
        public float viewAngle;

        [Header("Suspect meter")] public float suspectMeterMinimum = 0.0f;
        public float alertValue = 40.0f;
        public float suspectMeterMaximum = 100.0f;

        [Header("Death event")] public VoidEventChannel deathEvent;
    }
}