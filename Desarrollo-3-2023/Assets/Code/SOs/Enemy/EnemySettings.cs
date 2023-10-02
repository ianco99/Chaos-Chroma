using UnityEngine;

namespace Code.SOs.Enemy
{
    [CreateAssetMenu(fileName = "EnemySettings", menuName = "ScriptableObjects/EnemySettings")]

    public class EnemySettings : ScriptableObject
    {
        public Vector3[] patrolPoints;
        public float alertSpeed;
        public float acceleration;

        [Header("Patrol")]
        public float groundCheckDistance;
        public float wallCheckDistance;
        public float patrolSpeed;


        [Header("Field of View")]
        public float viewRadius;
        public float viewAngle;

        [Header("Suspect meter")]
        public float suspectMeterMinimum = 0.0f;
        public float alertValue = 40.0f;
        public float suspectMeterMaximum = 100.0f;
    }
}
