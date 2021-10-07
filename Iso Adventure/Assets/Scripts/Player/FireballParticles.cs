using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballParticles : MonoBehaviour
{
    //ParticleSystem ps;
    ShootFireball fireball;

    private void Start()
    {
        fireball = transform.parent.GetComponent<ShootFireball>();
    }

    private void Update()
    {
        if (fireball.hit == true){
            Destroy(gameObject, 2f);
        }
    }
}

