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
    Transform player;

    

    private void Start()
    {
        rigidBodies = GetComponentsInChildren<Rigidbody>();
        DeactivateRagdoll(true);
        player = PlayerManager.instance.player.transform;
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
        Vector3 direction = player.position - transform.position;
        Debug.Log("Direction " + direction);
        ActivateRagdoll(-direction * 5);
        GetComponent<Collider>().enabled = false;
        gameObject.layer = 9;
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
