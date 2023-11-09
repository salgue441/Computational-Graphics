using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static Action OnMinuteChanged;
    public static Action OnHourChanged;

    public static int Minute { get; private set; }
    public static int Hour { get; private set; }

    private float minuteToRealTime = 0.5f;
    private float timer;

    /// <summary>
    /// Sets the properties of the object, and starts the timer.
    /// </summary>
    private void Start()
    {
        Minute = 0;
        Hour = 0;

        timer = minuteToRealTime;
    }

    /// <summary>
    /// Updates the timer, and invokes the OnMinuteChanged and 
    /// OnHourChanged events.
    /// </summary>
    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Minute++;
            OnMinuteChanged?.Invoke();

            if (Minute >= 60)
            {
                Minute = 0;
                Hour++;
                OnHourChanged?.Invoke();

                if (Hour >= 24)
                {
                    Hour = 0;
                }
            }

            timer = minuteToRealTime;
        }
    }
}
