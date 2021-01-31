using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnchorController : MonoBehaviour
{
    public ShipController ship;
    public Text anchorStatus;

    public bool inRange = false;

    
    void Start()
    {
        if (ship.anchorDropped)
        {
            anchorStatus.text = "Anchor: Dropped";
        }
        else
        {
            anchorStatus.text = "Anchor: Raised";
        }
    }

    void Update()
    {
        if (inRange)
        {
            if (ship.anchorDropped)
            {
                GameUI.Instance.UpdateStatus("Hold E to Raise Anchor");

                if (Input.GetKeyDown(KeyCode.E))
                {
                    ship.raisingAnchor = true;
                }

                if (Input.GetKeyUp(KeyCode.E))
                {
                    ship.CancelAnchor();
                    return;
                }
            }
            else
            {

                GameUI.Instance.UpdateStatus("Press E to Drop Anchor");

                if (Input.GetKeyDown(KeyCode.E))
                {
                    ship.DropAnchor();
                    return;
                }
            }
        }
        
    }

    void OnTriggerStay(Collider other)
    {
        if (ship.anchorDropped)
        {
            anchorStatus.text = "Anchor: Dropped";
        }
        else
        {
            anchorStatus.text = "Anchor: Raised";
        }

        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            inRange = true;
        }

      
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {

            GameUI.Instance.ClearStatus();

            inRange = false;

        }
            
    }
}
