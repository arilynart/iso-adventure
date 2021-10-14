using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public static Transform LOCKTARGET;
    public static Camera MAINCAMERA;

    Vector3 camPosition;
    Vector3 targetPosition;

    public static bool LOCK;

    //public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public float lerpTime = 0.1f;

    private void Start()
    {
        MAINCAMERA = Camera.main;
    }

    void LateUpdate()
    {
        if (!FadeToBlack.FADEAWAY)
        {
            targetPosition = CalcPosition();
        }
        //Camera follows player
        camPosition = targetPosition + offset;

        transform.position = Vector3.Lerp(transform.position, camPosition, lerpTime);
    }

    Vector3 CalcPosition()
    {
        if (LOCK)
        {
            Vector3 dist = LOCKTARGET.position - target.position;
            return target.position + (dist * 0.5f);
        }
        return target.position;
    }
}
