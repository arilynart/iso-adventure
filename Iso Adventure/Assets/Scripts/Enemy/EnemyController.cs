using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Arilyn.State.EnemyState.Soldier;

public class EnemyController : MonoBehaviour
{
    public Animator animator;
    public EnemyStats stats;
    public GameObject hurtBox;
    public IEnemyStateMachine machine;

    private void Awake()
    {
        stats = GetComponent<EnemyStats>();
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

        machine.BackToChase();
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
