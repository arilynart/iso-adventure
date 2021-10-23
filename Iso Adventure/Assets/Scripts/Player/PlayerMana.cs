using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMana : MonoBehaviour
{
    public int mana;
    private static int max_Mana;
    public static int MAX_MANA
    {
        get { return max_Mana; }
        set
        {
            value = Mathf.Clamp(value, 0, 100);
            max_Mana = value;
        }
    }
    public static int MANA_UNLOCKED = 2;

    ManaBar manaBar;

    public delegate void ManaBarDelegate(int mana);
    public static event ManaBarDelegate OnManaChanged;


    // Start is called before the first frame update
    void Start()
    {
        manaBar = FindObjectOfType<ManaBar>();

        mana = MAX_MANA;
        manaBar.ResetBar();
    }

    public void AddMana(int amount)
    {
        //take damage
        mana += amount;
        ManaClamp();

        Debug.Log("Mana adjusted. Current Mana: " + mana);
    }

    void ManaClamp()
    {
        mana = Mathf.Clamp(mana, 0, MAX_MANA);
        OnManaChanged(mana);
    }


}
