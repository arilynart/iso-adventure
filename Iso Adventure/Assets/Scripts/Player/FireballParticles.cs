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
        fireball.hit = false;
    }

    private void Update()
    {
        if (fireball.hit == true)
        {
            Explosion();
            fireball.hit = false;
        }
        else
        {
            return;
        }
    }

    void Explosion()
    {
        if (fireball.hit == true)
        {
            fireballHit.Play();
            Debug.Log("Particle: " + fireballHit);
            Destroy(gameObject, 2f);
        }
    }
}

