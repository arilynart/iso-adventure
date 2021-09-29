using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerControls controls;
    PlayerCombat combat;
    PlayerHealth health;

    Vector2 move;

    Vector3 forward, right, head, point;
    Vector3 rightMovement, upMovement;

    Quaternion targetRotation;


    public bool dodge;
    public bool debug;
    public bool collision;
    public bool dashDelay;
    public bool invuln;
    

    bool grounded;
    bool moving;

    public float moveSpeed = 5f;
    public float turnSpeed = 10f;
    public float dashSpeed = 8f;
    public float dashDuration = 0.8f;
    public float dashTime = 0.5f;
    public float height = 0.5f;
    public float heightPadding = 0.05f;
    public float maxGroundAngle = 120;
    public float invulnDuration;
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

        combat = GetComponent<PlayerCombat>();
        controls.Gameplay.Attack.performed += ctx => combat.BasicAttack();

        health = GetComponent<PlayerHealth>();
        controls.Gameplay.Heal.performed += ctx => health.HealDamage(1);

    }

    void Start()
    {
        Physics.IgnoreLayerCollision(3, 7, false);
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
        //print("Move: " + move);

        CalculateDirection(mov);
        CalculateGroundAngle();
        CheckGround();
        DrawDebugLines();
        DodgeMove();

        
        //if we're inputting a move and not dodging
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
    
    void Move()
    {
        Debug.Log("Dodge: " + dodge);
        //if we're dodging no movement
        if (dodge) return;

        //calculate forward rotation based on input and incline and assign to point variable
        CalculateForward();
        
        //if slope ahead compared to the current location is too high, return.
        if (groundAngle >= maxGroundAngle) return;
        //Debug.Log("Slope is passable.");

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
/*        if (invulnBreak)
        {
            dashDelay = false;
            invuln = false;
            dodge = false;
            invulnBreak = false;
            StopAllCoroutines();
        }*/
        Debug.Log("Dodge inputted.");
        //if we're not already dodging
        if (!dodge && !dashDelay)
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
            StartCoroutine(DodgeMovement(dashDuration));
        }
        else
        {
            Debug.Log("Cannot dodge at this time.");
        }
    }

    void DodgeMove()
    {
        if (dodge)
        {
            //if the way we are facing is a sharp enough angle to the wall
            if (Physics.Raycast(transform.position, head, out hitInfo, height + heightPadding - 0.10f))
            {
                if (hitInfo.collider.tag != "Enemy")
                {
                    if (Vector3.Angle(hitInfo.normal, head) > 151f)
                    {
                        //cancel the dodge.
                        Debug.Log("Sharp Angle: " + Vector3.Angle(hitInfo.normal, point));
                        return;
                    }
                }

            }
            Debug.Log("dodging");

            if (move == Vector2.zero)
            {
                //move player during dodge
                transform.position += point * dashSpeed * Time.deltaTime;
                Debug.Log("Position: " + transform.position);
            }
            else
            {
                transform.position += head * dashSpeed * Time.deltaTime;
            }
        }
        else
        {
            
        }
    }

    public IEnumerator DodgeMovement(float duration)
    {
        Debug.Log("Moving Dodge");
        //reset timer
        float time = 0f;
        float bar = duration - dashTime;
        StartCoroutine(health.Invulnerability(0.3f));

        while (time < duration)
        {

            //for the first 0.3s of the dodge
            if (time < 0.3)
            {
                //we are dodging
                Debug.Log("Dodge executing: " + time);
                //movement
                dodge = true;
            }
            else
            {
                //afterwards, we aren't dodging.
                Debug.Log("Delay executing: " + time);
                dodge = false;
                dashDelay = true;

            }
            

            //Increase the timer
            time += Time.deltaTime;

            yield return null;
        }
        //finish movement and remove dodge status.
        dashDelay = false;
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
