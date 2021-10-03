using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    public GameObject door;
    public Transform doorPoint;
    public float gateSpeed = 3f;
    public void Open()
    {
        StartCoroutine(Opening());
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
}
