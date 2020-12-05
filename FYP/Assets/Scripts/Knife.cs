using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

public class Knife : MonoBehaviour
{
    private Vector3 _oriPos;
    private bool _called;

    public void Start()
    {
        _oriPos = transform.position;
        _called = false;
    }

    public void Update()
    {        
        if(GameManager.gm.SelectedExercise != null && !_called)
        {
            _called = true;
            if (!GameManager.gm.SelectedExercise[2])
                Destroy(this.gameObject.GetComponent<InteractionBehaviour>());                
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Hand" && this.gameObject.layer == 9)
        {
            GameManager.gm.UpdatePerformedTimes(0);
            //MyHand.handManager.HoldingKnife = true;
        }        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //MyHand.handManager.HoldingKnife = false;
        transform.position = _oriPos;
    }
}
