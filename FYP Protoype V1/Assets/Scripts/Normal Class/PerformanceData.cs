using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PerformanceData
{
    public int totalDuration;
    public bool[] selectedExercise;
    public string dateTime;

    public PerformanceData(int totalDuration, bool[] selectedExercise)
    {
        this.selectedExercise = new bool[4];    
        this.totalDuration = totalDuration;
        this.selectedExercise = selectedExercise;

        dateTime = DateTime.Now.ToString();
    }
}
