using Code.Scripts.Abstracts;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Code.Scripts.Menus
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject levelSelectCanvas;
        [SerializeField] private GameObject mainMenuCanvas;
        [SerializeField] private GameObject creditsMenuCanvas;
        [SerializeField] private GameObject firstSelectedObjectLevelSelector;
        [SerializeField] private GameObject firstSelectedObjectCredits;
        [SerializeField] private GameObject firstSelectedObjectMainMenu;
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
            EventSystem.current.SetSelectedGameObject(firstSelectedObjectCredits);
        }

        public void HideCredits()
        {
            creditsMenuCanvas.SetActive(false);
            mainMenuCanvas.SetActive(true);
            EventSystem.current.SetSelectedGameObject(firstSelectedObjectMainMenu);
        }

        public void BackToMainMenu()
        {
            levelSelectCanvas.SetActive(false);
            mainMenuCanvas.SetActive(true);
            EventSystem.current.SetSelectedGameObject(firstSelectedObjectMainMenu);
        }

        public void LoadLevel(string levelName)
        {
            SceneManager.LoadScene(levelName);
        }

        public void Exit()
        {
            GameManager.Exit();
        }
    }
}