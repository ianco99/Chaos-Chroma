using Code.Scripts.Abstracts;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Scripts.Menus
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject levelSelectCanvas;
        [SerializeField] private GameObject mainMenuCanvas;
        [SerializeField] private GameObject creditsMenuCanvas;
        [SerializeField] private GameObject firstSelectedObjectLevelSelector;
        [SerializeField] private TextMeshProUGUI versionText;
        
        private void Awake()
        {
            versionText.text = "v" + Application.version;
        }

        public void StartGame()
        {
            mainMenuCanvas.SetActive(false);
            levelSelectCanvas.SetActive(true);
            
            EventSystem.current.SetSelectedGameObject(firstSelectedObjectLevelSelector);
        }

        public void ShowCredits()
        {
            creditsMenuCanvas.SetActive(true);
            mainMenuCanvas.SetActive(false);
        }

        public void HideCredits()
        {
            creditsMenuCanvas.SetActive(false);
            mainMenuCanvas.SetActive(true);
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