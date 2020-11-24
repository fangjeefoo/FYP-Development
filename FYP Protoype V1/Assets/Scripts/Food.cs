using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private bool _isCooked;

    public void Start()
    {
        _isCooked = false;
    }

    public bool Cook
    {
        set { _isCooked = value; }
        get { return _isCooked; }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Hand" && this.gameObject.layer == 9)
        {
            Debug.Log("grab and release");
            GameManager.gm.UpdatePerformedTimes(0);
        }            
    }
}
