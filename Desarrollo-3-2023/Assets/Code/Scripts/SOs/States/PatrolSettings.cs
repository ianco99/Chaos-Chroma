using Code.SOs.States;
using UnityEngine;

namespace Code.Scripts.SOs.States
{
    /// <summary>
    /// Settings for the patrol state
    /// </summary>
    [CreateAssetMenu(fileName = "PatrolSettingsSO", menuName = "ScriptableObjects/StateSettings/PatrolSettings")]
    public class PatrolSettings : ScriptableObject, IPatrolSettings
    {
        public MoveSettings moveSettings;

        [Header("GroundCheck")]
        public float groundCheckDistance;
        public LayerMask groundLayer;

        [Header("WallCheck")]
        public LayerMask wallLayer;
        public float wallCheckDistance;
    }
}
