using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollision : MonoBehaviour
{
    PlayerMana mana;

    private void Start()
    {
        mana = transform.parent.GetComponent<PlayerMana>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyStats>()) {
            Debug.Log("Hit " + other.name);
            EnemyStats stats = other.GetComponent<EnemyStats>();

            //calling damage method on collided enemy
            stats.TakeDamage(transform.parent.GetComponent<PlayerCombat>().attackDamage);
            //mana restoration
            mana.AddMana(1);
        }
        else if (other.GetComponent<BlockPush>())
        {

            Vector3 dir = new Vector3(transform.position.x - other.transform.position.x, 0, transform.position.z - other.transform.position.z);
            
            dir = -dir.normalized;
            Debug.Log("Direction: " + dir);
            dir = NormalizeDirection(dir);

            other.GetComponent<BlockPush>().Slide(dir);
        }
    }

    Vector3 NormalizeDirection(Vector3 dir)
    {
        float dirX;
        float dirZ;
        if (dir.x < 1.5 && dir.x >= 0.5)
        {
            dirX = 1;
            dirZ = 0;
        }
        else if (dir.x > -1.5 && dir.x <= -0.5)
        {
            dirX = -1;
            dirZ = 0;
        }
        else if (dir.z < 1.5 && dir.z >= 0.5)
        {
            dirZ = 1;
            dirX = 0;
        }
        else if (dir.z > -1.5 && dir.z <= -0.5)
        {
            dirZ = -1;
            dirX = 0;
        }
        else
        {
            dirZ = 0;
            dirX = 0;
        }
        return new Vector3(dirX, 0, dirZ);
    }
}
