using Code.Scripts.SOs.States;
using Code.Scripts.States.Settings;
using Code.SOs.States;
using UnityEngine;

namespace Code.SOs.Enemy
{
    /// <summary>
    /// Settings for the common enemy
    /// </summary>
    [CreateAssetMenu(fileName = "EnemySettings", menuName = "ScriptableObjects/EnemySettings")]
    public class EnemySettings : BaseEnemySettings
    {

        [Header("Patrol")]
        public PatrolSettings patrolSettings;

        [Header("Alert")]
        public AlertSettings alertSettings;
        
        [Header("Attack")]
        public AttackStartSettings attackStartSettings;

        [Header("Damaged")]
        public DamagedSettings damagedSettings;

        [Header("Block")]
        public KnockbackBlockSettings blockSettings;

        [Header("Parry")]
        public ParrySettings parrySettings;

        [Header("Parried")]
        public DamagedSettings parriedSettings;

        [Header("Parried")]
        public TimerSettings deathSettings;
    }
}
