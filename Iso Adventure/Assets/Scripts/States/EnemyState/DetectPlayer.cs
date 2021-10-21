using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.DeveloperConsole.Behavior;

public class DetectPlayer : MonoBehaviour
{
    public EnemyStateMachine machine;
    public Transform attackPoint;

    public float viewAngle = 360;
    public float viewDistance = 15;

    float distanceToPlayer;

    private void Start()
    {
        machine = transform.parent.GetComponent<EnemyStateMachine>();
    }

    private void Update()
    {
        distanceToPlayer = Vector3.Distance(attackPoint.position, DeveloperConsoleBehavior.PLAYER.transform.position);
        machine.attackDistance = distanceToPlayer;
        if (machine.angleToPlayer < viewAngle && distanceToPlayer < viewDistance)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + new Vector3(0, 0.2f, 0), machine.lookRotation, out hit, 20f))
            {
                if (hit.transform.GetComponent<PlayerController>())
                {
                    machine.canSeePlayer = true;
                }
            }
        }
        else
        {
            machine.canSeePlayer = false;
        }
    }

    private void OnDestroy()
    {
        enabled = false;
    }
}
