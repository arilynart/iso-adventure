using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerControls controls;

    Vector2 move;

    [SerializeField]
    float moveSpeed = 150f;

    Vector3 forward, right;

    // Start is called before the first frame update
    void Awake()
    {
        
        controls = new PlayerControls();

        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
    }

    void Start()
    {
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mov = new Vector2(move.x, move.y) * Time.deltaTime;
        print("Move: " + move);
        if (mov != Vector2.zero)
        {
            Move(mov);
            //transform.Translate(m, Space.World);
        }
       
    }
    
    void Move(Vector2 m)
    {
        //Vector3 direction = new Vector3(m.x * moveSpeed, 0, m.y * moveSpeed);
        Vector3 rightMovement = right * moveSpeed * Time.deltaTime * m.x;
        Vector3 upMovement = forward * moveSpeed * Time.deltaTime * m.y;

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
