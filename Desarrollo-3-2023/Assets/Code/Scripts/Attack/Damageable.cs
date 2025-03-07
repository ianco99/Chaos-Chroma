using System;
using System.Collections;
using Code.Scripts.Abstracts;
using UnityEngine;
using UnityEngine.UI;

namespace Patterns.FSM
{
    /// <summary>
    /// Controls the life of the object
    /// </summary>
    public class Damageable : MonoBehaviour
    {
        [SerializeField] private float startLife = 100f;
        [SerializeField] private Image lifeBar;

        public bool parry;
        private float life;
        private bool block;

        public event Action<Vector2> OnTakeDamage;
        public event Action<Vector2> OnBlock;
        public event Action<Vector2> OnParry;
        public kuznickiEventChannel.VoidEventChannel OnDeath;

        private void Awake()
        {
            life = startLife;
        }

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
            {
                OnParry?.Invoke(attackOrigin);
                return false;
            }

            if (block)
            {
                OnBlock?.Invoke(attackOrigin);
                return true;
            }

            life -= damage;

            if (lifeBar)
                lifeBar.fillAmount = life / startLife;

            OnTakeDamage?.Invoke(attackOrigin);
            return true;
        }

        /// <summary>
        /// Kill object
        /// </summary>
        public void Die()
        {
            OnDeath?.RaiseEvent();

            if (gameObject.CompareTag("Player"))
                GameManager.Lose();
        }

        /// <summary>
        /// Heal the object by a given amount.
        /// </summary>
        /// <param name="heal">The amount to heal by.</param>
        /// <remarks>
        /// This will increase the object's life by the given amount,
        /// and update the life bar if one is present.
        /// </remarks>
        public void Heal(float heal)
        {
            life += heal;

            if (lifeBar)
                lifeBar.fillAmount = life / startLife;
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
        /// Get the current life of the object.
        /// </summary>
        /// <returns>The current life of the object.</returns>
        public float GetLife()
        {
            return life;
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