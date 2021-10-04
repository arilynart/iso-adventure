using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Ludiq;
using Bolt;
using Arilyn.DeveloperConsole.Behavior;

public class BullAttack : MonoBehaviour, IEnemyAttack
{

    NavMeshAgent agent;

    string attackName;
    string animationName;

    bool rotating;

    int damage;
    float range;
    float boxStart;
    float boxEnd;

    int nextAttack;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
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
        Debug.Log("Executing attack: " + attackName + " " + animationName + " " + damage + " " + boxStart + " " + boxEnd + " " + nextAttack + " ");

        if (attackName == "Attack_02" || attackName == "Attack_03")
        {
            Debug.Log("BullAttack: Rotating Attack");

        }
        else
        {

        }
    }


}
