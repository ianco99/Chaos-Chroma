using System;
using System.Collections;
using Patterns.FSM;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Scripts.Attack
{
    /// <summary>
    /// Projectile controller
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;

        private ProjectileSettings settings;

        public event Action<Projectile> EndProjectile;

        /// <summary>
        /// Shoot a projectile in the given direction with the given settings and inherited velocity.
        /// </summary>
        /// <param name="aim">The direction to shoot the projectile.</param>
        /// <param name="settings">The settings for the projectile.</param>
        /// <param name="inheritedVel">The velocity to inherit from the parent object.</param>
        public void Shoot(Vector2 aim, ProjectileSettings settings, Vector2 inheritedVel)
        {
            rb.velocity = inheritedVel;

            this.settings = settings;

            transform.rotation = Quaternion.LookRotation(Vector3.forward, aim);
            transform.Rotate(Vector3.forward, Random.Range(-settings.accuracyModifier / 2f, settings.accuracyModifier / 2f));

            rb.AddForce(transform.up * (settings.speed * Time.fixedDeltaTime), ForceMode2D.Impulse);

            StartCoroutine(WaitAndDestroy(settings.lifeTime));
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.gameObject.TryGetComponent(out Damageable damageable)) return;

            if (!damageable.TakeDamage(settings.damage, transform.position))
            {
                rb.velocity *= -1f;
                return;
            }

            EndProjectile?.Invoke(this);
        }

        /// <summary>
        /// Waits for the given time and then destroys the projectile.
        /// </summary>
        /// <param name="time">The time to wait before destroying the projectile.</param>
        /// <returns>A coroutine that waits for the given time and then destroys the projectile.</returns>
        private IEnumerator WaitAndDestroy(float time)
        {
            yield return new WaitForSeconds(time);

            EndProjectile?.Invoke(this);
        }
    }
}