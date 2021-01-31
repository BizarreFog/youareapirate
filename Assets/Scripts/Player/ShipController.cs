using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class ShipController : MonoBehaviour
{

    private Rigidbody rb;

    public float shipSpeed = 3f;

    public float maxShipSpeed = 5f;

    public Vector3 lastPos;

    public Vector3 lastMoved;

    private Vector3 direction;
    private float turnAmount = 0f;
    [HideInInspector]
    public float currentTurn = 0f;
    public float turnSpeed = 15f;
    public float turnLerpFactor = 0.25f;
    public float dampingFactor = 1f;


    void Start()
    {
        rb = GetComponent<Rigidbody>();

        lastPos = transform.position;
    }

    void FixedUpdate()
    {
       

        Movement();

      
    }


    void Update()
    {
        HandleSteering();
    }

    void Movement()
    {
        Debug.Log("WWEEE!!!");

        //Vector3 currentPos = this.transform.position;


        //transform.Translate(transform.forward * shipSpeed * Time.deltaTime);


        direction = transform.up.normalized * turnAmount;
        lastMoved = transform.position - lastPos;
        rb.AddForce(this.transform.forward.normalized * shipSpeed * Time.deltaTime);
        lastPos = this.transform.position;

        Quaternion q = Quaternion.AngleAxis((currentTurn / dampingFactor) * turnSpeed, transform.up) * transform.rotation;

        rb.MoveRotation(q);


        if(rb.velocity.magnitude > maxShipSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxShipSpeed);
        }

        //HelperUtilities.WaitForFrameAndExecute();

        //Physics.Simulate(1);
    }

    public void HandleSteering()
    {

        turnAmount = Mathf.Clamp(turnAmount, -180, 180);

        if (Input.GetKey(KeyCode.LeftArrow))
        {
           turnAmount--;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
           turnAmount++;
        }

        currentTurn = Mathf.Lerp(currentTurn, turnAmount, turnLerpFactor);
        currentTurn = Mathf.Clamp(currentTurn, -270, 270);
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
