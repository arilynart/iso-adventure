using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Arilyn.State.EnemyState.Soldier;

public class EnemyController : MonoBehaviour
{
    public Animator animator;
    public GameObject attackPoint;
    public IEnemyStateMachine machine;

    public List<Collider> colliders = new List<Collider>();

    private void Awake()
    {
        animator = GetComponent<Animator>();
        machine = GetComponent<IEnemyStateMachine>();
    }

    public void Spawn()
    {
        //agent.enabled = true;
        gameObject.SetActive(true);
    }

    public void Attack()
    {
        if (machine.Stats.activeAttack == null) machine.Stats.ChooseAttack();

        machine.Stats.InitializeAttack();
        //Start attack animation
        machine.Stats.attack.ExecuteAttack();
    }

    public void ActivateAttack()
    {
        foreach (Collider col in colliders)
        {
            col.enabled = true;
        }
        Debug.Log("Hurtbox on!");
    }

    public void DeactivateAttack()
    {
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }
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
