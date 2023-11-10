using Code.Scripts.Abstracts;
using Code.Scripts.Menus;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event music;
    [SerializeField] AK.Wwise.State menu;
    [SerializeField] AK.Wwise.State combate;
    
     
     bool menuActivo = false;
     bool winActivo = false;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        music.Post(gameObject);
        menu.SetValue();
        //menuActivo = true;
       // winActivo = false;

    }

    private void Update()
    {
       
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            //&& menuActivo == false
            combate.SetValue();
            //menuActivo= false;
        }

        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            //&& menuActivo == false
            combate.SetValue();
           // menuActivo = false;
        }

        if (SceneManager.GetActiveScene().name == "WinLoseScreen" )
        {
            //&& winActivo == false
            menu.SetValue();
            //winActivo = true;
        }



    }

}

