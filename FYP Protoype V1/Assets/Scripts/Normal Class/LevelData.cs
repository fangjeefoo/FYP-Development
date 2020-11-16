﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelData
{
    public int level;
    public int[] exercisePerformedTimes;
    public int score;

    public LevelData(int level, int[] performedTimes, int score)
    {
        exercisePerformedTimes = new int[4];

        this.level = level;
        exercisePerformedTimes = performedTimes;
        this.score = score;
    }
}