using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Animator animator;

    public EnemyStats stats;

    public float lookRadius = 5f;
    public float attackDelay = 5f;
    public GameObject hurtBox;
    public LayerMask playerLayer;

    Transform target;
    public NavMeshAgent agent;

    private void Start()
    {
        //Find and target player in instance
        target = PlayerManager.instance.player.transform;
        //Get Nav Mesh
        agent = GetComponent<NavMeshAgent>();
        stats = GetComponent<EnemyStats>();

        //Disable ragdoll
        setRigidbodyState(true);
        setColliderState(false);
    }

    void FixedUpdate()
    {
        //Distance from enemy to player
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
            animator.SetFloat("Speed", distance);
            //Debug.Log("Speed: " + distance);

            if (distance <= agent.stoppingDistance)
            {
                // Face Target
                FaceTarget();
                // Attack
                Attack();
            }
        }

        if (distance > lookRadius)
        {
            agent.isStopped = true;
            distance = 0;
            animator.SetFloat("Speed", distance);
            deactivateHurtbox();
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
        if (stats.activeAttack == null) stats.ChooseAttack();
        //Start attack animation
        Debug.Log("Playing animtion: " + stats.activeAttack.animation);
        animator.SetTrigger(stats.activeAttack.animation);
    }

    public void activateHurtbox()
    {
        hurtBox.GetComponent<Collider>().enabled = true;
        Debug.Log("Hurtbox on!");
    }

    public void deactivateHurtbox()
    {
        hurtBox.GetComponent<Collider>().enabled = false;
        Debug.Log("Hurtbox off!");
        stats.activeAttack = null;
    }

    //Enables ragdoll
    public void setRigidbodyState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = state;
        }

        GetComponent<Rigidbody>().isKinematic = state;
    }

    public void setColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            collider.enabled = state;
        }

        GetComponent<Collider>().enabled = !state;
    }

    private void OnDrawGizmosSelected()
    {
        //Draws radius of lookRadius in Scene View
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
