using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.DeveloperConsole.Behavior;

public class PlayerManager : MonoBehaviour
{
    //Keeps track of player position for Enemy AI
    #region Singleton

    public static PlayerManager instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    public GameObject player;

    private void Start()
    {
        player = DeveloperConsoleBehavior.PLAYER.gameObject;
    }
}
