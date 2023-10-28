using Code.Scripts.Abstracts;
using UnityEngine;

namespace Code.Scripts.Events
{
    public class WinTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                GameManager.Win();
        }
    }
}
