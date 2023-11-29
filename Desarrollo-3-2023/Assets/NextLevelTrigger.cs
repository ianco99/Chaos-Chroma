using Code.Scripts.Abstracts;
using System.Collections;
using UnityEngine;


public class NextLevelTrigger : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetTrigger("FadeIn");
            StartCoroutine(NextLevel());
        }
    }

    public IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(1.0f);
        GameManager.LoadLevel1();
    }
}
