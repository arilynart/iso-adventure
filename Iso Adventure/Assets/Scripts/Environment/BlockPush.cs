using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.DeveloperConsole.Behavior;

public class BlockPush : MonoBehaviour
{
    public Vector3 trajectory;
    public float movespeed;
    public bool moving;
    Vector3 startPos;

    private void Start()
    {
        trajectory = Vector3.zero;
        startPos = transform.position;
    }

    void FixedUpdate()
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
        if (Physics.Raycast(transform.position, trajectory, 0.7f, DeveloperConsoleBehavior.PLAYER.ground)) return;

        moving = true;
    }

    Vector3 LockDirection(Vector3 dir)
    {
        Debug.Log("Sliding block.");
        float dirX = 0;
        float dirZ = 0;

        float fwdDot = Vector3.Dot(dir, Vector3.forward);
        float rightDot = Vector3.Dot(dir, Vector3.right);
        float fwdPwr = Mathf.Abs(fwdDot);
        float rightPwr = Mathf.Abs(rightDot);
        if (rightPwr > fwdPwr) {
            if (rightDot > 0)
            {
                dirX = -1;
                dirZ = 0;
            }
            else
            {
                dirX = 1;
                dirZ = 0;
            }
        }
        else if(fwdPwr > rightPwr)
        {
            if (fwdDot > 0)
            {
                dirZ = -1;
                dirX = 0;
            }
            else
            {
                dirZ = 1;
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
