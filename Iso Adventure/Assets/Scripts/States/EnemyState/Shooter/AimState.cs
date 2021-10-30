using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Arilyn.DeveloperConsole.Behavior;

namespace Arilyn.State.EnemyState.Shooter
{
    public class AimState : EnemyState
    {
        public AimState(IEnemyStateMachine mch) : base(mch) { }

        public override IEnumerator EnterState()
        {
            machine.Agent.speed = 5;
            if (machine.AttackDistance <= machine.Stats.range)
            {
                if (!toggle)
                {
                    //Vector3 targetRot = Vector3.RotateTowards(machine.transform.forward, machine.lookRotation, 4 * Time.deltaTime, 0);
                    CalculatePosition();
                    toggle = true;
                }
            }
            else
            {
                machine.Controller.StartCoroutine(machine.ChangeState(new ChaseState(machine)));
            }
            yield break;
        }

        bool toggle;

        public override void LocalUpdate()
        {

            if (!machine.Agent.pathPending && machine.Agent.remainingDistance <= 0.1f && toggle)
            {
                machine.Controller.StartCoroutine(machine.ChangeState(new AttackState(machine)));
            }
        }

        void CalculatePosition()
        {
            //Generate Random Point between 0 and 1 for X value
            float x = machine.Transform.position.x;
            float y = machine.Transform.position.y;
            Vector2 position = CameraFollow.MAINCAMERA.WorldToViewportPoint(machine.Transform.position);
            Debug.Log("Shooter Position: " + position);
            //if x is in the middle half the screen

            //if we're on the right half
            if (position.x > 0.5f)
            {
                //select a random x value on this half
                x = Random.Range(0.5f, 0.9f);
                //if this value is on the edge of the screen
                if (x > 0.85f)
                {
                    //y is any value on the screen
                    if (position.y > 0.5f) y = Random.Range(0.5f, 0.9f);
                    else y = Random.Range(0.1f, 0.5f);
                }
                //otherwise y needs to be on the edge of the screen
                else
                {
                    if (position.y > 0.5f) y = Random.Range(0.85f, 0.9f);
                    else y = Random.Range(0.1f, 0.15f);
                }
            }
            //left half
            else
            {
                x = Random.Range(0.1f, 0.5f);
                if (x < 0.15f)
                {
                    if (position.y > 0.5f) y = Random.Range(0.5f, 0.9f);
                    else y = Random.Range(0.1f, 0.5f);
                }
                else
                {
                    if (position.y > 0.5f) y = Random.Range(0.85f, 0.9f);
                    else y = Random.Range(0.1f, 0.15f);
                }

            }


            /*if (position.x > 0.25f && position.x < 0.75f)
            {
                //go to random point along top or bottom

            }
            //otherwise we stay.
            else
            {
                x = Mathf.Clamp(position.x, 0.25f, 0.75f);
            }
            // if y is in the middle of the screen
            if (position.y > 0.25f && position.y < 0.75f)
            {
                //go to random point along sides
                if (position.x > 0.5f) x = Random.Range(0.75f, 0.85f);
                if (position.x <= 0.5f) x = Random.Range(0.15f, 0.25f);
            }
            //otherwise we stay
            else
            {
                x = Mathf.Clamp(position.x, 0.25f, 0.75f);
            }*/
            Vector2 viewPoint = new Vector2(x, y);
/*            Vector3 targetPosition = CameraFollow.MAINCAMERA.ViewportToWorldPoint(viewPoint);*/
            Debug.Log("Shooter Target Position: " + viewPoint);
            NavMeshHit navHit;
            RaycastHit rayHit;
            Ray ray = CameraFollow.MAINCAMERA.ViewportPointToRay(viewPoint);

            if (Physics.Raycast(ray, out rayHit, 100, DeveloperConsoleBehavior.PLAYER.ground))
            {

                if (NavMesh.SamplePosition(rayHit.point, out navHit, 20, machine.Agent.areaMask))
                {
                    if (machine.Agent.enabled)
                    {
                        machine.Agent.SetDestination(navHit.position);
                        machine.Animator.SetBool("Walking", true);
                    }
                }
            }

        }
    }
}
