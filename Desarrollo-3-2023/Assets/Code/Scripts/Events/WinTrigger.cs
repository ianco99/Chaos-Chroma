using Code.Scripts.Abstracts;
using System.Collections;
using UnityEngine;

namespace Code.Scripts.Events
{
    /// <summary>
    /// End level trigger
    /// </summary>
    public class WinTrigger : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Animator anim;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                anim.SetTrigger("FadeIn");
                StartCoroutine(Win());
            }
        }

        /// <summary>
        /// Initiates the win sequence by waiting for a set duration before triggering the win condition.
        /// </summary>
        /// <remarks>
        /// This coroutine waits for 1 second before calling the GameManager's Win method,
        /// which sets the game state to a winning condition.
        /// </remarks>
        /// <returns>An IEnumerator for coroutine management.</returns>
        private IEnumerator Win()
        {
            yield return new WaitForSeconds(1.0f);
            GameManager.Win();
        }
    }
}