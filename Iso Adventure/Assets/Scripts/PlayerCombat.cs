using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    PlayerController controller;

    public Animator animator;
    public Transform basicAttackPoint;
    public float basicAttackRange = 0.77f;
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
            //animation controls go here
            animator.SetTrigger("Basic Attack");
            Debug.Log("You attack!");

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
