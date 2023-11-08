using Code.Scripts.Abstracts;
using UnityEngine;

namespace Code.Scripts.Menus
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject levelSelectCanvas;
        [SerializeField] private GameObject mainMenuCanvas;
        public void StartGame()
        {
            levelSelectCanvas.SetActive(true);
            mainMenuCanvas.SetActive(false);
        }
        
        public void SelectTutorial()
        {
            GameManager.StartTutorial();
        }

        public void SelectLevel1()
        {
            GameManager.LoadLevel1();
        }

        public void Exit()
        {
            GameManager.Exit();
        }
    }
}