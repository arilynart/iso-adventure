using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using Ludiq;
using Bolt;
using Arilyn.DeveloperConsole.Behavior;

public class BullAttack : MonoBehaviour, IEnemyAttack
{
    public GameObject[] slows;
    List<GameObject> activeSlows = new List<GameObject>();
    public GameObject root;
    //public System.Random rnd = new System.Random();

    EnemyStats stats;

    string attackName;
    string animationName;

    bool active;

    int damage;
    [SerializeField]
    int attackCounter = 0;
    float range;
    float boxStart;
    float boxEnd;

    public float jumpSpeed = 1;

    int nextAttack;

    void Awake()
    {
        stats = GetComponent<EnemyStats>();

    }



    public void InitializeAttack(string name, string anim, int dmg, float rng, float start, float end, int next)
    {
        attackName = name;
        animationName = anim;
        damage = dmg;
        range = rng;
        boxStart = start;
        boxEnd = end;
        nextAttack = next;
    }

    public void ExecuteAttack()
    {
        Debug.Log("Executing attack: " + attackName + " " + animationName + " " + damage + " " + boxStart + " " + boxEnd + " " + nextAttack + " ");
        if (stats.activeAttack == stats.attacks[0])
        {
            attackCounter++;

        }
        else if (stats.activeAttack == stats.attacks[1])
        {
            ActivateSlows();
            attackCounter = 0;
        }
        else if (stats.activeAttack == stats.attacks[2])
        {
            attackCounter = 0;
        }
        else if (stats.activeAttack == stats.lockedAttacks[1])
        {
            if (attackCounter >= 2)
            {
                stats.nextAttack = 3;
            }
        }

    }



    public void ActivateSlows()
    {
        if (!active)
        {
            //Random a = new Random();
            /*            var sequence = Enumerable.Range(0, slows.Length).OrderBy(n => n * n * (new System.Random()).Next());
                        var result = sequence.Distinct().Take(1);
                        List<int> list = result.ToList();
                        Debug.Log("BullAttack List: " + list);
                        int t = 0;*/
            int r = Random.Range(0, slows.Length);
            int t = 0;
            foreach (GameObject slow in slows.ToList())
            {

                if (t == r)
                {
                    Debug.Log("BullAttack: Activating slowpost " + t);
                    slow.GetComponent<SlowPost>().Activate();
                    activeSlows.Add(slow);
                }

                t++;
            }
            active = true;
        }
        else
        {
            foreach (GameObject slow in activeSlows.ToList())
            {
                slow.GetComponent<SlowPost>().Activate();
            }
            activeSlows.Clear();
            active = false;
        }

    }

    public IEnumerator Jumping()
    {
        float startHeight = root.transform.position.y;
        float time = 0;
        while (time < (stats.animator.GetCurrentAnimatorStateInfo(0).length / 2))
        { 
            root.transform.Translate(Vector3.up * Time.deltaTime * jumpSpeed, Space.World);
            time += Time.deltaTime;
            yield return null;
        }

        while (time < stats.animator.GetCurrentAnimatorStateInfo(0).length)
        {
            root.transform.Translate(-Vector3.up * Time.deltaTime * jumpSpeed, Space.World);
            time += Time.deltaTime;
            yield return null;
        }
            
        root.transform.position = new Vector3(transform.position.x, startHeight, transform.position.z);
    }
}
