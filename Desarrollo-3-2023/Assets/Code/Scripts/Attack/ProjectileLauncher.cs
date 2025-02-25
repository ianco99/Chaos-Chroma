using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Code.Scripts.Attack
{
    /// <summary>
    /// Projectile launcher component
    /// </summary>
    public class ProjectileLauncher : MonoBehaviour
    {
        private static GameObject _projectiles;

        [Header("Settings")] [SerializeField] private int bullets;
        [SerializeField] private Projectile projectile;

        [Header("References")] [SerializeField]
        private Transform spawnPos;

        [SerializeField] private List<ProjectileSettings> projectileSettingsList = new();
        [SerializeField] private Rigidbody2D holderRb;

        private ObjectPool<Projectile> pool;

        private Vector2 aim;

        private void Awake()
        {
            pool = new ObjectPool<Projectile>(CreateProjectile, OnTakeFromPool, OnReturnedToPool,
                OnProjectileDestroyed);
        }

        /// <summary>
        /// Updates the rotation of the projectile launcher to face the current aim direction.
        /// </summary>
        private void UpdateRotation()
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, aim);
        }

        /// <summary>
        /// Called when a projectile is returned to the pool.
        /// Disables the projectile's game object to make it inactive.
        /// </summary>
        /// <param name="obj">The projectile being returned to the pool.</param>
        private static void OnReturnedToPool(Projectile obj)
        {
            obj.gameObject.SetActive(false);
        }

        /// <summary>
        /// Called when a projectile is taken from the pool.
        /// Resets the projectile's position to the spawn position and enables it.
        /// </summary>
        /// <param name="obj">The projectile being taken from the pool.</param>
        private void OnTakeFromPool(Projectile obj)
        {
            obj.gameObject.SetActive(true);

            obj.transform.position = spawnPos.position;
        }

        /// <summary>
        /// Creates and returns a new instance of the Projectile.
        /// </summary>
        /// <returns>A new Projectile instance.</returns>
        private Projectile CreateProjectile()
        {
            if (_projectiles == null)
                _projectiles = new GameObject("Projectiles");

            Projectile instance = Instantiate(projectile, _projectiles.transform);
            instance.EndProjectile += OnProjectileEndProjectile;

            return instance;
        }

        /// <summary>
        /// Called when a projectile is destroyed.
        /// Unsubscribes from projectile events to prevent memory leaks.
        /// </summary>
        /// <param name="obj">The projectile that was destroyed.</param>
        private void OnProjectileDestroyed(Projectile obj)
        {
            obj.EndProjectile -= OnProjectileEndProjectile;
        }

        /// <summary>
        /// Called when a projectile hits something.
        /// Releases the projectile back into the pool so it can be reused.
        /// </summary>
        /// <param name="obj">The projectile that hit something.</param>
        private void OnProjectileEndProjectile(Projectile obj)
        {
            if (obj.gameObject.activeSelf)
                pool.Release(obj);
        }

        /// <summary>
        /// Shoot a projectile in the direction of `aim`
        /// </summary>
        public void Shoot()
        {
            if (bullets <= 0)
                return;

            bullets--;

            UpdateRotation();
            
            Vector2 inhVel = holderRb != null ? holderRb.velocity : Vector2.zero;

            pool.Get().Shoot(aim, projectileSettingsList[Random.Range(0, projectileSettingsList.Count)], inhVel);
        }

        /// <summary>
        /// Sets the aim direction for the next projectile shot.
        /// </summary>
        /// <param name="aim">The direction to aim the projectile.</param>
        public void SetAim(Vector2 aim) => this.aim = aim;
    }
}