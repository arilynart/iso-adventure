using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.DeveloperConsole.Behavior;


public class EnemyFireball : MonoBehaviour
{
    public Vector3 trajectory = Vector3.forward;
    public float speed;
    public bool hit = false;
    public RaycastHit hitInfo;
    public int damage = 0;


    private void Update()
    {
        transform.position += trajectory * speed;
        if (Physics.Raycast(transform.position, trajectory, out hitInfo, 0.25f, DeveloperConsoleBehavior.PLAYER.ground))
        {
            Debug.Log("Fireball collided with wall");
            hit = true;
            transform.DetachChildren();
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        if (!controller) return;
        if (controller.invuln) return;

        Debug.Log("Fireball Hit " + other.name);



        //calling damage method on collided enemy
        controller.health.TakeDamage(damage);
        //Detach particle emitter to finish its 

        hit = true;
        transform.DetachChildren();
        Destroy(gameObject);


    }
}
