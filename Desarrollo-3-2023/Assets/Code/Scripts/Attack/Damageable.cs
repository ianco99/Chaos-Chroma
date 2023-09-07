using System;
using UnityEngine;

namespace Patterns.FSM
{
    public class Damageable : MonoBehaviour
    {
        [SerializeField] private float life = 100f;
        public event Action OnTakeDamage;

        private void Update()
        {
            if (life <= 0)
                Die();
        }

        /// <summary>
        /// Makes object lose life by damage
        /// </summary>
        /// <param name="damage">Amount of damage</param>
        public void TakeDamage(float damage)
        {
            OnTakeDamage?.Invoke();
            life -= damage;
        }

        /// <summary>
        /// Kill object
        /// </summary>
        private void Die()
        {
            if (!gameObject.CompareTag("Player"))
                Destroy(gameObject);
        }
    }
}
