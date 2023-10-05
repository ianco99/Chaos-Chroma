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
        
        public event Action<Vector2> OnTakeDamage;
        public event Action<Vector2> OnBlock;

        private void Update()
        {
            if (life <= 0)
                Die();
        }

        /// <summary>
        /// Makes object lose life by damage
        /// </summary>
        /// <param name="damage">Amount of damage</param>
        /// <param name="attackOrigin">Position where the attack comes from</param>
        public bool TakeDamage(float damage, Vector2 attackOrigin)
        {
            if (parry)
                return false;

            if (block)
            {
                OnBlock?.Invoke(attackOrigin);
                return true;
            }
            
            OnTakeDamage?.Invoke(attackOrigin);
            life -= damage;

            return true;
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
