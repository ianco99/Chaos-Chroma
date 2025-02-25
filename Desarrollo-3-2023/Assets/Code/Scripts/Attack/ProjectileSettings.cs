using UnityEngine;

namespace Code.Scripts.Attack
{
    [CreateAssetMenu(menuName = "ProjectileSettings", fileName = "ScriptableObjects/ProjectileSettings", order = 0)]
    public class ProjectileSettings : ScriptableObject
    {
        [Header("Projectile")]
        public float speed = 3f;
        public float damage = 1f;
        public float lifeTime = 1f;
        public float accuracyModifier = 10f;
    }
}