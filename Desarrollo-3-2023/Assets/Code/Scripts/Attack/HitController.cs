using System;
using System.Collections;
using System.Collections.Generic;
using Patterns.FSM;
using UnityEngine;

namespace Code.Scripts.Attack
{
    /// <summary>
    /// Controls the attack (what should be hit and how much damage)
    /// </summary>
    public class HitController : MonoBehaviour
    {
        [SerializeField] private float damage = 10f;
        [SerializeField] private float hitDelay = .2f;
        [SerializeField] private float hitDuration = 1f;
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private Transform attacker;
        public event Action OnParried;
        
        private readonly List<Damageable> hitObjects = new();
        private bool started;

        private void OnEnable()
        {
            sprite.enabled = false;
            
            StartCoroutine(BeginAttackOnTime());
        }

        private void Update()
        {
            if (!started) return;

            Transform trans = transform;

            Collider2D[] hits = Physics2D.OverlapBoxAll(trans.position, trans.localScale, trans.rotation.z);

            foreach (Collider2D hit in hits)
            {
                if (!hit.TryGetComponent(out Damageable damageable) || hitObjects.Contains(damageable)) continue;

                if (!damageable.TakeDamage(damage, attacker.transform.position))
                    OnParried?.Invoke();
                    
                hitObjects.Add(damageable);
            }
        }

        /// <summary>
        /// Start attack after delay
        /// </summary>
        /// <returns></returns>
        private IEnumerator BeginAttackOnTime()
        {
            yield return new WaitForSeconds(hitDelay);
            
            StartCoroutine(StopOnTime());
            sprite.enabled = true;
            started = true;
        }

        /// <summary>
        /// Turn of the object when attack is over
        /// </summary>
        /// <returns></returns>
        private IEnumerator StopOnTime()
        {
            yield return new WaitForSeconds(hitDuration);
            hitObjects.Clear();
            gameObject.SetActive(false);
        }
    }
}