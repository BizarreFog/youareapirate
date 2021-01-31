using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{

    private CharacterController pCon;
    private Vector2 rotation = Vector2.zero;
    public float lookSensitivity = 3;
    public float playerSpeed = 3;
    public float playerGravity = -9.8f;
    public float gravityEase = 0.025f;
    public float sprintEase = 0.25f;
    public float FOVEase = .8f;
    public float baseInputEase = 1f;
    public float fallingInputEase = .25f;
    private float currentGravity = -9.8f;
    private float currentSpeed = 3;
    private Vector3 currentMovement = Vector3.zero;
    private bool obstacleAbove;
    private bool sprinting = false;

    private float timeSinceLastLand = 0;

    public float turnSmoothness = .25f;
    float turnSmoothVelocity;

    private Vector3 yVelocity = Vector3.zero;

    public float jumpSpeed = 3;
    public float sprintSpeed = 6;

    public float bobSpeed, bobHeight;

    public float sprintFOVKick = 1.5f; 



    private bool isCrouching = false;
    private float originalHeight, originalCameraY, originalFOV;

    void Start()
    {
        pCon = GetComponent<CharacterController>();
        currentGravity = playerGravity;
        currentSpeed = playerSpeed;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        originalHeight = pCon.height;
        originalCameraY = Camera.main.transform.localPosition.y;
        originalFOV = Camera.main.fieldOfView;
    }

    void FixedUpdate()
    {
        Look();
        Gravity();
        Movement();
    }

    private void Look()
    {
        rotation.y += Input.GetAxis("Mouse X");
        rotation.x += Input.GetAxis("Mouse Y");
        rotation.x = Mathf.Clamp(rotation.x, -30f, 30f);
        transform.eulerAngles = new Vector2(0, rotation.y) * lookSensitivity;
        Camera.main.transform.localRotation = Quaternion.Euler(-rotation.x * lookSensitivity, 0, 0);
    }

    void Gravity()
    {
        yVelocity.y += playerGravity * Time.deltaTime;

        if (pCon.isGrounded && yVelocity.y < 0)
        {
            yVelocity.y = 0f;
        }


    }

    private void Movement()
    {
        // Vector3 movement = new Vector3((Input.GetAxis("Horizontal") * currentSpeed) * Time.deltaTime, 0, (Input.GetAxis("Vertical") * currentSpeed) * Time.deltaTime);
        if (pCon.isGrounded)
        {
            currentMovement = Vector3.Lerp(currentMovement, new Vector3((Input.GetAxis("Horizontal") * currentSpeed) * Time.deltaTime, 0, (Input.GetAxis("Vertical") * currentSpeed) * Time.deltaTime), baseInputEase);
        }
        else
        {
            currentMovement = Vector3.Lerp(currentMovement, new Vector3((Input.GetAxis("Horizontal") * currentSpeed) * Time.deltaTime, 0, (Input.GetAxis("Vertical") * currentSpeed) * Time.deltaTime), fallingInputEase);
        }


        Vector3 direction = (new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) );


        Vector3 move = (direction.x * transform.right + direction.y * transform.forward).normalized;


        pCon.Move((move + yVelocity) * currentSpeed * Time.deltaTime);


        if (this.transform.parent != null)
        {
            ShipController ship = this.transform.parent.GetComponent<ShipController>();

            if(ship != null)
            {
                transform.position += ship.lastMoved;
            }
        }

            /*
            if(this.transform.parent != null)
            {

                //take parent and find out what displacement (parent - child)(gives you direction twoards parent) (distance with direction)
                pCon.Move(((move + yVelocity) * currentSpeed * Time.deltaTime) + ( this.transform.parent.localPosition - this.transform.localPosition));

                //inverse the displacement 

            }
            else
            {
                pCon.Move((move + yVelocity) * currentSpeed * Time.deltaTime);
            }
            */

            Jump();
        Crouch();
        Sprint();
        LandingEffects();
    }

    private void Jump()
    {

        if (Input.GetButtonDown("Jump"))
        {
            RaycastHit hit;
            if ((Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, pCon.height / 2)))
            {
                Debug.Log("hit");
                yVelocity.y += Mathf.Sqrt(jumpSpeed * -3.0f * playerGravity);
            }
        }
        
    }

    private void LandingEffects()
    {

        if (!pCon.isGrounded)
        {
            timeSinceLastLand += Time.deltaTime;
        }

        if(pCon.isGrounded && timeSinceLastLand > 0)
        {
            Camera.main.transform.DOShakePosition(timeSinceLastLand * .25f, new Vector3 (0, .5f * timeSinceLastLand, 0), 10, 90,false, true);
            timeSinceLastLand = 0;
        }

    }


    private void Sprint()
    {

        if (Input.GetButton("Sprint") && !isCrouching && pCon.isGrounded && Input.GetAxis("Vertical") > .1f)
        {
            sprinting = true;
        }
        if (sprinting && Input.GetAxis("Vertical") < .1f)
        {
            sprinting = false;
        }
        if (sprinting && !isCrouching)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, sprintSpeed, sprintEase);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, sprintFOVKick, FOVEase - .1f);

            if (pCon.isGrounded)
            {
                Camera.main.transform.localPosition = new Vector3(0.0f, Mathf.Sin(Time.time * bobSpeed) * bobHeight + originalCameraY, 0.0f);
            }
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, playerSpeed, sprintEase);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, originalFOV, FOVEase);
        }
    }

    private void Crouch()
    {
        if (Input.GetButton("Crouch"))
        {
            isCrouching = true;
        }
        if(isCrouching && !Input.GetButton("Crouch"))
        {
            if (!obstacleAbove)
            {
                isCrouching = false;
            }


            currentSpeed = playerSpeed;
        }

        if (isCrouching && !sprinting)
        {
            if (pCon.isGrounded)
            {
                currentSpeed = playerSpeed / 2;
            }
            pCon.height = (originalHeight / 2) + .50f;
            Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, new Vector3(Camera.main.transform.localPosition.x, (Camera.main.transform.localPosition.y / 2) + .50f, Camera.main.transform.localPosition.z), .25f);
        }
        else
        {

            pCon.height = originalHeight;
            Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, new Vector3(Camera.main.transform.localPosition.x, originalCameraY, Camera.main.transform.localPosition.z), .25f);
        }

        int layerMask = 1 << 9;
        layerMask = ~layerMask;

        RaycastHit hit;
        if((Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, originalHeight, layerMask)))
        {
            Debug.Log("hit");
            obstacleAbove = true;
        }
        else
        {
            obstacleAbove = false;
            isCrouching = false;
        }

    }
}
