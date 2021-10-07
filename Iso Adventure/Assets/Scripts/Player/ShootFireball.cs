using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.DeveloperConsole.Behavior;


public class ShootFireball : MonoBehaviour
{
    public Vector3 trajectory = Vector3.forward;
    public float speed;
    public bool hit = false;
    public RaycastHit hitInfo;


    private void Update()
    {
        transform.position += trajectory * speed;
        if (Physics.Raycast(transform.position, trajectory, out hitInfo, 0.25f, DeveloperConsoleBehavior.PLAYER.ground))
        {
            Debug.Log("Fireball collided with wall");
            if (hitInfo.collider.GetComponent<Torch>())
            {
                Debug.Log("Fireball hit torch");
                hitInfo.collider.GetComponent<Torch>().EnableTorch();
            }
            hit = true;
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        if (!other.GetComponent<EnemyStats>() && other.tag != "Player")
        {
            hit = true;
            
            return;
        }
        if (other.tag == "Player")
        {
            return;
        }


        Debug.Log("Fireball Hit " + other.name);
        
        EnemyStats stats = other.GetComponent<EnemyStats>();

        //calling damage method on collided enemy
        stats.TakeDamage(DeveloperConsoleBehavior.PLAYER.GetComponent<PlayerCombat>().attackDamage);
        hit = true;
        transform.DetachChildren();
        Destroy(gameObject);
    }
}
