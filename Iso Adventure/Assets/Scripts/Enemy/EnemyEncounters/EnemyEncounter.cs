using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyEncounter : MonoBehaviour
{
    public static int DEATHCOUNT = 0;
    public static int DEATHS = 0;
    public static List<GameObject> ENEMIES = new List<GameObject>();

    public static void ADD_ENEMY(GameObject enemy)
    {
        ENEMIES.Add(enemy);
    }

    public static void REMOVE_ENEMY(GameObject enemy)
    {
        ENEMIES.Remove(enemy);
    }

    public static void AGGRO_ENEMIES()
    {
        foreach (GameObject obj in ENEMIES)
        {
            DetectPlayer detect = obj.GetComponentInChildren<DetectPlayer>();

            detect.viewAngle = 999;
            detect.viewDistance = 999;
        }
    }

    public virtual IEnumerator StartEncounter()
    {
        Debug.Log("Starting encounter: " + this);

        yield return null;
    }
}
