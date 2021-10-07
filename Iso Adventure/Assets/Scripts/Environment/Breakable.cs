using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    int hp;
    public int maxHp = 3;

    private void Start()
    {
        hp = maxHp;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.GetComponent<AttackCollision>()) return;

        hp--;

        if (hp <=0)
        {
            Destroy(gameObject);
        }
    }
}
