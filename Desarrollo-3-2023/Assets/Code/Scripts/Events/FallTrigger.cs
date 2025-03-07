using Patterns.FSM;
using UnityEngine;

namespace Code.Scripts.Events
{
    /// <summary>
    /// Trigger for infinite falls
    /// </summary>
    public class FallTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Damageable damageable))
                damageable.Die();
        }
    }
}