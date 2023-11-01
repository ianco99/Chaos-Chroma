using Code.Scripts.Abstracts;
using UnityEngine;

namespace Code.Scripts.Menus
{
    public class MainMenu : MonoBehaviour
    {
        public void StartGame()
        {
            GameManager.StartGame();
        }
    
        public void Exit()
        {
            GameManager.Exit();
        }
    }
}