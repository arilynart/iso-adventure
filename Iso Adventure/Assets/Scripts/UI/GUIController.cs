using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour
{

    public static GUIController CONTROLLER;

    private void Awake()
    {
        if (CONTROLLER != null && CONTROLLER != this)
        {
            Destroy(gameObject);
            return;
        }

        CONTROLLER = this;
        DontDestroyOnLoad(gameObject);
    }
}
