using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepPan : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        if (MyHand.handManager.UpdateWristExercise)
        {
            Destroy(collision.gameObject);
            return;
        }

        MyHand.handManager.UpdateWristExercise = true;
    }

    public void OnCollisionExit(Collision collision)
    {
        MyHand.handManager.UpdateWristExercise = false;
    }
}
