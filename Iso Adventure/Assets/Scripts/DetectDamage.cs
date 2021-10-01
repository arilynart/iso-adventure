using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectDamage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Attack" && !transform.parent.gameObject.GetComponent<PlayerController>().invuln)
        {
            Debug.Log("Damage: " + other.transform.parent.GetComponent<EnemyStats>().activeAttack.damage);
            transform.parent.gameObject.GetComponent<PlayerHealth>().TakeDamage(other.transform.parent.GetComponent<EnemyStats>().activeAttack.damage);

/*            Collider[] colliders = Physics.OverlapSphere(transform.position, 50f);

            foreach (Collider closeObjects in colliders)
            {
                Rigidbody rigidbody = closeObjects.GetComponent<Rigidbody>();

                if (rigidbody != null)
                {
                    rigidbody.AddExplosionForce(500f, transform.position, 50f);
                }
            }*/

        }
    }
}
