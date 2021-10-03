using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Ludiq;
using Bolt;

public class PlayerDodge : MonoBehaviour
{
    PlayerController controller;
    PlayerHealth health;

    public Animator animator;

    public float dashSpeed = 8f;
    public float dashDuration = 0.8f;
    public float dashTime = 0.5f;

    public bool dodge;
    public bool dashDelay;
    public bool velocity;

    RaycastHit hitInfo;
    Rigidbody rb;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        health = GetComponent<PlayerHealth>();
        rb = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();
        dodge = false;

        Debug.Log("Dodge set to false.");
    }

    private void FixedUpdate()
    {
        DodgeMove();
    }

    void DodgeMove()
    {



    }

    public void Dodge()
    {
                if ((bool)Variables.Object(gameObject).Get("animLock") == true) return;
                Debug.Log("Dodge inputted.");
                //if we're not already dodging
                if (!dashDelay)
                {
                    controller.moving = false;
                    if (controller.groundAngle != 90)
                        GetComponent<Rigidbody>().AddForce(0, -controller.slopeForce, 0);

                    Debug.Log("Dodge available.");
                    if (controller.move != Vector2.zero)
                    {
                        Debug.Log("Moving, snapping direction.");
                        //snap player rotation to inputted direction
                        controller.point = controller.head;
                        transform.forward = controller.head;
                    }

                    if (controller.groundAngle > controller.maxGroundAngle) return;

                    CustomEvent.Trigger(gameObject, "DodgeButton");
                    //Start movement
                    StartCoroutine(DodgeMovement(dashDuration));
                }

        
    }

    IEnumerator DodgeMovement(float duration)
    {
        Debug.Log("Moving Dodge");
        //reset timer
        float time = 0f;
        StartCoroutine(health.Invulnerability(duration - dashTime));

        while (time < duration)
        {
            
            //for the first 0.3s of the dodge
            if (time <= 0.3f)
            {
                if (velocity)
                    {

                        rb.velocity = controller.point * dashSpeed;
                        Debug.Log(" O1Velocity: " + transform.position);

                    }
                    //move player during dodge
                    else
                    {
                        transform.position += controller.point * dashSpeed * Time.deltaTime;
                        Debug.Log(" O1Position: " + transform.position);
                    }
                    //we are dodging
                    Debug.Log("Dodge executing: " + time);
                    //movement
                    //dodge = true;

            }
            else
            {
                CustomEvent.Trigger(gameObject, "Return");
                //afterwards, we aren't dodging.
                Debug.Log("Delay executing: " + time);
                //dodge = false;
                dashDelay = true;
            }
            if (time < 0.39 && time > 0.29)
            {
                rb.velocity = Vector3.zero;
            }

            //Increase the timer
            time += Time.deltaTime;

            yield return null;
        }
        //finish movement and remove dodge status.
        dashDelay = false;
        dashSpeed = 8f;
    }
}
