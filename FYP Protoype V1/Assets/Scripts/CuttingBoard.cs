using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : MonoBehaviour
{
    public static CuttingBoard cuttingBoard;

    public GameObject food;

    public void Start()
    {
        cuttingBoard = this;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (MyHand.handManager.UpdateElbowExercise)
        {
            Destroy(collision.gameObject);
            return;
        }

        MyHand.handManager.UpdateElbowExercise = true;
        food = collision.gameObject;
    }

    public void OnCollisionExit(Collision collision)
    {
        food = null;
        MyHand.handManager.UpdateElbowExercise = false;
        MyHand.handManager.ResetCounter(2);
    }
}
