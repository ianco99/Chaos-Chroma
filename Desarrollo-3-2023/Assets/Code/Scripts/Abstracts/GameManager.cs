using UnityEngine.Device;
using UnityEngine.SceneManagement;

namespace Code.Scripts.Abstracts
{
    public class GameManager
    {
        private static GameManager instance;

        private bool won;
        private string lostLevel = "Tutorial";

        private static GameManager Instance => instance ??= new GameManager();

        public static bool Won => Instance.won;

        /// <summary>
        /// Set the conditions of level complete
        /// </summary>
        public static void Win()
        {
            Instance.won = true;
            Instance.lostLevel = "Tutorial";
            SceneManager.LoadScene("WinLoseScreen");
        }

        /// <summary>
        /// Load level 1
        /// </summary>
        public static void LoadLevel1()
        {
            SceneManager.LoadScene("Level1");
        }

        /// <summary>
        /// Set the conditions of level lost
        /// </summary>
        public static void Lose()
        {
            Instance.won = false;
            Instance.lostLevel = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("WinLoseScreen");
        }

        /// <summary>
        /// Load tutorial level
        /// </summary>
        public static void StartTutorial()
        {
            SceneManager.LoadScene("Tutorial");
        }
        
        /// <summary>
        /// Reload failed level
        /// </summary>
        public static void RetryLevel()
        {
            SceneManager.LoadScene(Instance.lostLevel);
        }

        /// <summary>
        /// Load menu
        /// </summary>
        public static void ReturnToMenu()
        {
            SceneManager.LoadScene("Menu");
        }

        /// <summary>
        /// Exit game
        /// </summary>
        public static void Exit()
        {
            Application.Quit();
        }
    }
}