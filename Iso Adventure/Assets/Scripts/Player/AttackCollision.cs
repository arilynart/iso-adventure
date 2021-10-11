using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.DeveloperConsole.Behavior;

public class AttackCollision : MonoBehaviour
{
    PlayerMana mana;
    RaycastHit hit;
    Vector3 raycastOffset = new Vector3(0, 0.5f, 0);

    private void Start()
    {
        mana = transform.parent.GetComponent<PlayerMana>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyStats>()) {
            Debug.Log("Hit " + other.name);
            EnemyStats stats = other.GetComponent<EnemyStats>();

            //calling damage method on collided enemy
            stats.TakeDamage(transform.parent.GetComponent<PlayerCombat>().attackDamage);
            //mana restoration
            mana.AddMana(1);
        }
        else if (other.GetComponent<BlockPush>())
        {
            Debug.Log("Hit Block");
            if (Physics.Raycast(transform.parent.position + raycastOffset, transform.parent.forward + raycastOffset, out hit, 5f, DeveloperConsoleBehavior.PLAYER.ground)) {
                Debug.Log("Raycast Hit");

                Vector3 localPoint = hit.transform.InverseTransformPoint(hit.point);
                Vector3 localDir = localPoint.normalized;

                other.GetComponent<BlockPush>().Slide(localDir);
                
            }

        }
    }


}
