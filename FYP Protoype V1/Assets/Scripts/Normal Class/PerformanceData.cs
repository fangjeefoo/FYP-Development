using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PerformanceData
{
    public int durationPerLevel;
    public bool[] selectedExercise;
    public string dateTime;

    public PerformanceData(int duration, bool[] selectedExercise)
    {
        this.selectedExercise = new bool[4];
        durationPerLevel = duration;
        this.selectedExercise = selectedExercise;

        dateTime = DateTime.Now.ToString();
    }
}
