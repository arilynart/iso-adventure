using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    PlayerController controller;
    PlayerMana mana;
    HealthBar healthBar;

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
    public int healValue = 2;
    public int healCost = 4;
    public float invulnDuration = 1;

    public delegate void HealthBarDelegate(int hp);
    public static event HealthBarDelegate OnHealthChanged;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerController>();
        mana = GetComponent<PlayerMana>();
        healthBar = FindObjectOfType<HealthBar>();

        hp = MAX_HP;
        healthBar.ResetBar();
        
    }

    public void HealDamage(int amount)
    {
        //take damage
        hp += amount;
        HealthClamp();

        Debug.Log("Hp adjusted. Current HP: " + hp);
    }

    public void TakeDamage(int amount)
    {
        if (controller.invuln)
        {
            Debug.Log("Contorller is invuln, no damage");
            return;
        }

        Debug.Log("Taking damage");
        hp -= amount;
        HealthClamp();
        Debug.Log("Player is not invincible");

        if (hp <= 0)
        {
            controller.machine.Death();
            
            Debug.Log("You are dead.");
            string scene = SceneManager.GetActiveScene().name;
            CameraFollow.LOCK = false;
            FadeToBlack.FADEOUT(scene);
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

        Debug.Log("Invincible");
        invulnTime = System.DateTime.Now.AddSeconds(duration);

        yield return new WaitForSeconds(duration);


        controller.invuln = false;
    }

}
