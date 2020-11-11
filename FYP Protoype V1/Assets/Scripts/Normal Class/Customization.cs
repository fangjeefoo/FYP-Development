using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customization
{
    public int exerciseDuration; //in minutes
    public int exerciseTime; //need to perform how many times
    public float volume;
    public bool[] exercise;

    public Customization()
    {

    }

    //public Customization(int ed, bool e1, bool e2, bool e3, bool e4, int et, float vol)
    //{
    //    exercise = new bool[4];
    //    exerciseDuration = ed;
    //    exercise1 = e1;
    //    exercise2 = e2;
    //    exercise3 = e3;
    //    exercise4 = e4;
    //    exerciseTime = et;
    //    volume = vol;
    //}

    public Customization(int ed, bool[] exercise, int et, float vol)
    {
        this.exercise = new bool[4];
        this.exercise = exercise;
        exerciseDuration = ed;
        exerciseTime = et;
        volume = vol;
    }
}
