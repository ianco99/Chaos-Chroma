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
        [SerializeField] private bool finishByDuration = true;
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private Transform attacker;
        [SerializeField] private SpriteRenderer characterOutline;
        [SerializeField] private Color objectiveColor;
        [SerializeField] private bool showHitBox;
        
        public event Action OnParried;
        
        private readonly List<Damageable> hitObjects = new();
        private bool started;
        private float t;

        private void OnEnable()
        {
            if (sprite)
                sprite.enabled = false;
            
            started = false;

            if (characterOutline)
                characterOutline.color = Color.white;
            
            StartCoroutine(BeginAttackOnTime());
            t = 0;
        }

        private void OnDisable()
        {
            if (characterOutline)
                characterOutline.color = Color.clear;
        }

        private void Update()
        {
            if (characterOutline)
                UpdateCharacterOutlineColor();

            Hit();
        }

        private void Hit()
        {
            if (!started) return;

            Transform trans = transform;
            Vector2 colliderSize = trans.GetComponent<BoxCollider2D>().size;
            Collider2D[] hits = Physics2D.OverlapBoxAll(trans.position, colliderSize * trans.localScale, trans.rotation.z);

            foreach (Collider2D hit in hits)
            {
                if (!hit.TryGetComponent(out Damageable damageable) || hitObjects.Contains(damageable) || hit.transform == attacker) continue;

                if (!damageable.TakeDamage(damage, attacker.transform.position))
                {
                    OnParried?.Invoke();
                    Stop();
                    return;
                }   
                
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
            
            if (sprite && showHitBox)
                sprite.enabled = true;
            
            started = true;
            
            if (finishByDuration)
            {
                StartCoroutine(StopOnTime());
            }
            else
            {
                Hit();
            }
        }

        /// <summary>
        /// Turn of the object when attack is over
        /// </summary>
        /// <returns></returns>
        private IEnumerator StopOnTime()
        {
            yield return new WaitForSeconds(hitDuration);
            Stop();
        }

        /// <summary>
        /// Interrupt the attack
        /// </summary>
        public void Stop()
        {
            hitObjects.Clear();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Sets the color of the character outline to the current state of the attack
        /// </summary>
        private void UpdateCharacterOutlineColor()
        {
            t += Time.deltaTime / hitDelay;

            Color color = Vector4.Lerp(Color.white, objectiveColor, t);
            
            characterOutline.color = color;
        }
    }
}