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
    private void OnTriggerEnter(Collider enemy)
    {
        if (!enemy.GetComponent<EnemyStats>()) return;
        
        Debug.Log("Hit " + enemy.name);
        EnemyStats stats = enemy.GetComponent<EnemyStats>();

        //calling damage method on collided enemy
        stats.TakeDamage(transform.parent.GetComponent<PlayerCombat>().attackDamage);
        //mana restoration
        mana.AddMana(1);
    }
}
