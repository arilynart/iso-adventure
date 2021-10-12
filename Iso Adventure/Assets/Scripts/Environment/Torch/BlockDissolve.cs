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

        StartCoroutine(other.GetComponent<Dissolver>().ActivateDissolve(2, 0, 1, 0));
        other.GetComponent<BlockPush>().moving = false;
        Destroy(other.gameObject, 1);
        controller.LightTorch();
    }
}

