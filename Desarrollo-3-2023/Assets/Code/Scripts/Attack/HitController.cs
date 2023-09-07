using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Scripts.Attack
{
    /// <summary>
    /// Controls the attack (what should be hit and how much damage)
    /// </summary>
    public class HitController : MonoBehaviour
    {
        [SerializeField] private float damage = 10f;
        [SerializeField] private float hitDuration = 1f;
        
        private List<Damageable> hitObjects = new List<Damageable>();
        
        private void OnEnable()
        {
            StartCoroutine(StopOnTime());
        }

        private void Update()
        {
            var trans = transform;
            
            var hits = Physics2D.OverlapBoxAll(trans.position, trans.localScale, trans.rotation.z);
            
            foreach (var hit in hits)
            {
                // if (hit.TryGetComponent()(out Damageable damageable) && )
                    
            }
        }

        /// <summary>
        /// Turn of the object when attack is over
        /// </summary>
        /// <returns></returns>
        private IEnumerator StopOnTime()
        {
            yield return new WaitForSeconds(hitDuration);
            gameObject.SetActive(false);
        }
    }
}
