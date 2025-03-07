using Code.Scripts.Abstracts;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Code.Scripts.Menus
{
    /// <summary>
    /// Controls the main menu
    /// </summary>
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

        /// <summary>
        /// Shows the credits menu and hides the main menu.
        /// </summary>
        public void ShowCredits()
        {
            creditsMenuCanvas.SetActive(true);
            mainMenuCanvas.SetActive(false);
            EventSystem.current.SetSelectedGameObject(firstSelectedObjectCredits);
        }

        /// <summary>
        /// Hides the credits menu and shows the main menu.
        /// </summary>
        public void HideCredits()
        {
            creditsMenuCanvas.SetActive(false);
            mainMenuCanvas.SetActive(true);
            EventSystem.current.SetSelectedGameObject(firstSelectedObjectMainMenu);
        }

        /// <summary>
        /// Hides the level select canvas and shows the main menu.
        /// Sets the first selected object to the main menu's first selected object.
        /// </summary>
        public void BackToMainMenu()
        {
            levelSelectCanvas.SetActive(false);
            mainMenuCanvas.SetActive(true);
            EventSystem.current.SetSelectedGameObject(firstSelectedObjectMainMenu);
        }

        /// <summary>
        /// Loads the specified level by its name.
        /// </summary>
        /// <param name="levelName">The name of the level to load.</param>
        public void LoadLevel(string levelName)
        {
            SceneManager.LoadScene(levelName);
        }

        /// <summary>
        /// Exits the application.
        /// </summary>
        public void Exit()
        {
            GameManager.Exit();
        }
    }
}