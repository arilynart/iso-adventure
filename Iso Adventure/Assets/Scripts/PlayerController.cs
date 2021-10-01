using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerControls controls;
    PlayerCombat combat;
    PlayerHealth health;
    PlayerDodge playerDodge;
    public PlayerInput input;

    public GameObject mousePoint;

    public Vector2 move;

    Vector3 forward, right;
    Vector3 rightMovement, upMovement, mouseR, mouseU;

    public Vector3 head;
    public Vector3 point;

    Quaternion targetRotation;
    Quaternion mouseRotation;

    public Animator animator;

    public bool debug;
    public bool collision;
    public bool invuln;
    bool grounded;
    public bool moving;

    public float moveSpeed = 5f;
    public float turnSpeed = 10f;
    
    public float height = 0.5f;
    public float heightPadding = 0.05f;
    public float maxGroundAngle = 120;
    float angle;
    float mouseAngle;
    float groundAngle;
    float forwardGroundAngle;

    public LayerMask ground;

    private new Rigidbody rigidbody;

    RaycastHit hitInfo;
    RaycastHit hitInfoF;


    // Start is called before the first frame update
    void Awake()
    {
        
        controls = new PlayerControls();
        Debug.Log("Controls set");

        Debug.Log("Movement performance set.");
        Debug.Log("Movement cancellation set.");

        playerDodge = GetComponent<PlayerDodge>();
        Debug.Log("Dodge performance set.");

        combat = GetComponent<PlayerCombat>();

        health = GetComponent<PlayerHealth>();

        input = GetComponent<PlayerInput>();

    }

    void Start()
    {
        // Initialize rigidbody reference
        rigidbody = GetComponent<Rigidbody>();


        //forward is the way we're looking
        forward = Camera.main.transform.forward;
        Debug.Log("Forward direction set.");
        //but it has no y value
        forward.y = 0;
        Debug.Log("Forward y value set.");
        //normalize the value to 1 or 0
        forward = Vector3.Normalize(forward);
        Debug.Log("Forward normalized: " + forward);
        //rotation calculations I don't understand
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
        Debug.Log("Forward direction set.");
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //set a mov variable every frame to the current controller input
        Vector2 mov = new Vector2(move.x, move.y) * Time.deltaTime;
        //print("Move: " + move);

        CalculateDirection(mov);
        CalculateGroundAngle();
        CheckGround();
        DrawDebugLines();

        if (collision == false && !playerDodge.dodge)
        {
            animator.SetBool("Falling", true);
        }
        else
        {
            animator.SetBool("Falling", false);
        }
/*        if (Input.mousePresent)
        {*/
            //Debug.Log("Mouse Present");
            MouseRotate();
/*        }*/

        //if we're inputting a move and not dodging
        if (move == Vector2.zero)
        {
            moving = false;
            //Debug.Log("Moving: " + moving);
            animator.SetBool("Speed", moving);
            return;
        }
        if (playerDodge.dodge) return;

        Move();
        Rotate();

    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        move = inputMovement;
    }

    void CalculateDirection(Vector2 m)
    {
        //Debug.Log("Calculating direction...");

        //Calculate movement based on camera position.
        rightMovement = right * moveSpeed * Time.deltaTime * m.x;
        upMovement = forward * moveSpeed * Time.deltaTime * m.y;

        //calculate rotation angle based on move direction
        angle = Mathf.Atan2(m.x, m.y);
        angle = Mathf.Rad2Deg * angle;
        //Debug.Log("Direction calculated. Right: " + rightMovement + " Up: " + upMovement + "Angle: " + angle);


    }

    void CalculateForward()
    {
        //Debug.Log("Calculating Point...");

        head = Vector3.Normalize(rightMovement + upMovement);
        //Debug.Log("Head calculated: " + head);
        if (!grounded)
        {
            point = transform.forward;
            return;
        }

        point = Vector3.Cross(transform.right, hitInfo.normal);

        //Debug.Log("Point calculated: " + point);
    }

    void CalculateGroundAngle()
    {
        //Debug.Log("Calculating Ground Angle...");
        if (!grounded)
        {
            groundAngle = 90;
            forwardGroundAngle = 90;
            return;
        }

        groundAngle = Vector3.Angle(hitInfo.normal, transform.forward);
        forwardGroundAngle = Vector3.Angle(hitInfoF.normal, transform.forward);

        if (Physics.Raycast(transform.position + transform.forward + new Vector3(0, height + heightPadding, 0), -Vector3.up, out hitInfoF, height + heightPadding, ground))
        {
            if (groundAngle > 90f || groundAngle < 90f || forwardGroundAngle > 90f || forwardGroundAngle < 90f)
            {
                playerDodge.velocity = false;
            }
            else
            {
                playerDodge.velocity = true;
            }
        }

        Debug.Log("Ground Angle calculated: " + groundAngle);
        Debug.Log("ForwardGround Angle calculated: " + forwardGroundAngle);
    }

    void CheckGround()
    {
        //Debug.Log("Checking  if grounded...");
        //are we on the ground? raycast of length "height" to determine if so
        if (Physics.Raycast(transform.position + new Vector3(0, height + heightPadding, 0), -Vector3.up, out hitInfo, height + heightPadding, ground))
        {
            grounded = true;

        }
        else
        {
            
            grounded = false;
        }
        Debug.Log("Grounded: " + grounded);
    }
    
    void Move()
    {
        //if we're dodging no movement
        if (playerDodge.dodge) return;
        Debug.Log("Dodge: " + playerDodge.dodge);

        //calculate forward rotation based on input and incline and assign to point variable
        CalculateForward();
        
        //if slope ahead compared to the current location is too high, return.
        if (groundAngle >= maxGroundAngle) return;
        //Debug.Log("Slope is passable.");

        //move the player the direction they are facing in order to account for y-axis changes in terrain 
        transform.position += point * moveSpeed * Time.deltaTime;
        //Debug.Log("Moving: " + transform.position);
        moving = true;
        animator.SetBool("Speed", moving);
        //Debug.Log("Moving: " + moving);
    }

    void Rotate()
    {
        //lerp to the target rotation
        targetRotation = Quaternion.Euler(0, angle+45, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

        Debug.Log("Rotating: " + transform.rotation);
    }

    void MouseRotate()
    {
        mousePoint.transform.LookAt(GetLookPoint());
    }

    public Vector3 GetLookPoint()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;
        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.cyan);
            return new Vector3(pointToLook.x, mousePoint.transform.position.y, pointToLook.z);
        }
        else
        {
            return Vector3.zero;
        }
    }

    

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    void DrawDebugLines()
    {
        if (!debug) return;

        Debug.DrawLine(transform.position + new Vector3(0, (height + heightPadding) / 5, 0), transform.position + point + new Vector3(0, (height + heightPadding) / 5, 0), Color.blue);
        Debug.DrawLine(transform.position, transform.position - Vector3.up * height, Color.green);
    }
}
