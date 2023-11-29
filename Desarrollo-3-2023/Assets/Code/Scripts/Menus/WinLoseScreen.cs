using Code.Scripts.Abstracts;
using UnityEngine;

namespace Code.Scripts.Menus
{
    public class WinLoseScreen : MonoBehaviour
    {
        [SerializeField] private GameObject winScreen;
        [SerializeField] private GameObject loseScreen;

        private void Awake()
        {
            if (GameManager.Won)
                winScreen.SetActive(true);
            else
                loseScreen.SetActive(true);
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
