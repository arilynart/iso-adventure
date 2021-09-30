using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public GameObject Player;
    public float Distance;

    public bool aggro;

    public NavMeshAgent agent;
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        Distance = Vector3.Distance(Player.transform.position, this.transform.position);

        if(Distance <= 5)
        {
            aggro = true;
        }
        else
        {
            aggro = false;
        }

        if (aggro)
        {
            agent.isStopped = false;
            agent.SetDestination(Player.transform.position);
        }
        else
        {
            agent.isStopped = true;
        }
    }
}
