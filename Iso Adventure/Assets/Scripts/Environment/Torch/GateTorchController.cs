using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.DeveloperConsole.Behavior;

public class GateTorchController : MonoBehaviour, ITorchController
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

    [SerializeField]
    private GameObject gate;

    void Start()
    {
        CurrentTorches = 0;
    }

    public void LightTorch()
    {
        Debug.Log("torch lit");
        CurrentTorches++;
        if (CurrentTorches >= RequiredTorches)
        {
            FullTorch();
        }

    }

    public void FullTorch()
    {
        CameraFollow.MAINCAMERA.GetComponent<Cutscene>().CutsceneStart(gate.transform, 3f);
        StartCoroutine(GateDelay());
    }

    IEnumerator GateDelay()
    {
        yield return new WaitForSeconds(1);
        gate.GetComponent<GateController>().Open();
    }

    public void CloseTorch()
    {

    }

}
