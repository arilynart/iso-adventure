using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterAttack : MonoBehaviour, IEnemyAttack
{
    string attackName;
    string animationName;

    int damage;
    float range;
    float boxStart;
    float boxEnd;

    int nextAttack;

    ShooterStateMachine machine;

    private void Start()
    {
        machine = GetComponent<ShooterStateMachine>();
    }

    public void InitializeAttack(string name, string anim, int dmg, float rng, float start, float end, int next)
    {
        attackName = name;
        animationName = anim;
        damage = dmg;
        range = rng;
        boxStart = start;
        boxEnd = end;
        nextAttack = next;
    }

    public void ExecuteAttack()
    {
        Debug.Log("Initializing attack: " + attackName + " " + animationName + " " + damage + " " + boxStart + " " + boxEnd + " " + nextAttack + " ");
        machine.looking = true;
    }

}
