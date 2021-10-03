using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.DeveloperConsole.Behavior;


public class ShootFireball : MonoBehaviour
{
    public Vector3 trajectory = Vector3.forward;
    float speed;



    private void Update()
    {
        transform.position += trajectory * speed;
    }
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<EnemyStats>() && other.tag != "Player")
        {
            Destroy(gameObject);
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
        Destroy(gameObject);
    }
}
