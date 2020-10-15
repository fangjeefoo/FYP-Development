using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    public GameObject customer;

    //private variable
    private float counter = 0;
    private int currentCustomer = 0;
    private int maxCustomer = 5;

    void Awake()
    {
        gm = this.GetComponent<GameManager>();
    }

    void Update()
    {
        counter += Time.deltaTime;

        if(counter > 2) //more than 2 seconds instantiate customer
        {
            if(currentCustomer < maxCustomer)
            {
                Instantiate(customer);
                currentCustomer++;
            }            
            counter = 0f;
        }
    }
}
