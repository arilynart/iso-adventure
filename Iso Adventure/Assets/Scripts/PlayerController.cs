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

    public float baseMoveSpeed = 5f;
    float moveSpeed;
    public float turnSpeed = 10f;
    public float dashSpeed = 10f;
    public float height = 0.5f;
    public float heightPadding = 0.05f;
    public float maxGroundAngle = 120;
    float angle;
    float groundAngle;

    public LayerMask ground;

    RaycastHit hitInfo;


    // Start is called before the first frame update
    void Awake()
    {
        
        controls = new PlayerControls();

        //When movement input is performed set move to the value
        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        //Otherwise set to 0
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;

        controls.Gameplay.Dodge.performed += ctx => Dodge(move);
    }

    void Start()
    {
        //forward is the way we're looking
        forward = Camera.main.transform.forward;
        //but it has no y value
        forward.y = 0;
        //normalize the value to 1 or 0
        forward = Vector3.Normalize(forward);
        //rotation calculations I don't understand
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;

        dodge = false;

        moveSpeed = baseMoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        print(moveSpeed);
        //set a mov variable every frame to the current controller input
        Vector2 mov = new Vector2(move.x, move.y) * Time.deltaTime;
        //print("Move: " + move);

        CalculateDirection(mov);
        CalculateForward();
        CalculateGroundAngle();
        CheckGround();
        ApplyGravity();
        DrawDebugLines();
        DodgeMove();
        //if we're inputting a move
        if (move == Vector2.zero || dodge) return;

        Move(mov);
        Rotate();
       
    }

    void CalculateDirection(Vector2 m)
    {
        //Calculate movement based on camera position.
        rightMovement = right * moveSpeed * Time.deltaTime * m.x;
        upMovement = forward * moveSpeed * Time.deltaTime * m.y;

        //calculate rotation angle based on move direction
        angle = Mathf.Atan2(m.x, m.y);
        angle = Mathf.Rad2Deg * angle;
    }

    void CalculateForward()
    {
        head = Vector3.Normalize(rightMovement + upMovement);

        if (!grounded)
        {
            point = transform.forward;
            return;
        }

        point = Vector3.Cross(transform.right, hitInfo.normal);
    }

    void CalculateGroundAngle()
    {
        if (!grounded)
        {
            groundAngle = 90;
            return;
        }

        groundAngle = Vector3.Angle(hitInfo.normal, transform.forward);
    }

    void CheckGround()
    {
        //are we on the ground? raycast of length "height" to determine if so
        if (Physics.Raycast(transform.position, -Vector3.up, out hitInfo, height + heightPadding, ground))
        {
            if (Vector3.Distance(transform.position, hitInfo.point) < height)
            {
                transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * height, 5 * Time.deltaTime);
            }
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    void ApplyGravity()
    {
        if (!grounded)
        {
            transform.position += Physics.gravity * Time.deltaTime;
        }
    }
    
    void Move(Vector2 m)
    {
        //if we're not dodging
        if (dodge) return;

        CalculateForward();

     
        
        //if slope is too high, return.
        if (groundAngle >= maxGroundAngle) return;
        //wall collision
        if (Physics.Raycast(transform.position, point, out hitInfo, height + heightPadding, ground)) return;
       
        //move the player
        transform.position += point * moveSpeed * Time.deltaTime;
        //transform.position += upMovement;
    }

    void Rotate()
    {
        //lerp to the target rotation
        targetRotation = Quaternion.Euler(0, angle+45, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    void Dodge(Vector2 m)
    {
        //if we're not already dodging
        if (!dodge && grounded)
        {

            dodge = true;

            if (m != Vector2.zero)
            {
                
                transform.forward = head;
            }
            else
            {
                
            }
            
            //Start movement
            StartCoroutine(DodgeMovement(0.5f));
        }
    }

    void DodgeMove()
    {
        if (dodge /*&& !Physics.Raycast(transform.position, point, out hitInfo, height + heightPadding, ground)*/)
        {
            //wall collision
            print("dodging");
            
            transform.position += point * dashSpeed * Time.deltaTime;
        }
        else
        {
            moveSpeed = baseMoveSpeed;
        }
    }

    IEnumerator DodgeMovement(float duration)
    {
        //reset timer
        float time = 0f;
        

        while (time < duration)
        {
            //Lerp the movement
            dodge = true;
                

            //Increase the timer
            time += Time.deltaTime;

            yield return null;
        }
        //finish movement and remove dodge status.
        //transform.position = newPos;
        dodge = false;
        
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
