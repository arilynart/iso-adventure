using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Arilyn.State;

public interface IEnemyStateMachine
{
    float AttackDistance { get; set; }
    float AngleToPlayer { get; set; }
    bool CanSeePlayer { get; set; }
    bool Toggle { get; set; }
    bool Parryable { get; set; }
    Vector3 LookRotation { get; set; }
    NavMeshAgent Agent { get; set; }
    float Speed { get; set; }
    float Acceleration { get; set; }
    float StaggerDuration { get; set; }
    EnemyStats Stats { get; set; }
    Transform Transform { get; set; }
    Animator Animator { get; set; }
    EnemyController Controller { get; set; }

    IEnumerator ChangeState(State state);

    void Attack();

    void AttackBox(EnemyAttackSO attack);

    void Stagger();
}
