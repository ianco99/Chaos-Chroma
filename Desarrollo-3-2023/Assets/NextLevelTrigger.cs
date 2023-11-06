using Code.Scripts.Abstracts;
using UnityEngine;


public class NextLevelTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            GameManager.LoadLevel1();
    }
}
