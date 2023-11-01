using UnityEngine.SceneManagement;

namespace Code.Scripts.Abstracts
{
    public class GameManager
    {
        private static GameManager instance;

        private bool won;

        private static GameManager Instance => instance ??= new GameManager();

        public static bool Won => Instance.won;

        public static void Win()
        {
            Instance.won = true;
            SceneManager.LoadScene("WinLoseScreen");
        }

        public static void Lose()
        {
            Instance.won = false;
            SceneManager.LoadScene("WinLoseScreen");
        }

        public static void StartGame()
        {
            SceneManager.LoadScene("Tutorial");
        }

        public static void ReturnToMenu()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}