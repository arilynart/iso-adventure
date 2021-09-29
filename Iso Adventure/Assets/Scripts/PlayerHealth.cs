using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    PlayerController controller;
    PlayerCombat combat;

    public int hp = 10;
    public int maxhp = 10;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerController>();
        combat = GetComponent<PlayerCombat>();
    }

    public void AdjustHealth(int amount)
    {
        
        //take damage
        hp += amount;

        //can't go above maximum.
        if (hp > maxhp) hp = maxhp;
        Debug.Log("Hp adjusted. Current HP: " + hp);
        if (hp <= 0)
        {
            //can't go below zero, also dead
            hp = 0;
            Debug.Log("You are dead.");
        }
    }

}
