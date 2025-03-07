using Code.Scripts.Abstracts;
using Code.Scripts.Game;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Scripts.Menus
{
    /// <summary>
    /// Controls the options menu
    /// </summary>
    public class OptionsMenu : MonoBehaviour
    {
        private void Start()
        {
            GameManager.LostLevel = SceneManager.GetActiveScene().name;
        }

        /// <summary>
        /// Resume the game
        /// </summary>
        public void Resume()
        {
            GameManager.Pause();
        }

        /// <summary>
        /// Return to the menu
        /// </summary>
        public void Quit()
        {
            GameManager.ReturnToMenu();
        }

        /// <summary>
        /// Update de value of the game settings volume
        /// </summary>
        /// <param name="volume">New volume</param>
        public void UpdateSound(float volume)
        {
            GameSettings.Instance.volume = volume;
        }

        /// <summary>
        /// Update de value of the game settings music volume
        /// </summary>
        /// <param name="volume">New volume</param>
        public void UpdateMusic(float volume)
        {
            GameSettings.Instance.music = volume;
        }
        
        /// <summary>
        /// Reload current level
        /// </summary>
        public void RestartLevel()
        {
            GameManager.RetryLevel();
        }

        /// <summary>
        /// Set the time scale to 1f when the component is destroyed.
        /// This is used to prevent the game from being paused when the options menu is closed.
        /// </summary>
        private void OnDestroy()
        {
            Time.timeScale = 1f;
        }
    }
}