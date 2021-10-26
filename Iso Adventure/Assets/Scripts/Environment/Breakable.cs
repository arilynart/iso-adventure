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

    public void Hit()
    {
        hp--;

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
