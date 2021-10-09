using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.DeveloperConsole.Behavior;


public class ShootFireball : MonoBehaviour
{
    public Vector3 trajectory = Vector3.forward;
    public float speed;
    public bool hit = false;


    private void Update()
    {
        transform.position += trajectory * speed;
    }

    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        if (!other.GetComponent<EnemyStats>() && other.tag != "Player")
        {
            hit = true;
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
        //Detach particle emitter to finish its lifetime
        hit = true;
        transform.DetachChildren();
        Destroy(gameObject);
    }
}
