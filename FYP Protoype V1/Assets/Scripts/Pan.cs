using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        MyHand.handManager.UpdatePanObject(true);
    }

    public void OnCollisionExit(Collision collision)
    {
        MyHand.handManager.UpdatePanObject(false);
    }
}
