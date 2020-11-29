using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : MonoBehaviour
{
    public static Pan pan;

    public GameObject food;

    public void Start()
    {
        pan = this;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (MyHand.handManager.UpdateForearmExercise)
        {
            Destroy(collision.gameObject);
            return;
        }

        MyHand.handManager.UpdateForearmExercise = true;
        food = collision.gameObject;
    }

    public void OnCollisionExit(Collision collision)
    {
        food = null;
        MyHand.handManager.UpdateForearmExercise = false;
        MyHand.handManager.ResetCounter(1);
    }
}
