using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelData
{
    public int level;
    public int[] exercisePerformedTimes;
    public string score;

    public LevelData(int level, int[] performedTimes, string score)
    {
        exercisePerformedTimes = new int[4];

        this.level = level;
        exercisePerformedTimes = performedTimes;
        this.score = score;
    }
}