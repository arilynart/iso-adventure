using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockButton : MonoBehaviour
{
    ITorchController controller;
    public bool activated;

    private void Start()
    {
        controller = transform.parent.GetComponent<ITorchController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<BlockPush>()) return;
        activated = true;
        Activate();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<BlockPush>()) return;
        activated = false;
        if (controller.CurrentTorches >= controller.RequiredTorches)
        {
            controller.CurrentTorches = 0;
            controller.CloseTorch();
        }
    }

    public void Activate()
    {
        controller.LightTorch();
    }
}
