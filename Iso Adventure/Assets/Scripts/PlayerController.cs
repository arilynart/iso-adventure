using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerControls controls;

    Vector2 move;

    Vector3 forward, right, head, point;
    Vector3 rightMovement, upMovement;

    Quaternion targetRotation;


    public bool dodge;
    public bool debug;
    bool grounded;
    bool moving;

    public float moveSpeed = 5f;
    public float turnSpeed = 10f;
    public float dashSpeed = 12f;
    public float height = 0.5f;
    public float heightPadding = 0.05f;
    public float maxGroundAngle = 120;
    float angle;
    float groundAngle;

    public LayerMask ground;

    private new Rigidbody rigidbody;

    RaycastHit hitInfo;


    // Start is called before the first frame update
    void Awake()
    {
        
        controls = new PlayerControls();
        Debug.Log("Controls set");

        //When movement input is performed set move to the value
        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        Debug.Log("Movement performance set.");
        //Otherwise set to 0
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
        Debug.Log("Movement cancellation set.");

        controls.Gameplay.Dodge.performed += ctx => Dodge(move);
        Debug.Log("Dodge performance set.");
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

        dodge = false;
        Debug.Log("Dodge set to false.");
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //set a mov variable every frame to the current controller input
        Vector2 mov = new Vector2(move.x, move.y) * Time.deltaTime;
        rigidbody.velocity = new Vector3(mov.normalized.x * moveSpeed, rigidbody.velocity.y);
        //print("Move: " + move);

        CalculateDirection(mov);
        CalculateGroundAngle();
        CheckGround();
        ApplyGravity();
        DrawDebugLines();
        DodgeMove();
        //if we're inputting a move
        if (move == Vector2.zero || dodge) return;

        Move();
        Rotate();
       
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
            return;
        }

        groundAngle = Vector3.Angle(hitInfo.normal, transform.forward);

        //Debug.Log("Ground Angle calculated: " + groundAngle);
    }

    void CheckGround()
    {
        //Debug.Log("Checking  if grounded...");
        //are we on the ground? raycast of length "height" to determine if so
        if (Physics.Raycast(transform.position, -Vector3.up, out hitInfo, height + heightPadding, ground))
        {
            if (Vector3.Distance(transform.position, hitInfo.point) < height)
            {
                Debug.Log("Cube fell below floor, correcting...");
                transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * height, 5 * Time.deltaTime);
            }
            grounded = true;
        }
        else
        {
            
            grounded = false;
        }
        //Debug.Log("Grounded: " + grounded);
    }

    void ApplyGravity()
    {
        if (!grounded)
        {
            Debug.Log("Not grounded, applying gravity.");
            transform.position += Physics.gravity * Time.deltaTime;
        }
    }
    
    void Move()
    {
        //if we're dodging no movement
        if (dodge) return;
        Debug.Log("Dodge: " + dodge);

        //calculate forward rotation based on input and incline and assign to point variable
        CalculateForward();
        
        //if slope ahead compared to the current location is too high, return.
        if (groundAngle >= maxGroundAngle) return;
        Debug.Log("Slope is passable.");
        //check if we're hitting anything on the ground layer in front of us.
        if (Physics.Raycast(transform.position, point, out hitInfo, height + heightPadding, ground)) return;
        //move the player the direction they are facing in order to account for y-axis changes in terrain 
        transform.position += point * moveSpeed * Time.deltaTime;
        Debug.Log("Moving: " + transform.position);
    }

    void Rotate()
    {
        //lerp to the target rotation
        targetRotation = Quaternion.Euler(0, angle+45, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

        Debug.Log("Rotating: " + transform.rotation);
    }

    void Dodge(Vector2 m)
    {
        Debug.Log("Dodge inputted.");
        //if we're not already dodging
        if (!dodge && grounded)
        {
            Debug.Log("Dodge available.");
            if (m != Vector2.zero)
            {
                Debug.Log("Moving, snapping direction.");
                //snap player rotation to inputted direction
                transform.forward = head;
            }

            dodge = true;
  
            //Start movement
            StartCoroutine(DodgeMovement(0.5f));
        }
    }

    void DodgeMove()
    {
        if (dodge && !Physics.Raycast(transform.position, point, out hitInfo, height + heightPadding, ground))
        {
            Debug.Log("dodging");
            
            transform.position += head * dashSpeed * Time.deltaTime;
            Debug.Log("Position: " + transform.position);
        }
/*        else
        {
            moveSpeed = baseMoveSpeed;
        }*/
    }

    IEnumerator DodgeMovement(float duration)
    {
        Debug.Log("Moving Dodge");
        //reset timer
        float time = 0f;
        

        while (time < duration)
        {
            Debug.Log("Dodge executing: " + time);
            //movement
            dodge = true;
                

            //Increase the timer
            time += Time.deltaTime;

            yield return null;
        }
        //finish movement and remove dodge status.
        dodge = false;
        Debug.Log("Dodge: " + dodge);
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

        Debug.DrawLine(transform.position, transform.position + point * height * 2, Color.blue);
        Debug.DrawLine(transform.position, transform.position - Vector3.up * height, Color.green);
    }
}
