using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodge : MonoBehaviour
{
    public PlayerController controller;
    public PlayerHealth health;

    public float dashSpeed = 8f;
    public float dashDuration = 0.8f;
    public float dashTime = 0.5f;

    public bool dodge;
    public bool dashDelay;

    RaycastHit hitInfo;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        health = GetComponent<PlayerHealth>();

        dodge = false;
        Debug.Log("Dodge set to false.");
    }

    private void FixedUpdate()
    {
        DodgeMove();
    }

    void DodgeMove()
    {
        if (dodge)
        {
            //if the way we are facing is a sharp enough angle to the wall
            if (Physics.Raycast(transform.position, controller.head, out hitInfo, controller.height + controller.heightPadding - 0.10f))
            {
                if (hitInfo.collider.tag != "Enemy")
                {
                    if (Vector3.Angle(hitInfo.normal, controller.head) > 151f)
                    {
                        //cancel the dodge.
                        Debug.Log("Sharp Angle: " + Vector3.Angle(hitInfo.normal, controller.point));
                        return;
                    }
                }

            }
            Debug.Log("dodging");

            if (controller.move == Vector2.zero)
            {
                //move player during dodge
                transform.position += controller.point * dashSpeed * Time.deltaTime;
                Debug.Log("Position: " + transform.position);
            }
            else
            {
                transform.position += controller.head * dashSpeed * Time.deltaTime;
            }
        }
        else
        {

        }
    }

    public void Dodge(Vector2 m)
    {
        Debug.Log("Dodge inputted.");
        //if we're not already dodging
        if (!dodge && !dashDelay)
        {
            Debug.Log("Dodge available.");
            if (m != Vector2.zero)
            {
                Debug.Log("Moving, snapping direction.");
                //snap player rotation to inputted direction
                transform.forward = controller.head;
            }

            //Start movement
            StartCoroutine(DodgeMovement(dashDuration));
        }
        else
        {
            Debug.Log("Cannot dodge at this time.");
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
}
