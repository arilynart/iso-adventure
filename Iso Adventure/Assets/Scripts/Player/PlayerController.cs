using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.InputSystem;
using Arilyn.DeveloperConsole.Behavior;
using Arilyn.DeveloperConsole.Commands;
using Arilyn.State.PlayerState;

public class PlayerController : MonoBehaviour
{
    public static PlayerController CONTROL;

    public PlayerControls controls;
    public PlayerStateMachine machine;
    public PlayerHealth health;
    public PlayerBlink blink;
    public PlayerMana mana;
    public Ladder currentLadder;
    CameraRotate camRot;

    public GameObject mousePoint;


    Vector2 moveLast;
    Vector2 lastMousePos;

    Vector3 forward, right;
    Vector3 rightMovement, upMovement;

    public Vector3 head;
    public Vector3 point;
    public Vector3 headPoint;
    public Vector2 move;

    Quaternion targetRotation;

    public Animator animator;

    public bool debug;
    public bool collision;
    public bool invuln;
    public bool grounded;
    public bool moving;
    public bool interacting;
    public bool interactTrigger;
    public bool onLadder;
    public bool exitLadder;
    public bool airDodge;
    bool idle;

    int idleCount;
    public int idleTimer = 180;

    public float moveSpeed = 5f;
    public float turnSpeed = 10f;
    
    public float height = 0.5f;
    public float heightPadding = 0.05f;
    public float slopeForce;
    public float maxGroundAngle = 120;
    float angle;
    float angleOffset;
    public float groundAngle;

    public LayerMask ground;
    public LayerMask indoors;
    public LayerMask mouseLayer;

    RaycastHit hitInfo;


    // Start is called before the first frame update
    void Awake()
    {
        
        controls = new PlayerControls();
        machine = GetComponent<PlayerStateMachine>();
        mana = GetComponent<PlayerMana>();
        blink = GetComponent<PlayerBlink>();
        health = GetComponent<PlayerHealth>();
        //Debug.Log("Controls set");

        //Debug.Log("Movement performance set.");
        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        //Debug.Log("Movement cancellation set.");
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
        controls.Gameplay.Move.canceled += ctx => moving = false;

        controls.Gameplay.Dodge.started += ctx => machine.Dodge();
        //Debug.Log("Dodge performance set.");

        controls.Gameplay.Attack.started += ctx => machine.BasicAttack();
        controls.Gameplay.Shoot.started += ctx => machine.Shoot();
        
        controls.Gameplay.Heal.started += ctx => machine.Heal();
        
        controls.Gameplay.Blink.started += ctx => machine.Blink();

        controls.Gameplay.Interact.performed += ctx => interacting = true;
        controls.Gameplay.Interact.canceled += ctx => interacting = false;
        controls.Gameplay.Interact.started += ctx => machine.Interact();
        controls.Gameplay.Interact.canceled += ctx => interactTrigger = false;

        controls.Gameplay.Pause.started += ctx => PauseMenu.PAUSE();

        if (DeveloperConsoleBehavior.PLAYER != null && DeveloperConsoleBehavior.PLAYER != this)
        {
            Destroy(gameObject);
            return;
        }

        DeveloperConsoleBehavior.PLAYER = this;
    }

    void Start()
    {
        idleCount = 181;

        camRot = CameraFollow.MAINCAMERA.GetComponent<CameraRotate>();

        SetCamera();
    }

    private void Update()
    {
        CheckGround();
        CalculateGroundAngle();

        if (MoveDiff())
        {
            SetCamera();
        }
        moveLast = move;
        Vector2 mov = moveLast * Time.deltaTime;
        CalculateDirection(mov);
        CalculateForward();

        DrawDebugLines();

        if (!GodCommand.GODMODE) return;

        invuln = true;
        machine.attackDamage = 99;
        mana.mana = 99;
        machine.manaDamage = 99;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        machine.Movement(move);
        //print("Move: " + move);
        //Debug.Log("Idle: " + idleCount);
        if (lastMousePos == Mouse.current.position.ReadValue())
        {
            idleCount++;
        }
        else
        {
            idle = false;
            idleCount = 0;
            mousePoint.SetActive(true);
        }
        if (idleCount > idleTimer)
        {
            idle = true;
            mousePoint.SetActive(false);
            idleCount = idleTimer;
        }
        lastMousePos = Mouse.current.position.ReadValue();

        if (MouseActivityCheck())
        {
            MouseRotate();
        }
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
/*
        if (move == Vector2.zero)
        {
            point = Vector3.zero;
            headPoint = Vector3.zero;
            return;
        }*/

        //Debug.Log("Head calculated: " + head);
        if (!grounded)
        {
            point = transform.forward;
            headPoint = transform.forward;
            return;
        }
        headPoint = Vector3.Cross(Quaternion.Euler(new Vector3(0, 90, 0)) * head, hitInfo.normal);
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
        //Debug.Log("ForwardGround Angle calculated: " + forwardGroundAngle);
    }

