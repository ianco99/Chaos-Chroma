using UnityEngine;

namespace Code.Scripts.Attack
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ProjectileSettings", fileName = "ProjectileSettings", order = 0)]
    public class ProjectileSettings : ScriptableObject
    {
        [Header("Projectile")]
        public float speed = 3f;
        public float damage = 10f;
        public float lifeTime = 1f;
        public float accuracyModifier = 10f;
    }
}