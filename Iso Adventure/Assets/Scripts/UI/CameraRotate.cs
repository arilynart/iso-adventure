using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public int rotState;
    public float turnSpeed = 0.02f;

    void Rotate(int state)
    {
        //lerp to the target rotation
        Quaternion targetRotation = Quaternion.Euler(transform.rotation.x, RotationScale(state), transform.rotation.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

        Debug.Log("Rotating: " + transform.rotation);
    }

    float RotationScale(int state)
    {
        switch(state)
        {
            case 0:
                return 40;
            case 1:
                return 130;
            case 2:
                return 220;
            case 3:
                return 310;
        }
        return 40;
    }
}
