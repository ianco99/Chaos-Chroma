using Code.Scripts.SOs.States;
using Code.SOs.States;
using UnityEngine;

namespace Code.SOs.Enemy
{
    /// <summary>
    /// Settings for the ray enemy
    /// </summary>
    [CreateAssetMenu(fileName = "RayEnemySettings", menuName = "ScriptableObjects/RayEnemySettings")]
    public class RayEnemySettings : BaseEnemySettings
    {
        [Header("Patrol")] public PatrolSettings patrolSettings;

        [Header("Alert")] public AlertSettings alertSettings;

        [Header("Attack")] public AttackStartSettings attackStartSettings;

        [Header("Damaged")] public DamagedSettings damagedSettings;

        [Header("Shoot")] public TimerSettings shootTimerSettings;

        [Header("Death")] public TimerSettings deathTimerSettings;

    }
}