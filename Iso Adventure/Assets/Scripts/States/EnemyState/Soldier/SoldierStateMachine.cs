using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Arilyn.DeveloperConsole.Behavior;
using Arilyn.State;
using Arilyn.State.EnemyState.Soldier;

public class SoldierStateMachine : MonoBehaviour, IEnemyStateMachine
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

    private  float angleToPlayer;
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
        set
        {
            toggle = value;
            //Debug.Log(name + " Toggle: " + Toggle);
        }
    }
    private bool parryable;
    public bool Parryable
    {
        get => parryable;
        set => parryable = value;
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

    public GameObject attackBox;

    private void Start()
    {
        player = DeveloperConsoleBehavior.PLAYER;
        Animator = GetComponent<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        Controller = GetComponent<EnemyController>();
        Stats = GetComponent<EnemyStats>();
        Transform = transform;
        Acceleration = 8;
        Speed = 1.1f;
        StaggerDuration = 3f;
        Toggle = false;
    }

    private void Update()
    {
        if (stats.dead)
        {
            StartCoroutine(ChangeState(new DeadState(this)));
        }
        LookRotation = player.transform.position - transform.position;
        angleToPlayer = Vector3.Angle(transform.forward, LookRotation);
        if (currentState != null) currentState.LocalUpdate();
    }

    private void OnEnable()
    {
        StartCoroutine(ChangeState(new WanderState(this)));
    }

    public IEnumerator ChangeState(State state)
    {
        Debug.Log(name + " Changing State. " + state);
        if (Toggle) yield break;
        Toggle = true;
        Debug.Log(name + " No toggle.");
        if (currentState != null) 
            yield return StartCoroutine(currentState.ExitState());
        currentState = state;
        yield return null;
        Toggle = false;
        StartCoroutine(currentState.EnterState());

        Debug.Log("Soldier: Current State " + currentState);
    }

    public void Attack()
    {
        StartCoroutine(AttackAnimation(Stats.boxStart, Stats.boxEnd));
    }

    IEnumerator AttackAnimation(float attackStart, float attackEnd)
    {
        Animator.SetTrigger(Stats.animationName);
        AttackBox(Stats.activeAttack);
        float time = 0;
        while (time < attackStart)
        {
            Quaternion lookRotation = Quaternion.LookRotation(LookRotation);
            Transform.rotation = Quaternion.Slerp(Transform.rotation, lookRotation, 5 * Time.deltaTime);

            time += Time.deltaTime;
            yield return null;
        }
        Controller.ActivateAttack();
        Parryable = true;
        while (time < attackStart + DeveloperConsoleBehavior.PLAYER.machine.parryDuration)
        {
            time += Time.deltaTime;
            yield return null;
        }
        Parryable = false;
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
        Controller.colliders.Add(attackBox.GetComponent<Collider>());
    }

    public void Stagger()
    {
        Debug.Log("Stagger soldier.");
        Parryable = false;
        StopAllCoroutines();
        Toggle = false;
        StartCoroutine(ChangeState(new StaggerState(this)));
    }
}