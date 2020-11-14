using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LevelData
{
    public int level;
    public int[] exercisePerformedTimes;
    public int score;
    public int levelDuration;

    public LevelData(int level, int[] performedTimes, int score, int levelDuration)
    {
        exercisePerformedTimes = new int[4];

        this.level = level;
        this.exercisePerformedTimes = performedTimes;
        this.score = score;
        this.levelDuration = levelDuration;
    }
}