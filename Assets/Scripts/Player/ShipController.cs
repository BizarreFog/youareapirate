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

    [HideInInspector]
    public Vector3 lastPos;
    [HideInInspector]
    public Vector3 lastMoved;

    private Vector3 direction;
    private float turnAmount = 0f;

    [HideInInspector]
    public float currentTurn = 0f;

    public float turnSpeed = 15f;
    public float turnLerpFactor = 0.25f;
    public float sailLerpFactor = 0.25f;
    public float dampingFactor = 1f;

    public SkinnedMeshRenderer sail;
    public float speedAmount = 0;
    private float currentSailLength = 0;

    public bool anchorDropped = false;
    public Quaternion anchorRotation;
    private Quaternion originalAnchorRot;
    public bool raisingAnchor = false;

    public float anchorRaiseTime = 5f;
    private float currentAnchorRaise = 0f;

    public GameObject anchorObject;


    void Start()
    {
        rb = GetComponent<Rigidbody>();

        lastPos = transform.position;

        shipSpeed = maxShipSpeed - 10;
        speedAmount = maxShipSpeed - 10;
        currentSailLength = (speedAmount / maxShipSpeed);

        originalAnchorRot = anchorObject.transform.localRotation;

        HandleSail();

        DropAnchor();
    }

    void FixedUpdate()
    {
       
        Movement();

    }


    void Update()
    {
        HandleAnchor();
    }

    public void DropAnchor()
    {
        anchorDropped = true;

        anchorObject.transform.DOKill();
        anchorObject.transform.DORotate(Vector3.zero, .5f, RotateMode.FastBeyond360);
    }

    void HandleAnchor()
    {

        if (currentAnchorRaise >= anchorRaiseTime)
        {
            anchorDropped = false;
            currentAnchorRaise = 0;
        }

        if (raisingAnchor)
        {
            currentAnchorRaise += Time.deltaTime;

            //anchorObject.transform.localRotation = Quaternion.Lerp(anchorObject.transform.localRotation, anchorRotation, .1f);

            anchorObject.transform.DOKill();
            anchorObject.transform.DORotate(new Vector3 (0, 700,0), anchorRaiseTime, RotateMode.FastBeyond360);
        }
        if (!anchorDropped)
        {
            anchorObject.transform.DOKill();
        }

        //if raising anchor lerp rotate to preset vector at slow time
        //if anything else go back to original at very rapid rate

    }

    public void CancelAnchor()
    {
        raisingAnchor = false;
        anchorDropped = true;
        currentAnchorRaise = 0;

        anchorObject.transform.DOKill();
        anchorObject.transform.DORotate(Quaternion.identity.eulerAngles, .5f, RotateMode.FastBeyond360);

    }

    void Movement()
    {
        Debug.Log("WWEEE!!!");

        //Vector3 currentPos = this.transform.position;


        //transform.Translate(transform.forward * shipSpeed * Time.deltaTime);


        direction = transform.up.normalized * turnAmount;
        lastMoved = transform.position - lastPos;

        if (!anchorDropped)
        {
            rb.AddForce(this.transform.forward.normalized * shipSpeed * Time.deltaTime);
        }
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

    public void HandleSail()
    {

        speedAmount = Mathf.Clamp(speedAmount, 10f, maxShipSpeed);

        if (Input.GetKey(KeyCode.DownArrow))
        {
            speedAmount -= 1000 * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            speedAmount += 1000 * Time.deltaTime;
        }

        float speedPercent = (speedAmount / maxShipSpeed);

        Debug.Log(speedPercent);

        currentSailLength = Mathf.Lerp(currentSailLength, speedPercent, sailLerpFactor);
        currentSailLength = Mathf.Clamp(currentSailLength, 0, 1);

        
        shipSpeed = maxShipSpeed * Mathf.Abs(1 - speedPercent);

        sail.SetBlendShapeWeight(0, speedPercent * 100);
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