    void CheckGround()
    {
        //Debug.Log("Checking  if grounded...");
        //are we on the ground? raycast of length "height" to determine if so
        if (Physics.Raycast(transform.position + new Vector3(0, height, 0), -Vector3.up, out hitInfo, height + heightPadding, ground))
        {
            grounded = true;

        }
        else if (Physics.Raycast(transform.position + new Vector3(0, height, 0), -Vector3.up, out hitInfo, height + heightPadding, indoors))
        {
            grounded = true;
        }
        else if (groundAngle != 90 && groundAngle > 0)
        {
            grounded = true;
        }
        else
        { 
            grounded = false;
        }
        //Debug.Log("Grounded: " + grounded);
    }
    
    void AddSlopeForce(float amount)
    {
        GetComponent<Rigidbody>().AddForce(0, -amount, 0);
    }
    
    public void Move()
    {
        if (move != Vector2.zero)
        {
            Rotate();
            //if slope ahead compared to the current location is too high, return.
            if (groundAngle > maxGroundAngle) return;
            //Debug.Log("Slope is passable.");

            if (groundAngle != 90)
                AddSlopeForce(slopeForce);

            moving = true;
            //move the player the direction they are facing in order to account for y-axis changes in terrain 
            transform.position += headPoint * moveSpeed * Time.deltaTime;
            //Debug.Log("Moving: " + transform.position);
        }
    }

    void Rotate()
    {
        //lerp to the target rotation
        targetRotation = Quaternion.Euler(0, angle + angleOffset, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }
    
    void MouseRotate()
    {
        mousePoint.transform.LookAt(GetLookPoint());
    }

    public Vector3 GetLookPoint()
    {
        RaycastHit hit;
        Ray ray = CameraFollow.MAINCAMERA.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out hit, 100, mouseLayer))
        {
            return new Vector3(hit.point.x, mousePoint.transform.position.y, hit.point.z);
        }
        else
        {
            return mousePoint.transform.forward;
        }
    }

    public bool MouseActivityCheck()
    {
        return !idle;
    }

    public void SetCamera()
    {
        //forward is the way we're looking
        forward = CameraFollow.MAINCAMERA.transform.forward;
        //Debug.Log("Forward direction set.");
        //but it has no y value
        forward.y = 0;
        //Debug.Log("Forward y value set.");
        //normalize the value to 1 or 0
        forward = Vector3.Normalize(forward);
        //Debug.Log("Forward normalized: " + forward);
        //the right direction is 90 degrees from our forward
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
        //Debug.Log("Right direction set.");

        angleOffset = camRot.rotation;
    }

    public bool MoveDiff()
    {
        if (move.x < moveLast.x - 0.05f || move.x > moveLast.x + 0.05f ||
            move.y < moveLast.y - 0.05f || move.y > moveLast.y + 0.05f)
            return true;
        return false;
    }

    public void LadderMove()
    {
        headPoint = new Vector3(0, move.y + Mathf.Abs(move.x), 0);
        moving = true;
        transform.position += headPoint * moveSpeed * Time.deltaTime;
    }

    public void StartLadder(Ladder ladder)
    {
        interactTrigger = false;
        currentLadder = ladder;
        machine.ChangeState(new LadderState(machine));
    }

    public void LeaveLadder()
    {
        onLadder = false;
        machine.rb.useGravity = true;
        machine.ChangeState(new IdleState(machine));
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

        Debug.DrawLine(transform.position + new Vector3(0, height + heightPadding, 0), transform.position + headPoint + new Vector3(0, height + heightPadding, 0), Color.blue);
        Debug.DrawLine(transform.position, transform.position - Vector3.up * height, Color.green);
    }
}
