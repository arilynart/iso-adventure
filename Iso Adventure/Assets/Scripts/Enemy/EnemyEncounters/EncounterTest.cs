using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterTest : EnemyEncounter
{
    public GameObject[] enemies;
    public Transform[] spawns;

    int deaths;

    public bool endRound;

    private void OnTriggerStay(Collider other)
    {
        if (!other.GetComponent<PlayerController>()) return;

        Debug.Log("Encounter Starting");

        StartCoroutine(StartEncounter());
        GetComponent<Collider>().enabled = false;
    }

    private void Update()
    {
        if (DEATHCOUNT >= deaths)
        {
            endRound = true;
        }
    }

    public override IEnumerator StartEncounter()
    {
        base.StartEncounter();

        int round = 0;
        int maxRounds = 2;

        while (round < maxRounds)
        {
            round++;
            DEATHCOUNT = 0;
            endRound = false;
            Debug.Log("Starting Round: " + round);
            switch (round)
            {
                case 1:
                    deaths = 2;
                    Instantiate(enemies[0], spawns[0]);
                    Instantiate(enemies[0], spawns[2]);
                    break;
                case 2:
                    deaths = 3;
                    Instantiate(enemies[0], spawns[0]);
                    Instantiate(enemies[0], spawns[1]);
                    Instantiate(enemies[0], spawns[2]);
                    break;
            }
            yield return new WaitUntil(() => endRound);
        }

        Debug.Log("Encounter Ended");

        Destroy(gameObject);
    }
}
