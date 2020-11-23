using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    private Vector3 _oriPos;

    public void Start()
    {
        _oriPos = transform.position;
    }

    public void OnCollisionEnter(Collision collision)
    {
        MyHand.handManager.HoldingKnife = true;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        MyHand.handManager.HoldingKnife = false;
        transform.position = _oriPos;
    }
}
