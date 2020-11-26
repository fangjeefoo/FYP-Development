using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPlate : MonoBehaviour
{
    [Tooltip("Food to generate")]
    public GameObject food;
    public Transform foodParent;

    private int _maxFood;
    private int _currentFood;


    void Start()
    {
        _maxFood = 3;
        _currentFood = 0;

        for(int i = _currentFood; i < _maxFood; i++)
            Instantiate(food, foodParent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionExit(Collision collision)
    {
        for (int i = _currentFood; i < _maxFood; i++)
            Instantiate(food, foodParent);
    }
}
