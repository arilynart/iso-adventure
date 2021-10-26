using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockReset : MonoBehaviour
{
    ITorchController controller;

    private void Start()
    {
        controller = transform.parent.GetComponent<ITorchController>();
    }

    public void Restart()
    {
        if (controller.CurrentTorches >= controller.RequiredTorches) return;

        foreach (Transform child in transform)
        {
            child.GetComponent<BlockPush>().Restart();
        }
    }
}
