using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PerformanceData
{
    public LevelData[] level;
    public int totalDuration;
    public bool[] selectedExercise;
    public DateTime dateTime;

    public PerformanceData(LevelData[] level, int totalDuration, bool[] selectedExercise)
    {
        this.selectedExercise = new bool[4];
        this.level = level;
        this.totalDuration = totalDuration;
        this.selectedExercise = selectedExercise;

        dateTime = DateTime.Now;
    }
}
