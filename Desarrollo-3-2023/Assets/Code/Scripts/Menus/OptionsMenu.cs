using Code.Scripts.Game;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Scripts.Menus
{
    public class OptionsMenu : MonoBehaviour
    {
        /// <summary>
        /// Resume the game
        /// </summary>
        public void Resume()
        {
            Time.timeScale = 1f;
        }

        /// <summary>
        /// Return to the menu
        /// </summary>
        public void Quit()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Menu");
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

        private void OnDestroy()
        {
            Time.timeScale = 1f;
        }
    }
}