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

    Vector3 forward, right;

    public bool dodge;

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
        if (mov != Vector2.zero && !dodge)
        {
            Move(mov);
        }
       
    }
    
    void Move(Vector2 m)
    {
        //Calculate movement based on camera position.
        Vector3 rightMovement = right * moveSpeed * Time.deltaTime * m.x;
        Vector3 upMovement = forward * moveSpeed * Time.deltaTime * m.y;

        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

        //move and rotate
        transform.forward = Vector3.Lerp(transform.forward, heading, 0.1f);
        transform.position += rightMovement;
        transform.position += upMovement;
        
    }

    void Dodge(Vector2 m)
    {
        //if we're not already dodging
        if (!dodge)
        {
            dodge = true;

            //same movement calculations
            Vector3 rightMovement = right * moveSpeed * Time.deltaTime * m.x;
            Vector3 upMovement = forward * moveSpeed * Time.deltaTime * m.y;

            Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

            //calculating next position based on input or if no input then just forward
            Vector3 dashHead = heading * 250f * Time.deltaTime;
            Vector3 dashDefault = transform.forward * 250f * Time.deltaTime;

            Vector3 basePos = transform.position;
            Vector3 newPos;

            if (m != Vector2.zero)
            {
                newPos = basePos + dashHead;
                transform.forward = heading;
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
