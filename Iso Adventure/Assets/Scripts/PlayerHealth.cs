using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    PlayerController controller;
    PlayerCombat combat;

    public int hp = 10;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerController>();
        combat = GetComponent<PlayerCombat>();
    }

    public void TakeDamage(int amount)
    {
        hp -= amount;
        Debug.Log("Damage. Remaining HP: " + hp);
    }

}
