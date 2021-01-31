using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedController : MonoBehaviour
{
    //call uui after trigger
    //give text messsage that they can do this

    public ShipController ship;


    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerController>() != null)
           GameUI.Instance.UpdateStatus("Press Up and Down to Set Sail Height");
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
            ship.HandleSail();  
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
            GameUI.Instance.ClearStatus();
    }
}
