using UnityEngine;
using UnityEngine.SceneManagement;
using Application = UnityEngine.Device.Application;

namespace Code.Scripts.Abstracts
{
    public class GameManager
    {
        private static GameManager instance;

        private bool won;
        private bool paused;
        private string lostLevel = "Tutorial";

        private static GameManager Instance => instance ??= new GameManager();

        public static string LostLevel
        {
            set => Instance.lostLevel = value;
        }

        public static bool Won => Instance.won;

        /// <summary>
        /// Set the conditions of level complete
        /// </summary>
        public static void Win()
        {
            Instance.won = true;
            LostLevel = "Tutorial";
            SceneManager.LoadScene("WinLoseScreen");
        }

        /// <summary>
        /// Load level 1
        /// </summary>
        public static void LoadLevel1()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Level1");
        }

        /// <summary>
        /// Set the conditions of level lost
        /// </summary>
        public static void Lose()
        {
            Time.timeScale = 1f;
            Instance.won = false;
            LostLevel = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("WinLoseScreen");
        }

        /// <summary>
        /// Load tutorial level
        /// </summary>
        public static void StartTutorial()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Tutorial");
        }

        /// <summary>
        /// Reload failed level
        /// </summary>
        public static void RetryLevel()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(Instance.lostLevel);
        }

        /// <summary>
        /// Load menu
        /// </summary>
        public static void ReturnToMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Menu");
        }

        /// <summary>
        /// Exit game
        /// </summary>
        public static void Exit()
        {
            Application.Quit();
        }

        /// <summary>
        /// Pause and unpause the game
        /// </summary>
        /// <returns>true if game is paused else false</returns>
        public static bool Pause()
        {
            Instance.paused = !Instance.paused;
            Time.timeScale = Instance.paused ? 0f : 1f;

            return Instance.paused;
        }
    }
}