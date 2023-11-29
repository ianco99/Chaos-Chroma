using Code.Scripts.Abstracts;
using System.Collections;
using UnityEngine;

namespace Code.Scripts.Events
{
    public class WinTrigger : MonoBehaviour
    {
        [SerializeField] private Animator anim;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                anim.SetTrigger("FadeIn");
                StartCoroutine(Win());
            }
        }

        private IEnumerator Win()
        {
            yield return new WaitForSeconds(1.0f);
            GameManager.Win();
        }
    }
}
