using AK.Wwise;
using UnityEngine;
using UnityEngine.SceneManagement;
using Event = AK.Wwise.Event;

namespace Code.Scripts.Music
{
    public class MusicController : MonoBehaviour
    {
        [SerializeField] private Event music;
        [SerializeField] private State menu;
        [SerializeField] private State combate;

        private void Start()
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "Level1":
                case "Level2":
                case "Tutorial":
                    combate.SetValue();
                    break;
                case "WinLoseScreen":
                    menu.SetValue();
                    break;
            }

            music.Post(gameObject);
            menu.SetValue();
        }
    }
}

