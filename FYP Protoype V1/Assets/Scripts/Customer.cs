using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Customer : MonoBehaviour
{
    //public variable
    public static GameObject[] chair;
    public static GameObject door; 

    //private variable
    private int chairCounter;
    private float finishMealCounter;
    private bool isEating;


    void Update()
    {
        //if is eating false, means the customer just reach the restaurant
        if (!isEating)
        {
            chairCounter = int.MaxValue;

            //check the chair is available
            for (int i = 0; i < chair.Length; i++)
            {
                if (!chair[i].GetComponent<Chair>().occupied)
                {
                    chair[i].GetComponent<Chair>().occupied = true;
                    chairCounter = i;
                    break;
                }
            }

            //not enough chair
            if (chairCounter == int.MaxValue)
            {
                //go out the restaurant
            }
            else
            {
                //move towwards the chair
            }
        }       
    }

    //activate this function when player place the food in front of player
    //take time to finish meal and leave
    IEnumerator FinishMeal()
    {
        //do nothing
        yield return new WaitForSeconds(finishMealCounter); 
        //leave the shop

    }
}
