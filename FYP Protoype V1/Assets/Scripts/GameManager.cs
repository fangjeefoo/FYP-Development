using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    //public variable
    public GameObject customer;
    public int maxCustomer = 5;

    //private variable
    private float counter = 10;
    private int currentCustomer = 0;

    void Awake()
    {
        gm = this.GetComponent<GameManager>();
    }

    void Update()
    {
        counter += Time.deltaTime;

        if(counter > 10) //more than 10 seconds instantiate customer
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
