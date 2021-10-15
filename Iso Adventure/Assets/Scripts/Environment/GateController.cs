using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    public GameObject door;
    public Transform doorPoint;
    Vector3 startPoint;
    public float gateSpeed = 3f;

    private void Start()
    {
        startPoint = door.transform.position;
    }
    public void Open()
    {
        StartCoroutine(Opening());
    }

    public void Close()
    {
        StartCoroutine(Closing());
    }

    IEnumerator Opening()
    {
        float time = 0;

        while (time < gateSpeed)
        { 
            door.transform.position = Vector3.Lerp(door.transform.position, doorPoint.position, time / gateSpeed);

            time += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator Closing()
    {
        Debug.Log("Closing Gate");
        float time = 0;

        while (time < gateSpeed)
        {
            door.transform.position = Vector3.Lerp(door.transform.position, startPoint, time / gateSpeed);

            time += Time.deltaTime;
            yield return null;
        }
    }
}
