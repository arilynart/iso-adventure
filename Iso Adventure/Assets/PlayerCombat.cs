using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    PlayerController controller;

    public Transform basicAttackPoint;
    public float basicAttackRange = 0.5f;
    public LayerMask enemyLayers;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    public void BasicAttack()
    {
        //if not already dodging or falling (based on collision with floor)
        if (!controller.dodge && controller.collision == true)
        {
            Debug.Log("You attack!");
            //animation controls go here

            //detect enemies in range of attack
            Collider[] hitEnemies = Physics.OverlapSphere(basicAttackPoint.position, basicAttackRange, enemyLayers);

            //damage enemies
            foreach (Collider enemy in hitEnemies)
            {
                Debug.Log("Hit " + enemy.name);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (basicAttackPoint == null)
            return;

        Gizmos.DrawWireSphere(basicAttackPoint.position, basicAttackRange);
    }

}
