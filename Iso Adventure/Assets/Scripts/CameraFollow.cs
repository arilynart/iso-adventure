using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    //public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public float lerpTime = 0.1f;

    void LateUpdate()
    {
        //Camera follows player
        Vector3 targetPosition = target.position + offset;

        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpTime);
    }
}
