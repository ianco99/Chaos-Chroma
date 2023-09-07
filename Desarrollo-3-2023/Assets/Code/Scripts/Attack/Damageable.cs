using System;
using System.Collections;
using UnityEngine;

namespace Patterns.FSM
{
    public class Damageable : MonoBehaviour
    {
        [SerializeField] private float life = 100f;
        
        public bool parry;
        private bool block;
        
        public event Action OnTakeDamage;
        public event Action OnParry;
        public event Action OnBlock;

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

        /// <summary>
        /// Start action block
        /// </summary>
        public void StartBlock()
        {
            block = true;
        }

        /// <summary>
        /// Stop action block
        /// </summary>
        public void StopBlock()
        {
            block = false;
        }
        
        /// <summary>
        /// Start action parry
        /// </summary>
        public void StartParry(float parryDuration)
        {
            StartCoroutine(Parry(parryDuration));
        }

        /// <summary>
        /// Set to block and parry and turn off on parry on time
        /// </summary>
        /// <returns></returns>
        private IEnumerator Parry(float parryDuration)
        {
            parry = true;

            yield return new WaitForSeconds(parryDuration);
            parry = false;
        }
    }
}
