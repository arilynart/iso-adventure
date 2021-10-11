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
        other.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
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
