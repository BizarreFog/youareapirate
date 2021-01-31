using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SteeringWheelHandler : MonoBehaviour
{
    public ShipController ship;

    public Text debug;

    void Update()
    {
        RotateWheel();

    }

    void RotateWheel()
    {
        //set z value directly
        transform.localRotation = Quaternion.Euler(0, 0, -ship.currentTurn);
        debug.text = ship.currentTurn.ToString();
    }


}
