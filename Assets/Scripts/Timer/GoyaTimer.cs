using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoyaTimer : MonoBehaviour
{
    [Serializable]
    public class FillSettings
    {
        public Color color;
    }
    public FillSettings fillSettings;

    [Serializable]
    public class BackgroundSettings
    {
        public Color color;
    }
    public BackgroundSettings backgroundSettings;

    [SerializeField] private Image fillImage;
    [SerializeField] private float totalTime;
    
    public float CurrentTime { get; private set; }
    private bool _isPaused;

    private void Update()
    {
        if (!_isPaused)
        {
            CurrentTime += Time.deltaTime;
            fillImage.fillAmount = CurrentTime / totalTime;
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

    public void RessetTimer()
    {
        CurrentTime = 0;
        fillImage.fillAmount = 1;
    }
}
