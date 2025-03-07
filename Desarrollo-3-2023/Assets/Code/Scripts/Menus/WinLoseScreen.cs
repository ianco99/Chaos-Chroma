using Code.Scripts.Abstracts;
using UnityEngine;

namespace Code.Scripts.Menus
{
    /// <summary>
    /// Controls the win/lose screen
    /// </summary>
    public class WinLoseScreen : MonoBehaviour
    {
        [SerializeField] private GameObject winScreen;
        [SerializeField] private GameObject loseScreen;
        [SerializeField] private UnityEngine.Animator animator;
        
        private static readonly int Won = UnityEngine.Animator.StringToHash("Won");
        
        /// <summary>
        /// Set the animator boolean Won to the current Won state of GameManager
        /// </summary>
        private void Awake()
        {
            animator.SetBool(Won, GameManager.Won);
        }

        /// <summary>
        /// Reload current level
        /// </summary>
        public void Restart()
        {
            GameManager.RetryLevel();
        }
        
        /// <summary>
        /// Return to the main menu
        /// </summary>
        public void Exit()
        {
            GameManager.ReturnToMenu();
        }
    }
}
