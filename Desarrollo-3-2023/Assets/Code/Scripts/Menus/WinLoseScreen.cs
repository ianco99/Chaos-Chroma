using Code.Scripts.Abstracts;
using UnityEngine;

namespace Code.Scripts.Menus
{
    public class WinLoseScreen : MonoBehaviour
    {
        [SerializeField] private GameObject winScreen;
        [SerializeField] private GameObject loseScreen;
        [SerializeField] private Animator animator;
        
        private static readonly int Won = Animator.StringToHash("Won");
        
        private void Awake()
        {
            animator.SetBool(Won, GameManager.Won);
        }

        public void Restart()
        {
            GameManager.RetryLevel();
        }
        
        public void Exit()
        {
            GameManager.ReturnToMenu();
        }
    }
}
