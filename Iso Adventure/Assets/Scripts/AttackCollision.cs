using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider enemy)
    {
        if (enemy.tag != "Enemy") return;

        Debug.Log("Hit " + enemy.name);
        EnemyStats stats = enemy.GetComponent<EnemyStats>();

        //calling damage method on collided enemy
        stats.TakeDamage(transform.parent.GetComponent<PlayerCombat>().attackDamage);
    }
}
