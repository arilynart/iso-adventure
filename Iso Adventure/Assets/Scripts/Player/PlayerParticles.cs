using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{

    private ParticleSystem ps;
    public PlayerController controller;


    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        var main = ps.main;
        var emmission = ps.emission;

        if (controller.moving == true)
        {
            main.startLifetime = new ParticleSystem.MinMaxCurve(0.35f, 0.45f);
            main.startSpeed = 2.5f;
            main.startSize = new ParticleSystem.MinMaxCurve(2.25f, 2.45f);
            emmission.rateOverTime = new ParticleSystem.MinMaxCurve(100f, 125f);

        }
        else
        {
            main.startLifetime = new ParticleSystem.MinMaxCurve(1.25f, 1.75f);
            main.startSpeed = 3.35f;
            main.startSize = new ParticleSystem.MinMaxCurve(1.35f, 1.55f);
            emmission.rateOverTime = new ParticleSystem.MinMaxCurve(45f, 55);
        }
    }
}
