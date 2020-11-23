using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepPan : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        MyHand.handManager.UpdateDeepPanObject(true);
    }

    public void OnCollisionExit(Collision collision)
    {
        MyHand.handManager.UpdateDeepPanObject(false);
    }
}
