using System.Collections;
using Code.Scripts.Abstracts;
using Patterns.FSM;
using UnityEngine;

namespace Code.Scripts.Attack
{
    /// <summary>
    /// Component to control the throwing of the ray
    /// </summary>
    public class RayLauncher : MonoBehaviour, IShooter
    {
        [SerializeField] private UnityEngine.LineRenderer lineRenderer;
        [SerializeField] private Transform spawnPos;
        [SerializeField] private Transform headPos;
        [SerializeField] private SpriteRenderer headSprite;
        [SerializeField] private float damage = 5;

        private Vector2 direction;

        /// <summary>
        /// Returns the transform of the spawn position.
        /// </summary>
        /// <returns>The transform of the spawn position.</returns>
        public Transform GetTransform()
        {
            return spawnPos.transform;
        }

        /// <summary>
        /// Sets the aim direction for the ray launcher based on the given aim vector.
        /// </summary>
        /// <param name="aim">The direction vector to aim the ray.</param>
        public void SetAim(Vector2 aim)
        {
            direction = aim;

            if (headSprite.flipX)
            {
                headPos.rotation = Quaternion.LookRotation(Vector3.forward, -aim);
            }
            else
                headPos.rotation = Quaternion.LookRotation(Vector3.forward, aim);
        }

        /// <summary>
        /// Shoot a ray in the direction of <see cref="direction"/>.
        /// The ray is shot from the spawn position and checks for collisions with the <see cref="Damageable"/> component.
        /// If the ray hits a collidable object with the <see cref="Damageable"/> component,
        /// it deals damage to the object and turns off the ray renderer.
        /// If the ray does not hit a collidable object, it simply turns off the ray renderer.
        /// </summary>
        public void Shoot()
        {
            RaycastHit2D hit = Physics2D.Raycast(spawnPos.position, direction, Mathf.Infinity);

            if (hit)
            {
                Debug.Log(hit.rigidbody?.name);

                if (!hit.collider.TryGetComponent(out Damageable damageable))
                {
                    //miss
                }

                if (!damageable.TakeDamage(damage, transform.position))
                {
                    //parry
                    //return;
                }
            }

            Vector3 endPos = spawnPos.position + new Vector3(direction.x, direction.y, 0);

            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, spawnPos.position);
            lineRenderer.SetPosition(1, endPos);
            StopAllCoroutines();
            StartCoroutine(DisappearRay());
            //direction;
        }

        /// <summary>
        /// Disables the line renderer after a short delay.
        /// </summary>
        /// <remarks>
        /// This is used to make the ray appear to fade out.
        /// </remarks>
        private IEnumerator DisappearRay()
        {
            yield return new WaitForSeconds(0.1f);
            lineRenderer.enabled = false;
        }
    }
}