using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Ludiq;
using Bolt;

public class EnemyController : MonoBehaviour
{
    public Animator animator;

    public EnemyStats stats;

    public float lookRadius = 5f;
    public float attackDelay = 5f;
    public GameObject hurtBox;
    public LayerMask playerLayer;

    //Transform target;
    public NavMeshAgent agent;

    private void Awake()
    {
        //Find and target player in instance
        //target = PlayerManager.instance.player.transform;
        //Get Nav Mesh
        /*agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;*/
        stats = GetComponent<EnemyStats>();
        animator = GetComponent<Animator>();
        //gameObject.SetActive(false);
    }

    public void Spawn()
    {
        //agent.enabled = true;
        gameObject.SetActive(true);
    }

    public void Attack()
    {
        if (stats.activeAttack == null) stats.ChooseAttack();

        stats.InitializeAttack();
        //Start attack animation
        stats.attack.ExecuteAttack();
    }


    public IEnumerator AttackAnimation(float hurtBoxStart, float hurtBoxEnd)
    {
        float time = 0;
        while (time < animator.GetCurrentAnimatorStateInfo(0).length)
        {
            if (time >= hurtBoxStart && time < hurtBoxEnd && hurtBoxStart >= 0) activateHurtbox();
            if (time > hurtBoxEnd && hurtBoxEnd >= 0) deactivateHurtbox();

            time += Time.deltaTime;
            yield return null;
        }

        CustomEvent.Trigger(gameObject, "EndEnemyAttack");
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
        //stats.activeAttack = null;
    }

/*    private void OnDrawGizmosSelected()
    {
        //Draws radius of lookRadius in Scene View
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }*/
}
