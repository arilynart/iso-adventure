using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerControls controls;

    Vector2 move;

    [SerializeField]
    float moveSpeed = 250f;

    Vector3 forward, right, head;
    Vector3 rightMovement, upMovement;

    Quaternion targetRotation;


    public bool dodge;
    public bool debug;
    bool grounded;

    public float height = 0.05f;
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
    }

    // Update is called once per frame
    void Update()
    {
        //set a mov variable every frame to the current controller input
        Vector2 mov = new Vector2(move.x, move.y) * Time.deltaTime;
        print("Move: " + move);
        //if we're inputting a move
        if (move == Vector2.zero || dodge) return;

        CalculateDirection(mov);
        Move(mov);
        Rotate();
       
    }

    void CalculateDirection(Vector2 m)
    {
        //Calculate movement based on camera position.
        rightMovement = right * moveSpeed * Time.deltaTime * m.x;
        upMovement = forward * moveSpeed * Time.deltaTime * m.y;

        head = Vector3.Normalize(rightMovement + upMovement);

        //calculate rotation angle based on move direction
        angle = Mathf.Atan2(m.x, m.y);
        angle = Mathf.Rad2Deg * angle;
    }
    
    void Move(Vector2 m)
    {
        //move the player
        transform.position += rightMovement;
        transform.position += upMovement;
    }

    void Rotate()
    {
        //lerp to the target rotation
        targetRotation = Quaternion.Euler(0, angle+45, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
    }

    void Dodge(Vector2 m)
    {
        //if we're not already dodging
        if (!dodge)
        {
            dodge = true;

            //calculating next position based on input or if no input then just forward
            Vector3 dashHead = head * 250f * Time.deltaTime;
            Vector3 dashDefault = transform.forward * 250f * Time.deltaTime;

            Vector3 basePos = transform.position;
            Vector3 newPos;

            if (m != Vector2.zero)
            {
                newPos = basePos + dashHead;
                transform.forward = head;
            }
            else
            {
                newPos = basePos + dashDefault;
            }
            
            //Start movement
            StartCoroutine(LerpMovement(basePos, newPos, 0.5f));
        }
    }

    IEnumerator LerpMovement(Vector3 basePos, Vector3 newPos, float duration)
    {
        //reset timer
        float time = 0f;
        

        while (time < duration)
        {
            //Lerp the movement
            transform.position = Vector3.Lerp(basePos, newPos, time / duration);
            
            //Increase the timer
            time += Time.deltaTime;
            
            yield return null;
        }
        //finish movement and remove dodge status.
        transform.position = newPos;
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
}
