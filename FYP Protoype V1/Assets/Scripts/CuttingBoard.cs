using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        MyHand.handManager.UpdateCuttingBoardObject(true);
    }

    public void OnCollisionExit(Collision collision)
    {
        MyHand.handManager.UpdateCuttingBoardObject(false);
    }
}
