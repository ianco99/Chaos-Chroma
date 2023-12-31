using System;
using Code.Scripts.Abstracts;
using Code.Scripts.Game;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Scripts.Menus
{
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

        private void OnDestroy()
        {
            Time.timeScale = 1f;
        }
    }
}