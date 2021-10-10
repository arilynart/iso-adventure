using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPush : MonoBehaviour
{
    public Vector3 trajectory;
    public float movespeed;
    bool moving;

    private void Start()
    {
        trajectory = Vector3.zero;
    }

    void Update()
    {
        if (moving)
        {
            transform.position += trajectory * movespeed * Time.deltaTime;
        }
    }

    public void Slide(Vector3 direction)
    {
        trajectory = direction;

        moving = true;
    }
}
