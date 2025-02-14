using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector] public int heartCount;

    private void Start()
    {
<<<<<<< HEAD
        heartCount = UserInformation.HeartCount;
=======
        heartCount = UserInformations.HeartCount;
>>>>>>> upstream/main
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
    
    public void QuitGame()
    {
        SceneManager.LoadScene("Main");
    }
    
    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
    }

    private void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit!!");
        UserInformations.HeartCount = heartCount;
    }

    private void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
        UserInformation.HeartCount = heartCount;
    }
}
