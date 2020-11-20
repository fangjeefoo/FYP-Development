using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Hand" && this.gameObject.layer == 9)
        {
            Debug.Log("grab and release");
            //0 = fist exercise
            GameManager.gm.UpdatePerformedTimes(0);
        }            
    }
}
