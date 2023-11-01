using Code.Scripts.Abstracts;
using UnityEngine;

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