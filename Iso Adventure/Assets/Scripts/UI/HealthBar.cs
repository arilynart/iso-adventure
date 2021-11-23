using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Arilyn.DeveloperConsole.Behavior;

public class HealthBar : MonoBehaviour
{
    public Image[] life;
    public int healthPerLife = 3;



    private void Start()
    {
        ResetBar();
    }

    void OnEnable()
    {
        PlayerHealth.OnHealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= OnHealthChanged;
    }

    void OnHealthChanged(int hp)
    {
        Debug.Log("Updating Health Bar");
        int health = hp / healthPerLife; //divide current hp by how many containers we have to find the index of the heart
        int lifeFill = hp % healthPerLife; // return remainder

        int i = 0;
        foreach (Image img in life)
        {

            if (i > health)
            {
                life[i].fillAmount = 0;
            }
            else
            {
                life[i].fillAmount = 1;
            }
            i++;
        }
        if (hp % healthPerLife == 0)
        {
            if (health == PlayerHealth.LIFE_UNLOCKED) // full health
            {
                life[health - 1].fillAmount = 1;
                return;
            }

            if (health > 0) //anything but zero health
            {
                life[health].fillAmount = 0;
                life[health - 1].fillAmount = 1;
            }
            else // 0 health
            {
                life[health].fillAmount = 0;
            }

            return;
        } 

        life[health].fillAmount = lifeFill / (float)healthPerLife;

        
    }
    
    public void ResetBar()
    {
        PlayerHealth.MAX_HP = PlayerHealth.LIFE_UNLOCKED * healthPerLife;
        DeveloperConsoleBehavior.PLAYER.health.hp = PlayerHealth.MAX_HP;
        int i = 0;
        foreach (Image img in life)
        {
            if (i < PlayerHealth.LIFE_UNLOCKED)
            {
                img.transform.parent.gameObject.SetActive(true);
                img.fillAmount = 1;
            }
            else
                img.transform.parent.gameObject.SetActive(false);
            i++;
        }
    }
}
