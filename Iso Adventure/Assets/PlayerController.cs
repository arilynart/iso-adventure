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

    // Start is called before the first frame update
    void Awake()
    {
        
        controls = new PlayerControls();

        //When movement input is performed set move to the value
        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        //Otherwise set to 0
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
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
    }

    // Update is called once per frame
    void Update()
    {
        //set a mov variable every frame to the current controller input
        Vector2 mov = new Vector2(move.x, move.y) * Time.deltaTime;
        print("Move: " + move);
        //if we're inputting a move
        if (mov != Vector2.zero)
        {
            Move(mov);
        }
       
    }
    
    void Move(Vector2 m)
    {
        //positive or negative movement, calculating rotation based on camera.
        Vector3 rightMovement = right * moveSpeed * Time.deltaTime * m.x;
        Vector3 upMovement = forward * moveSpeed * Time.deltaTime * m.y;

        //facing the right direction
        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

        transform.forward = heading;
        transform.position += rightMovement;
        transform.position += upMovement;
        
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
