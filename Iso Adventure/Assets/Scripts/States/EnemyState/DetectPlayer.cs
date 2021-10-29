using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.DeveloperConsole.Behavior;

public class DetectPlayer : MonoBehaviour
{
    public IEnemyStateMachine machine;
    public GameObject attackPoint;

    public float viewAngle = 360;
    public float viewDistance = 15;

    float distanceToPlayer;

    private void Start()
    {
        machine = transform.parent.GetComponent<IEnemyStateMachine>();
        attackPoint = transform.parent.GetComponent<EnemyController>().attackPoint;
    }

    private void Update()
    {
        distanceToPlayer = Vector3.Distance(attackPoint.transform.position, DeveloperConsoleBehavior.PLAYER.transform.position);
        machine.AttackDistance = distanceToPlayer;
        if (machine.AngleToPlayer < viewAngle && distanceToPlayer < viewDistance)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + new Vector3(0, 0.2f, 0), machine.LookRotation, out hit, viewDistance * 1.2f))
            {
                if (hit.transform.GetComponent<PlayerController>())
                {
                    machine.CanSeePlayer = true;
                }
            }
        }
        else
        {
            machine.CanSeePlayer = false;
        }
    }

    private void OnDestroy()
    {
        enabled = false;
    }
}
