using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.DeveloperConsole.Behavior;

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
            //Debug.Log("Direction: " + dir);

            other.GetComponent<BlockPush>().Slide(DeveloperConsoleBehavior.PLAYER.transform.forward);
        }
    }


}
