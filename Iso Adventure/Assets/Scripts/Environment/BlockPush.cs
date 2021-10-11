using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.DeveloperConsole.Behavior;

public class BlockPush : MonoBehaviour
{
    public Vector3 trajectory;
    public float movespeed;
    bool moving;
    Rigidbody rb;
    Vector3 startPos;

    private void Start()
    {
        trajectory = Vector3.zero;
        startPos = transform.position;
    }

    void Update()
    {
        if (moving)
        {
            if (Physics.Raycast(transform.position, trajectory, 0.6f, DeveloperConsoleBehavior.PLAYER.ground)) return;
            transform.position += trajectory * movespeed * Time.deltaTime;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        moving = false;
        trajectory = Vector3.zero;
    }

    public void Slide(Vector3 direction)
    {
        trajectory = LockDirection(direction);
        if (Physics.Raycast(transform.position, trajectory, 0.6f, DeveloperConsoleBehavior.PLAYER.ground)) return;

        moving = true;
    }

    Vector3 LockDirection(Vector3 dir)
    {
        float dirX = 0;
        float dirZ = 0;
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z)) {
            if (dir.x > 0)
            {
                dirX = 1;
                dirZ = 0;
            }
            else
            {
                dirX = -1;
                dirZ = 0;
            }
        }
        else if(Mathf.Abs(dir.z) > Mathf.Abs(dir.x))
        {
            if (dir.z > 0)
            {
                dirZ = 1;
                dirX = 0;
            }
            else
            {
                dirZ = -1;
                dirX = 0;
            }
        }
        return new Vector3(dirX, 0, dirZ);
    }

    public void Restart()
    {
        transform.position = startPos;
        gameObject.SetActive(true);
        moving = false;
        trajectory = Vector3.zero;
    }
}
