using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int hp;
    public int maxHp;

    public EnemyAttack[] attacks;
    public EnemyAttack activeAttack;
    Rigidbody[] rigidBodies;

    

    private void Start()
    {
        rigidBodies = GetComponentsInChildren<Rigidbody>();
        DeactivateRagdoll(true);
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
        int r = Random.Range(0, attacks.Length - 1);
        activeAttack = attacks[r];

        Debug.Log("Chosen attack: " + activeAttack.name);
    }

    public void Die()
    {
        ActivateRagdoll();
        GetComponent<Collider>().enabled = false;
        Physics.IgnoreLayerCollision(7, 3, true);
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
