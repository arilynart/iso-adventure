using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.DeveloperConsole.Behavior;

public class CameraRotate : MonoBehaviour
{
    public int rotState;
    CameraFollow follow;
    float turnTime = 4f;

    public float rotation;

    Vector3 rot0 = new Vector3(-6, 5.7f, -6);
    Vector3 rot1 = new Vector3(-4.6f, 4.5f, 4.3f);
    Vector3 rot2 = new Vector3(5, 5, 5);
    Vector3 rot3 = new Vector3(-4, -2.4f, 4);

    private void Awake()
    {
        follow = GetComponent<CameraFollow>();
        rotation = 45;
        rotState = 0;
        follow.offset = rot0;
    }

    void Rotate()
    {
        //lerp to the target rotation
        Quaternion targetRotation = Quaternion.Euler(30, rotation, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.015f);

        Debug.Log("Rotating: " + transform.rotation);
    }

    public float RotationScale(int state)
    {
        switch(state)
        {
            case 0:
                follow.offset = rot0;
                rotState = 0;
                return 45;
            case 1:
                follow.offset = rot1;
                rotState = 1;
                return 135;
            case 2:
                follow.offset = rot2;
                rotState = 2;
                return 225;
            case 3:
                follow.offset = rot3;
                rotState = 3;
                return 315;
        }
        return 40;
    }

    public IEnumerator Rotation(int state)
    {
        if (rotState == state) yield break;
        rotation = RotationScale(state);
        float time = 0;
        while (time < turnTime)
        {
            Rotate();
            time += Time.deltaTime;
            yield return null;
        }
    }
}
