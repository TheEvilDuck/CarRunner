using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField]private CarBehaviour _carBehavior;
    //view машинки

    public void InitCar(CarConfig Config)
    {
        _carBehavior.Init(Config.Acceleration, Config.MaxSpeed);
    }
}
