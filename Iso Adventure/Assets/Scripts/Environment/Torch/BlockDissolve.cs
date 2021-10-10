using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDissolve : MonoBehaviour
{
    ITorchController controller;

    private void Start()
    {
        controller = transform.parent.GetComponent<ITorchController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<BlockPush>()) return;

        other.gameObject.SetActive(false);
        controller.LightTorch();
    }
}

