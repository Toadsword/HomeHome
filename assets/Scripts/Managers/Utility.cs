using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility{
    public static float StartTimer(float deltaTime)
    {
        return Time.time + deltaTime;
    }

    // Return "true" if the timer has passed the time
    public static bool IsOver(float timerToCheck)
    {
        return timerToCheck <= Time.time;
    }

    public static float GetTimerRemainingTime(float timer)
    {
        return timer - Time.time;
    }

    public static float ResetTimer()
    {
        return Time.time;
    }
}
