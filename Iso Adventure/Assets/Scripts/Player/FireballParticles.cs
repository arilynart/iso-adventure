using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballParticles : MonoBehaviour
{
    ShootFireball fireball;
    public ParticleSystem fireballHit;

    private void Start()
    {
        fireball = transform.parent.GetComponent<ShootFireball>();
    }

    private void Update()
    {
        if (fireball.hit == true)
        {
            fireballHit.Play();
            Destroy(gameObject, 3f);
        }
    }
}

