using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.DeveloperConsole.Behavior;

public class BlockGateController : MonoBehaviour, ITorchController
{
    [SerializeField]
    private int requiredTorches;
    public int RequiredTorches
    {
        get { return requiredTorches;  }
        set { requiredTorches = value;  }
    }
    private int currentTorches;
    public int CurrentTorches
    {
        get { return currentTorches; }
        set { currentTorches = value; }
    }

    public bool open;

    [SerializeField]
    private GameObject gate;
    private GateController gateController;

    void Start()
    {
        CurrentTorches = 0;
        gateController = gate.GetComponent<GateController>();
    }

    public void LightTorch()
    {
        CurrentTorches = 0;
        Debug.Log("torch lit");
        foreach (Transform child in transform)
        {
            if (child.GetComponent<BlockButton>())
            {
                if (child.GetComponent<BlockButton>().activated)
                {
                    CurrentTorches++;
                }
            }
        }
        if (CurrentTorches >= RequiredTorches)
        {
            FullTorch();
        }
        
    }

    public void FullTorch()
    {
        open = true;
        Camera.main.GetComponent<Cutscene>().CutsceneStart(gate.transform, 3f);
        StartCoroutine(GateDelay());
    }

    IEnumerator GateDelay()
    {
        yield return new WaitForSeconds(1);
        gateController.Open();
    }

    public void CloseTorch()
    {
        gateController.Close();
    }
}
