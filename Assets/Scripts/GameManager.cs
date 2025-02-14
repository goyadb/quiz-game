using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector]
    public int heartCount;

    private void Start()
    {
        heartCount = 3;
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
}
