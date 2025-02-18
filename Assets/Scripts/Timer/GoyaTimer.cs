using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class GoyaTimer : MonoBehaviour
{
    // [Serializable]
    // public class FillSettings
    // {
    //     public Color color;
    // }
    // public FillSettings fillSettings;
    //
    // [Serializable]
    // public class BackgroundSettings
    // {
    //     public Color color;
    // }
    // public BackgroundSettings backgroundSettings;

    [SerializeField] private Image fillImage;
    [SerializeField] private float totalTime;
    [SerializeField] private Image headCapImage;
    [SerializeField] private Image tailCapImage;
    [SerializeField] private TMP_Text timeText;
    
    
    
    public float CurrentTime { get; private set; }
    private bool _isPaused;


    public delegate void GoyaTimerDelegate();

    public GoyaTimerDelegate OnTimeout;
    //여기서 설정하고
    
    private void Awake()
    {
        _isPaused = true;

    }
    
    private void Update()
    {
        if (!_isPaused)
        {
            CurrentTime += Time.deltaTime;

            if (CurrentTime >= totalTime)
            {
                headCapImage.gameObject.SetActive(false);
                tailCapImage.gameObject.SetActive(false);
                _isPaused = true;
                OnTimeout?.Invoke();//여기서 실행
                
                //GameOver();
            }
            else
            {
                fillImage.fillAmount = (totalTime-CurrentTime) / totalTime;
                headCapImage.transform.localRotation = Quaternion.Euler(new Vector3(0,0, fillImage.fillAmount*360));
                
                var timeTextTime = totalTime - CurrentTime;
                timeText.text = timeTextTime.ToString("F0");
                
            }
            
            
        }
    }

   
    

    public void StartTimer()
    {
        _isPaused = false;
    }

    public void PauseTimer()
    {
        _isPaused = true;
    }

    public void InitTimer()
    {
        CurrentTime = 0;
        fillImage.fillAmount = 1;
        timeText.text = totalTime.ToString("F0");
        
        headCapImage.gameObject.SetActive(true);
        tailCapImage.gameObject.SetActive(true);
        _isPaused = true;
    }
}
