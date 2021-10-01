using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int hp;
    public int maxHp;

    //public List<EnemyAttack> attacks;
    public EnemyAttack[] attacks;
    public EnemyAttack activeAttack;

    private void Start()
    {
        
    }

    public void TakeDamage(int Amount)
    {
        hp -= Amount;
    }

    public void ChooseAttack()
    {
        int r = Random.Range(0, attacks.Length - 1);
        activeAttack = attacks[r];

        Debug.Log("Chosen attack: " + activeAttack.name);
    }
}
