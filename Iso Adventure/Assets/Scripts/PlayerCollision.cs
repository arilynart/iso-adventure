
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerController controller;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    private void OnCollisionStay(Collision collision)
    {
        controller.collision = true;
        Debug.Log("Colliding");
        
    }

    private void OnCollisionExit(Collision collision)
    {
        controller.collision = false;
        Debug.Log("Stop Colliding");
    }
}
