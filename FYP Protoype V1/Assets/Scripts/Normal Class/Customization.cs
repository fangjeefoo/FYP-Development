using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customization
{
    public int exerciseDuration; //in minutes
    public string selectedExercise;
    public int exerciseTime; //need to perform how many times
    public float volume;

    public Customization(int ed, string se, int et, float vol)
    {
        exerciseDuration = ed;
        selectedExercise = se;
        exerciseTime = et;
        volume = vol;
    }
}
