using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowPost : MonoBehaviour
{
    public Material inactive;
    public Material active;
    public BoxCollider box;

    MeshRenderer mesh;

    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
        Time.timeScale = 1;
    }

    public void Activate()
    {
        if (box.enabled)
        {
            mesh.material = inactive;
            box.enabled = false;
            Time.timeScale = 1;
        }
        else
        {
            mesh.material = active;
            box.enabled = true;
        }
    }

    private void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.GetComponent<PlayerController>()) return;

        Time.timeScale = 0.4f;
    }

    private void OnTriggerExit(Collider other)
    {


       if (!other.GetComponent<PlayerController>()) return;

        Time.timeScale = 1;
    }
}
