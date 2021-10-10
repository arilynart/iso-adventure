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
            else if (hitInfo.collider.GetComponent<BlockPush>())
            {
                Debug.Log("Direction: " + trajectory);

                hitInfo.collider.GetComponent<BlockPush>().Slide(DeveloperConsoleBehavior.PLAYER.transform.forward);
            }
            else if (hitInfo.collider.GetComponent<BlockReset>())
            {
                hitInfo.collider.GetComponent<BlockReset>().Restart();
            }
            hit = true;
            transform.DetachChildren();
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {

        if (other.GetComponent<EnemyStats>())
        {
            Debug.Log("Fireball Hit " + other.name);

            EnemyStats stats = other.GetComponent<EnemyStats>();

            //calling damage method on collided enemy
            stats.TakeDamage(DeveloperConsoleBehavior.PLAYER.GetComponent<PlayerCombat>().manaDamage);
            //Detach particle emitter to finish its lifetime

        }
/*        else if (!other.GetComponent<EnemyStats>() && other.tag != "Player")
        {
            hit = true;

            return;
        }*/
        else if (other.tag == "Player")
        {
            return;
        }
        hit = true;
        transform.DetachChildren();
        Destroy(gameObject);
    }
}
