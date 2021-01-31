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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
            GameUI.Instance.UpdateStatus("Press Left and Right to Steer");
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
            ship.HandleSteering();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
            GameUI.Instance.ClearStatus();
    }


}
