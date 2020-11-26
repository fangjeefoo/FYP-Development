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
        _maxFood = 1;
        _currentFood = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(_currentFood < _maxFood)
            Instantiate(food, foodParent);

    }

    public void OnCollisionEnter(Collision collision)
    {
        _currentFood++;

        if (_currentFood > _maxFood)
        {
            _currentFood--;
            Destroy(collision.gameObject);
        }            
    }

    public void OnCollisionExit(Collision collision)
    {
        _currentFood--;
    }
}
