using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Arilyn.DeveloperConsole.Behavior;
using Arilyn.State;
using Arilyn.State.EnemyState.Soldier;

public class EnemyStateMachine : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;
    public EnemyController controller;
    public EnemyStats stats;

    public float angleToPlayer;
    public float attackDistance;
    public Vector3 directionToPlayer;
    public Vector3 lookRotation;
    public bool canSeePlayer;
    public PlayerController player;

    public State currentState;

    private void Start()
    {
        player = DeveloperConsoleBehavior.PLAYER;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<EnemyController>();
        stats = GetComponent<EnemyStats>();

        ChangeState(new WanderState(this));
    }

    public void ChangeState(State state)
    {
        if (currentState != null) StartCoroutine(currentState.ExitState());
        currentState = state;
        StartCoroutine(currentState.EnterState());
    }

    private void Update()
    {
        if (stats.dead)
        {
            ChangeState(new DeadState(this));
        }
        lookRotation = player.transform.position - transform.position;
        angleToPlayer = Vector3.Angle(transform.forward, lookRotation);
        currentState.LocalUpdate();
    }
}

