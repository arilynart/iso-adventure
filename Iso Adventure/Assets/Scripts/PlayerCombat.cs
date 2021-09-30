using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    PlayerController controller;
    PlayerDodge dodge;

    public Animator animator;
    public Transform basicAttackPoint;
    public float basicAttackRange = 0.77f;
    public int noOfPresses = 0;
    System.DateTime lastPressedTime;
    public float maxComboDelay = 0.43f;
    public LayerMask enemyLayers;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        dodge = GetComponent<PlayerDodge>();
    }

    private void FixedUpdate()
    {
        if(System.DateTime.Now.Subtract(lastPressedTime).TotalSeconds > maxComboDelay)
        {
            noOfPresses = 0;
            animator.SetBool("Basic Attack", false);
            animator.SetBool("Basic Attack 2", false);
        }

    }

    public void BasicAttack()
    {
        //if not already dodging or falling (based on collision with floor)
        if (!dodge.dodge && controller.collision == true)
        {
            //detect enemies in range of attack
            Collider[] hitEnemies = Physics.OverlapSphere(basicAttackPoint.position, basicAttackRange, enemyLayers);

            //damage enemies
            foreach (Collider enemy in hitEnemies)
            {
                Debug.Log("Hit " + enemy.name);
            }

            //animation controls go here
            lastPressedTime = System.DateTime.Now;
            noOfPresses++;
            if (noOfPresses == 1)
            {
                animator.SetBool("Basic Attack", true);
                Debug.Log("You attack once!");
            }
            else
            {
                animator.SetBool("Basic Attack", false);
            }

            if (noOfPresses == 2)
            {
                animator.SetBool("Basic Attack", false);
                animator.SetBool("Basic Attack 2", true);
                Debug.Log("You attack twice!");
                noOfPresses = 0;
            }
            else
            {
                animator.SetBool("Basic Attack 2", false);
            }
            //noOfPresses = Mathf.Clamp(noOfPresses, 0, 2);
            return;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (basicAttackPoint == null)
            return;

        Gizmos.DrawWireSphere(basicAttackPoint.position, basicAttackRange);
    }

}
