using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class ShipController : MonoBehaviour
{

    private Rigidbody rb;

    public float shipSpeed = 3f;

    public Vector3 lastPos;

    public Vector3 lastMoved;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        lastPos = transform.position;
    }

    void FixedUpdate()
    {
       

        Movement();

      
    }

    void Movement()
    {
        Debug.Log("WWEEE!!!");

        //Vector3 currentPos = this.transform.position;

        //transform.Translate(transform.forward * shipSpeed * Time.deltaTime);
        lastMoved = transform.position - lastPos;
        rb.AddForce(this.transform.forward.normalized * shipSpeed * Time.deltaTime);
        lastPos = this.transform.position;

        //HelperUtilities.WaitForFrameAndExecute();

        //Physics.Simulate(1);
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag != "Environment")
        {
            other.transform.SetParent(this.transform);

        }
    }

    void OnTriggerExit(Collider other)
    {


        if (other.tag != "Environment")
        {
            other.transform.SetParent(null);
        }
    }

}
