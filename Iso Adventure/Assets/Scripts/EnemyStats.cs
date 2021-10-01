using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyController controller;
    public int hp;
    public int maxHp;

    //public List<EnemyAttack> attacks;
    public EnemyAttack[] attacks;
    public EnemyAttack activeAttack;

    private void Start()
    {
        controller = GetComponent<EnemyController>();
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

    void Die()
    {
        controller.setRigidbodyState(false);
        controller.setColliderState(true);
        controller.agent.enabled = false;
        GetComponent<Animator>().enabled = false;
        GetComponent<EnemyController>().enabled = false;

        Destroy(this.gameObject, 10f);
    }
}
