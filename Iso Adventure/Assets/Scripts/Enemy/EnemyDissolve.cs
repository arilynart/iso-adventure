using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDissolve : MonoBehaviour
    
{
    //Renderer renderer;
    EnemyStats enemyStats;
    Material materials;
    Renderer rend;
    //MaterialPropertyBlock block = new MaterialPropertyBlock();
    void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        //Material index starts at 0, but the Zombie has two materials (one for head, one for body) - change to, []
        materials = GetComponent<Renderer>().materials[1];
        //Fetch the Material from the Renderer of the GameObject
        //m_Material = GetComponent<Renderer>().material;
        //Debug.Log("Material(s):" + m_Material);
        /*block.GetTexture("_MainTex", string);
        Debug.Log("Texture: " + tex);*/

    }

    // Update is called once per frame
    void Update()
    {
        enemyStats = transform.parent.GetComponentInParent<EnemyStats>();
        if (enemyStats.hp <= 0)
        {
            Destroy(materials);
        }
    }
}
