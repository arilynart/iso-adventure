using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.DeveloperConsole.Behavior;

public class EnemyStats : MonoBehaviour
{
    public int hp;
    public int maxHp;

    public IEnemyAttack attack;
    public EnemyAttackSO[] attacks;
    public EnemyAttackSO[] lockedAttacks;
    public EnemyAttackSO activeAttack;

    Rigidbody[] rigidBodies;
    Transform player;

    public string attackName;
    public string animationName;

    public int damage;
    public float range;
    public float boxStart;
    public float boxEnd;

    public int nextAttack;

    private void Start()
    {
        rigidBodies = GetComponentsInChildren<Rigidbody>();
        DeactivateRagdoll(true);
        attack = GetComponent<IEnemyAttack>();
        player = DeveloperConsoleBehavior.PLAYER.transform;

        activeAttack = attacks[0];
    }

    public void InitializeAttack()
    {
        attackName = activeAttack.name;
        animationName = activeAttack.animationName;
        damage = activeAttack.damage;
        range = activeAttack.range;
        boxStart = activeAttack.boxStart;
        boxEnd = activeAttack.boxEnd;
        nextAttack = activeAttack.nextAttack;
        attack.InitializeAttack(attackName, animationName, damage, range, boxStart, boxEnd, nextAttack);
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

    public void NextAttack()
    {
        if (activeAttack.nextAttack >= 0)
            activeAttack = lockedAttacks[activeAttack.nextAttack];
        else if (activeAttack.nextAttack <= -2)
        {
            activeAttack = attacks[0];
        } 
        else
            activeAttack = null;
    }

    public void Die()
    {
        Vector3 direction = player.position - transform.position;
        Debug.Log("Direction " + direction);
        ActivateRagdoll(-direction * 5);
        GetComponent<Collider>().enabled = false;

        if (IndoorTrigger.INDOORS)
            gameObject.layer = 12;
        else
            gameObject.layer = 9;

        EnemyEncounter.DEATHCOUNT++;

        //Destroy(gameObject, 4);
    }

    public void DeactivateRagdoll(bool state)
    {
        foreach(var rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = state;
        }
        GetComponent<Animator>().enabled = true;
    }

    public void ActivateRagdoll(Vector3 direction)
    {
        foreach(var rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = false;
            rigidBody.AddForce(direction, ForceMode.VelocityChange);
        }
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Animator>().enabled = false;
    }
}
