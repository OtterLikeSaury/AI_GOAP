using System;
using UnityEngine;

public sealed class CountDownTimer
{

    public bool IsAutoCycle { get; private set; }                   
    public bool IsStoped { get; private set; }                      
    public float CurrentTime { get { return UpdateCurrentTime(); } }
    public bool IsTimeUp { get { return CurrentTime <= 0; } }      
    public float Duration { get; private set; }                    

    private float lastTime;                                        
    private int lastUpdateFrame;                                    
    private float currentTime;                                      

    public CountDownTimer(float duration, bool autocycle = false, bool autoStart = true)
    {
        IsStoped = true;
        Duration = Mathf.Max(0f, duration);
        IsAutoCycle = autocycle;
        Reset(duration, !autoStart);
    }

    public event Action OnTimerStop;

    private float UpdateCurrentTime()
    {
        if (IsStoped || lastUpdateFrame == Time.frameCount)         
            return currentTime;
        if (currentTime <= 0)                                       
        {
            if (IsAutoCycle)
                Reset(Duration, false);
            return currentTime;
        }
        currentTime -= Time.time - lastTime;
        UpdateLastTimeInfo();
        return currentTime;
    }

    private void UpdateLastTimeInfo()
    {
        lastTime = Time.time;
        lastUpdateFrame = Time.frameCount;
    }

    public void Start()
    {
        Reset(Duration, false);
    }

    public void Reset(float duration, bool isStoped = false)
    {
        UpdateLastTimeInfo();
        Duration = Mathf.Max(0f, duration);
        currentTime = Duration;
        IsStoped = isStoped;
    }

    public void Pause()
    {
        UpdateCurrentTime();   
        IsStoped = true;
    }

    public void Continue()
    {
        UpdateLastTimeInfo();
        IsStoped = false;
    }

    public void End()
    {
        IsStoped = true;
        currentTime = 0f;
        OnTimerStop?.Invoke();
    }

    public float GetPercent()
    {
        UpdateCurrentTime();
        if (currentTime <= 0 || Duration <= 0)
            return 1f;
        return 1f - currentTime / Duration;
    }
}