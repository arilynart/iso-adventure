using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyEncounter : MonoBehaviour
{
    public static int DEATHCOUNT = 0;
    public virtual IEnumerator StartEncounter()
    {
        Debug.Log("Starting encounter: " + this);

        yield return null;
    }
}
