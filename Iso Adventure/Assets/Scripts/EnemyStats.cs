using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int hp;
    public int maxHp;

    public IEnemyAttack attack;
    public EnemyAttackSO[] attacks;
    public EnemyAttackSO[] lockedAttacks;
    public EnemyAttackSO activeAttack;

    Rigidbody[] rigidBodies;

    public string attackName;
    public string animationName;

    public int damage;
    public float boxStart;
    public float boxEnd;

    public int nextAttack;

    private void Start()
    {
        rigidBodies = GetComponentsInChildren<Rigidbody>();
        DeactivateRagdoll(true);
        attack = GetComponent<IEnemyAttack>();
    }

    public void InitializeAttack()
    {
        attackName = activeAttack.name;
        animationName = activeAttack.animationName;
        damage = activeAttack.damage;
        boxStart = activeAttack.boxStart;
        boxEnd = activeAttack.boxEnd;
        nextAttack = activeAttack.nextAttack;
        attack.InitializeAttack(attackName, animationName, damage, boxStart, boxEnd, nextAttack);
    }



    public void TakeDamage(int Amount)
    {
        hp -= Amount;
        Debug.Log("Enemy took damage: " + hp);
        if (hp <= 0)
        {
            Die();
            Debug.Log("Enemy is dead");
        }
    }

    public void ChooseAttack()
    {
        int r = Random.Range(0, attacks.Length);
        activeAttack = attacks[r];

        Debug.Log("Chosen attack: " + activeAttack.attackName);
    }

    public void Die()
    {
        ActivateRagdoll();
        GetComponent<Collider>().enabled = false;

        gameObject.layer = 9;
        foreach (Transform child in transform)
        {
            child.gameObject.layer = 9;
        }

    }

    public void DeactivateRagdoll(bool state)
    {
        foreach(var rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = state;
        }
        GetComponent<Animator>().enabled = true;
    }

    public void ActivateRagdoll()
    {
        foreach(var rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = false;
        }
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Animator>().enabled = false;
    }
}
