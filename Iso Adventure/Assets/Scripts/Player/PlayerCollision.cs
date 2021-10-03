using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    PlayerController controller;
    PlayerHealth health;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        health = GetComponent<PlayerHealth>();
    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    private void OnCollisionStay(Collision collision)
    {
        controller.collision = true;
        //Debug.Log("Colliding");

    }

    private void OnCollisionExit(Collision collision)
    {
        controller.collision = false;
        Debug.Log("Stop Colliding");
    }
}