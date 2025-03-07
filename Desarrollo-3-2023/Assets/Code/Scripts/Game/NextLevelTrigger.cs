using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Scripts.Game
{
    /// <summary>
    /// A trigger for loading the next level.
    /// </summary>
    public class NextLevelTrigger : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Animator anim;
        [SerializeField] private string levelName;

        private static readonly int FadeIn = UnityEngine.Animator.StringToHash("FadeIn");

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            anim.SetTrigger(FadeIn);
            StartCoroutine(NextLevel());
        }

        /// <summary>
        /// Waits for a specified duration and then loads the specified level.
        /// </summary>
        /// <returns>An IEnumerator used for coroutine management.</returns>
        private IEnumerator NextLevel()
        {
            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene(levelName);
        }
    }
}