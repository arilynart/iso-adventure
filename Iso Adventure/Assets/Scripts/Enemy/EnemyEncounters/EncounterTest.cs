using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterTest : EnemyEncounter
{
    public GameObject[] enemies;
    public GameObject[] currentEnemies;
    public GameObject[] doors;

    public float zoom = 5;
    float baseZoom;
    float currentZoom;
    float targetZoom;

    public bool endRound;
    bool zoomOut;

    private void Start()
    {
        baseZoom = CameraFollow.MAINCAMERA.orthographicSize;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<PlayerController>()) return;

        Debug.Log("Encounter Starting");

        /*        foreach (Transform child in transform)
                {
                    if (child.GetComponent<GateController>())
                    {
                        child.GetComponent<GateController>().Close();
                    }
                }*/

        targetZoom = zoom;
        zoomOut = true;

        CameraFollow.LOCK = true;
        CameraFollow.LOCKTARGET = transform;
        

        StartCoroutine(StartEncounter());
        GetComponent<Collider>().enabled = false;
    }

    private void Update()
    {
        if (DEATHCOUNT >= DEATHS)
        {
            endRound = true;
        }
        if (zoomOut)
        {
            currentZoom = CameraFollow.MAINCAMERA.orthographicSize;
            CameraFollow.MAINCAMERA.orthographicSize = Mathf.Lerp(currentZoom, targetZoom, 0.01f);
            if (currentZoom >= targetZoom - 0.005f && currentZoom <= targetZoom + 0.005f) CameraFollow.MAINCAMERA.orthographicSize = targetZoom;
            if (currentZoom == targetZoom) zoomOut = false;
            if (CameraFollow.MAINCAMERA.orthographicSize == baseZoom) Destroy(gameObject);
        }
    }

    public override IEnumerator StartEncounter()
    {

        foreach (GameObject obj in doors)
        {
            obj.SetActive(true);
        }

        int round = 0;
        int maxRounds = 2;

        while (round < maxRounds)
        {
            round++;
            DEATHCOUNT = 0;
            DEATHS = 0;
            endRound = false;
            Debug.Log("Starting Round: " + round);
            switch (round)
            {
                case 1:
                    enemies[0].GetComponent<EnemyController>().Spawn();
                    enemies[1].GetComponent<EnemyController>().Spawn();
                    AGGRO_ENEMIES();

                    break;
                case 2:
                    enemies[2].GetComponent<EnemyController>().Spawn();
                    enemies[3].GetComponent<EnemyController>().Spawn();
                    enemies[4].GetComponent<EnemyController>().Spawn();
                    AGGRO_ENEMIES();
                    break;
            }
            yield return new WaitUntil(() => endRound);
        }

        Debug.Log("Encounter Ended");
        targetZoom = baseZoom;
        zoomOut = true;

        CameraFollow.LOCK = false;

        
    }
}
