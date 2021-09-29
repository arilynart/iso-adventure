using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    PlayerController controller;
    PlayerCombat combat;

    public System.DateTime invulnTime = System.DateTime.MinValue;

    public int hp = 10;
    public int maxhp = 10;
    public float invulnDuration = 1;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerController>();
        combat = GetComponent<PlayerCombat>();
    }

    public void HealDamage(int amount)
    {
        //take damage
        hp += amount;
        //can't go above maximum.
        if (hp > maxhp) hp = maxhp;

        Debug.Log("Hp adjusted. Current HP: " + hp);
    }

    public void TakeDamage(int amount)
    {
        if (controller.invuln || controller.dodge) return;

        Debug.Log("Taking damage");
        hp -= amount;

        Debug.Log("Player is not invincible");

        if (hp <= 0)
        {
            //can't go below zero, also dead
            hp = 0;
            Debug.Log("You are dead.");
        }
        else
        {
            if (controller.dodge) return;
            controller.dodge = false;
            StartCoroutine(Invulnerability(invulnDuration));
        }
        Debug.Log("Hp adjusted. Current HP: " + hp);
    }

    public IEnumerator Invulnerability(float duration)
    {
        Debug.Log("Starting Invuln");
        if (System.DateTime.Now.AddSeconds(duration) < invulnTime) yield break;


        controller.invuln = true;
        Physics.IgnoreLayerCollision(3, 7, true);
        Debug.Log("Invincible");
        invulnTime = System.DateTime.Now.AddSeconds(duration);

        yield return new WaitForSeconds(duration);

        Physics.IgnoreLayerCollision(3, 7, false);
        controller.invuln = false;

    }

}
