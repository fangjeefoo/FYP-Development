using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        if (MyHand.handManager.UpdateElbowExercise)
        {
            Destroy(collision.gameObject);
            return;
        }

        MyHand.handManager.UpdateElbowExercise = true;
    }

    public void OnCollisionExit(Collision collision)
    {
        MyHand.handManager.UpdateElbowExercise = false;
    }
}
