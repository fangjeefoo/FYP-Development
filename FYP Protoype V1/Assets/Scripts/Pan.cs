using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision enter");
    }

    public void OnCollisionExit(Collision collision)
    {
        Debug.Log("Collision exit");
    }
}
