using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Arilyn.DeveloperConsole.Behavior;
using Arilyn.State;
using Arilyn.State.EnemyState.Golem;

public class GolemStateMachine : MonoBehaviour, IEnemyStateMachine
{
    private Animator animator;
    public Animator Animator
    {
        get => animator;
        set => animator = value;
    }
    private NavMeshAgent agent;
    public NavMeshAgent Agent
    {
        get => agent;
        set => agent = value;
    }
    private EnemyController controller;
    public EnemyController Controller
    {
        get => controller;
        set => controller = value;
    }
    private EnemyStats stats;
    public EnemyStats Stats
    {
        get => stats;
        set => stats = value;
    }
    private Transform transformer;
    public Transform Transform
    {
        get => transformer;
        set => transformer = value;
    }

    private float angleToPlayer;
    public float AngleToPlayer
    {
        get => angleToPlayer;
        set => angleToPlayer = value;
    }
    private float attackDistance;
    public float AttackDistance
    {
        get => attackDistance;
        set => attackDistance = value;
    }
    public Vector3 directionToPlayer;
    private Vector3 lookRotation;
    public Vector3 LookRotation
    {
        get => lookRotation;
        set => lookRotation = value;
    }
    private bool canSeePlayer;
    public bool CanSeePlayer
    {
        get => canSeePlayer;
        set => canSeePlayer = value;
    }
    private bool toggle;
    public bool Toggle
    {
        get => toggle;
        set => toggle = value;
    }
    public PlayerController player;

    private float acceleration;
    public float Acceleration
    {
        get => acceleration;
        set => acceleration = value;
    }
    private float speed;
    public float Speed
    {
        get => speed;
        set => speed = value;
    }
    private float staggerDuration;
    public float StaggerDuration
    {
        get => staggerDuration;
        set => staggerDuration = value;
    }
    private State currentState;

    public GameObject FrontSphere;
    public GameObject ArmL;
    public GameObject ArmR;

    private void Start()
    {
        player = DeveloperConsoleBehavior.PLAYER;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<EnemyController>();
        stats = GetComponent<EnemyStats>();
        Transform = transform;
        Acceleration = 8;
        Speed = 2.6f;
        Toggle = false;

        StartCoroutine(ChangeState(new WanderState(this)));
    }

    private void Update()
    {
        if (stats.dead)
        {
            StartCoroutine(ChangeState(new DeadState(this)));
        }
        lookRotation = player.transform.position - transform.position;
        angleToPlayer = Vector3.Angle(transform.forward, lookRotation);
        if (currentState != null) currentState.LocalUpdate();
    }

    public IEnumerator ChangeState(State state)
    {
        if (Toggle) yield break;
        Toggle = true;
        if (currentState != null)
        {
            yield return StartCoroutine(currentState.ExitState());
            //StartCoroutine(currentState.ExitState());
        }
        currentState = state;
        yield return null;
        Toggle = false;
        StartCoroutine(currentState.EnterState());
        
        Debug.Log("Golem: Current State " + currentState);
        
    }

    public void BackToChase()
    {
        StartCoroutine(ChangeState(new ChaseState(this)));
    }

    public void Attack()
    {
        StartCoroutine(AttackAnimation(Stats.boxStart, Stats.boxEnd));
    }

    IEnumerator AttackAnimation(float attackStart, float attackEnd)
    {
        AttackBox(Stats.activeAttack);
        float time = 0;
        while (time < attackStart)
        {
            time += Time.deltaTime;
            yield return null;
        }
        Controller.ActivateAttack();
        while (time < attackEnd)
        {
            time += Time.deltaTime;
            yield return null;
        }
        Controller.DeactivateAttack();
        while (time < Animator.GetCurrentAnimatorStateInfo(0).length)
        {
            time += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(ChangeState(new ChaseState(this)));

    }

    public void AttackBox(EnemyAttackSO attack)
    {
        Controller.colliders.Clear();
        if (attack == Stats.attacks[0])
            Controller.colliders.Add(ArmL.GetComponent<Collider>());
        else if (attack == Stats.lockedAttacks[0])
            Controller.colliders.Add(ArmR.GetComponent<Collider>());
        else if (attack == Stats.lockedAttacks[1])
        {
            Controller.colliders.Add(ArmL.GetComponent<Collider>());
            Controller.colliders.Add(ArmR.GetComponent<Collider>());
        }
        else
            Controller.colliders.Add(FrontSphere.GetComponent<Collider>());
    }

    public void Stagger()
    {
        //if the stagger bar is full then stun the boss.
    }
}

