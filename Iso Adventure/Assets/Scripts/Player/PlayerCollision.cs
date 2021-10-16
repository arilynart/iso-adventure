using UnityEngine;
using Ludiq;
using Bolt;

public class PlayerCollision : MonoBehaviour
{
    PlayerController controller;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    private void OnCollisionStay(Collision collision)
    {
        controller.collision = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        controller.collision = false;
        Debug.Log("Stop Colliding");

    }
}