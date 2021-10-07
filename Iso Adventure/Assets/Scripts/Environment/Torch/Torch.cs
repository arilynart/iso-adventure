using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public GameObject fire;
    ITorchController controller;
    bool lit;

    private void Start()
    {
        controller = transform.parent.transform.parent.GetComponent<ITorchController>();
        lit = false;
    }

    public void EnableTorch()
    {
        if (lit) return;

        fire.SetActive(true);

        controller.LightTorch();
        lit = true;
    }
}

