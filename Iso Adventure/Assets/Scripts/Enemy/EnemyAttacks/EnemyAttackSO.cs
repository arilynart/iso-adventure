using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttack", menuName = "ScriptableObjects/EnemyAttack", order = 1)]
public class EnemyAttackSO : ScriptableObject
{
    public string attackName;
    public string animationName;

    public int damage;
    public float range;
    public float boxStart;
    public float boxEnd;

    public int nextAttack;

    public bool parryable;
}
