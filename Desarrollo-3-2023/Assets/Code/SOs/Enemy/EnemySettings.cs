using UnityEngine;

namespace Code.SOs.Enemy
{
    [CreateAssetMenu(fileName = "EnemySettings", menuName = "ScriptableObjects/EnemySettings")]

    public class EnemySettings : ScriptableObject
    {
        public Vector3[] patrolPoints;
        public float patrolSpeed;
    }
}
