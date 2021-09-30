using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Animator animator;

    public float lookRadius = 5f;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public float attackDelay = 5f;
    public LayerMask playerLayer;

    Transform target;
    NavMeshAgent agent;

    private void Start()
    {
        //Find and target player in instance
        target = PlayerManager.instance.player.transform;
        //Get Nav Mesh
        agent = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate()
    {
        //Distance from enemy to player
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);

            if (distance <= agent.stoppingDistance)
            {
                // Face Target
                FaceTarget();
                // Attack
                Attack();
            }
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    public void Attack()
    {
        //detect enemies in range of attack
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);

        //damage enemies
        foreach (Collider enemy in hitEnemies)
        {
            StartCoroutine(DoDamage(attackDelay));
        }

        //animation controls go here
    }

    IEnumerator DoDamage (float delay)
    {
        Debug.Log("Hit player!");
        yield return new WaitForSeconds(delay);
    }

    private void OnDrawGizmosSelected()
    {
        //Draws radius of lookRadius in Scene View
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
