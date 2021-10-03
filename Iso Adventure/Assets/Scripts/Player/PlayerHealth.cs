using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Ludiq;
using Bolt;

public class PlayerHealth : MonoBehaviour
{
    PlayerController controller;
    PlayerCombat combat;
    PlayerDodge playerDodge;


    public System.DateTime invulnTime = System.DateTime.MinValue;

    public int hp;
    private static int max_Hp;
    public static int MAX_HP {
        get { return max_Hp; }
        set
        {
            value = Mathf.Clamp(value, 0, 100);
            max_Hp = value;
        }
    }
    public static int LIFE_UNLOCKED = 3;
    public int healValue = 1;
    public float invulnDuration = 1;

    public delegate void HealthBarDelegate(int hp);
    public static event HealthBarDelegate OnHealthChanged;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerController>();
        combat = GetComponent<PlayerCombat>();
        playerDodge = GetComponent<PlayerDodge>();

        hp = MAX_HP;
    }

    public void HealDamage(int amount)
    {
        //take damage
        hp += amount;
        HealthClamp();

        Debug.Log("Hp adjusted. Current HP: " + hp);
    }

    public void HealButton(/*InputAction.CallbackContext value*/)
    {
/*        if (value.started)
        {*/
            //take damage
            HealDamage(healValue);
/*        }*/
    }

    public void TakeDamage(int amount)
    {
        if (controller.invuln) return;

        Debug.Log("Taking damage");
        hp -= amount;
        HealthClamp();
        Debug.Log("Player is not invincible");

        if (hp <= 0)
        {
            CustomEvent.Trigger(gameObject, "PlayerDeath");
            Debug.Log("You are dead.");
        }
        else
        {
            //if (controller.dodge) return;
            StartCoroutine(Invulnerability(invulnDuration));
        }
        Debug.Log("Hp adjusted. Current HP: " + hp);
    }

    void HealthClamp()
    {
        hp = Mathf.Clamp(hp, 0, MAX_HP);
        OnHealthChanged(hp);
    }

    public IEnumerator Invulnerability(float duration)
    {
        if (System.DateTime.Now.AddSeconds(duration) < invulnTime) yield break;

        Debug.Log("Starting Invuln");
        controller.invuln = true;
        Physics.IgnoreLayerCollision(3, 7, true);
        Debug.Log("Invincible");
        invulnTime = System.DateTime.Now.AddSeconds(duration);

        yield return new WaitForSeconds(duration);

        Physics.IgnoreLayerCollision(3, 7, false);
        controller.invuln = false;
    }

}
