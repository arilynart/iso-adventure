using UnityEngine;
using Ludiq;
using Bolt;

public class PlayerCollision : MonoBehaviour
{
    PlayerController controller;
    PlayerHealth health;
    PlayerDodge dodge;

    RaycastHit hitInfo;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        health = GetComponent<PlayerHealth>();
        dodge = GetComponent<PlayerDodge>();
    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    private void OnCollisionStay(Collision collision)
    {
        controller.collision = true;
        //Debug.Log("Colliding");

/*        if (collision.collider.GetComponent<EnemyStats>())
        {
            Debug.Log("PlayerCollision: enemy collision");

            Vector3 storedPosition = transform.position;

            transform.position -= controller.point * 2;
            if (GetComponent<Rigidbody>().SweepTest(controller.point, out hitInfo, 1f))
            {
                Debug.Log("PlayerCollision: Forward Sweep Test Complete");
                transform.position = storedPosition;
                //if we are inside an enemy
                TestBounds();
            }
            else
                transform.position = storedPosition;

        }*/
    }

/*    private void TestBounds()
    {
        if (hitInfo.collider.bounds.Contains(transform.position)*//* || hitInfo.collider.bounds.Contains(transform.position + new Vector3(0, -1, 0))*//*)
        {

            Debug.Log("PlayerCollision: Inside enemy!");
            transform.position = hitInfo.collider.ClosestPoint(transform.forward) + new Vector3(0, 1, 0);
        }
    }*/

    private void OnCollisionExit(Collision collision)
    {
        controller.collision = false;
        Debug.Log("Stop Colliding");

    }
}