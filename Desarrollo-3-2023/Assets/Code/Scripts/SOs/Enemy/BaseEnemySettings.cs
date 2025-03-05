using kuznickiEventChannel;
using UnityEngine;

namespace Code.SOs.Enemy
{
    public abstract class BaseEnemySettings : ScriptableObject
    {
        [Header("Field of View")]
        public float viewRadius;
        public float viewAngle;

        [Header("Suspect meter")]
        public float suspectMeterMinimum = 0.0f;
        public float alertValue = 40.0f;
        public float suspectMeterMaximum = 100.0f;

        [Header("Death event")]
        public VoidEventChannel deathEvent;
    }
}