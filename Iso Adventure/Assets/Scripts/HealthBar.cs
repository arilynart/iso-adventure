using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image[] life;
    public int healthPerLife = 3;

    void OnEnable()
    {
        PlayerHealth.MAX_HP = life.Length * healthPerLife;
        PlayerHealth.OnHealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= OnHealthChanged;
    }

    void OnHealthChanged(int hp)
    {
        Debug.Log("Updating Health Bar");
        int health = hp / healthPerLife; //divide current hp by how many containers we have to find the index of the hearth
        int lifeFill = hp % healthPerLife; // return remainder

        if(hp % healthPerLife == 0)
        {
            if (health == life.Length) // full health
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
}
