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
    public float dashDuration = 0.7f;
    public float dashTime = 0.5f;

    public bool dodge;
    public bool dashDelay;

    public Rigidbody rb;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        health = GetComponent<PlayerHealth>();
        rb = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();
        dodge = false;

        Debug.Log("Dodge set to false.");
    }

    public void Dodge()
    {
        if ((bool)Variables.Object(gameObject).Get("animLock") == true) return;
        if (controller.onLadder) return;
        Debug.Log("Dodge inputted.");
        //if we're not already dodging
        if (!dashDelay)
        {
            controller.moving = false;
            if (controller.groundAngle != 90)
                GetComponent<Rigidbody>().AddForce(0, -controller.slopeForce * 50, 0);

            Debug.Log("Dodge available.");
            if (controller.move != Vector2.zero)
            {
                Debug.Log("Moving, snapping direction.");
                //snap player rotation to inputted direction
                controller.point = controller.headPoint;
                transform.forward = controller.headPoint;
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
        StartCoroutine(health.Invulnerability(dashTime));
        Physics.IgnoreLayerCollision(3, 7, true);
        Physics.IgnoreLayerCollision(3, 11, true);

        while (time < duration)
        {
            
            //for the first 0.3s of the dodge
            if (time <= dashTime)
            {
                rb.velocity = controller.point * dashSpeed;
                Debug.Log(" O1Velocity: " + transform.position);

                //we are dodging
                Debug.Log("Dodge executing: " + time);
                //movement
            }
            else
            {
                //afterwards, we aren't dodging.
                Physics.IgnoreLayerCollision(3, 7, false);
                Physics.IgnoreLayerCollision(3, 11, false);
                CustomEvent.Trigger(gameObject, "ReturnDodge");
                
                dashDelay = true;
                
            }

            if (time < dashTime + 0.05f && time > dashTime) rb.velocity = Vector3.zero;

            //Increase the timer
            time += Time.deltaTime;

            yield return null;
        }
        
        dashDelay = false;
    }
}
