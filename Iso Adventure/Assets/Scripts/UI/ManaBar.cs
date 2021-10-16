using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Arilyn.DeveloperConsole.Behavior;

public class ManaBar : MonoBehaviour
{
    public Image[] containers;
    public int manaPerContainer = 4;

    private void Start()
    {
        ResetBar();
    }

    void OnEnable()
    {
        PlayerMana.OnManaChanged += OnManaChanged;
    }

    private void OnDisable()
    {
        PlayerMana.OnManaChanged -= OnManaChanged;
    }

    void OnManaChanged(int mp)
    {
        Debug.Log("Updating Health Bar");
        int manaC = mp / manaPerContainer; //divide current hp by how many containers we have to find the index of the heart
        int manaFill = mp % manaPerContainer; // return remainder

        int i = 0;
        foreach (Image img in containers)
        {

            if (i > manaC)
            {
                containers[i].fillAmount = 0;
            }
            else
            {
                containers[i].fillAmount = 1;
            }
            i++;
        }
        if (mp % manaPerContainer == 0)
        {
            if (manaC == PlayerHealth.LIFE_UNLOCKED) // full health
            {
                containers[manaC - 1].fillAmount = 1;
                return;
            }

            if (manaC > 0) //anything but zero health
            {
                containers[manaC].fillAmount = 0;
                containers[manaC - 1].fillAmount = 1;
            }
            else // 0 health
            {
                containers[manaC].fillAmount = 0;
            }

            return;
        }

        containers[manaC].fillAmount = manaFill / (float)manaPerContainer;


    }

    public void ResetBar()
    {
        PlayerMana.MAX_MANA = PlayerMana.MANA_UNLOCKED * manaPerContainer;
        DeveloperConsoleBehavior.PLAYER.mana.mana = PlayerMana.MAX_MANA;
        int i = 0;
        foreach (Image img in containers)
        {
            if (i < PlayerMana.MANA_UNLOCKED)
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
