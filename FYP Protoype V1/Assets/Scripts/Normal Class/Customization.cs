using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customization
{
    public int exerciseDurationPerLevel; //in minutes
    public int exerciseTime; //need to perform how many times
    public float volume;
    public bool[] exercise;

    public Customization()
    {

    }

    public Customization(int ed, bool[] exercise, int et, float vol)
    {
        this.exercise = new bool[4];
        this.exercise = exercise;
        exerciseDurationPerLevel = ed;
        exerciseTime = et;
        volume = vol;
    }
}
