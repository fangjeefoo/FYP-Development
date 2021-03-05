using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customization
{
    public int exerciseDurationPerLevel; //in minutes
    public float volume;
    public bool[] exercise;

    public Customization(int ed, bool[] exercise, float vol)
    {
        this.exercise = new bool[4];
        this.exercise = exercise;
        exerciseDurationPerLevel = ed;
        volume = vol;
    }
}

