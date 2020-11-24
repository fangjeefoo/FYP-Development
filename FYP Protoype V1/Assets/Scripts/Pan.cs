using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        if (MyHand.handManager.UpdateForearmExercise)
        {
            Destroy(collision.gameObject);
            return;
        }

        MyHand.handManager.UpdateForearmExercise = true;
    }

    public void OnCollisionExit(Collision collision)
    {
        MyHand.handManager.UpdateForearmExercise = false;
    }
}
