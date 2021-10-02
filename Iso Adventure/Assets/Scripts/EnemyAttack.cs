using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyAttack", order = 1)]
public class EnemyAttack : ScriptableObject
{
    public string animation;
    //public string windup;

    public int damage;
}