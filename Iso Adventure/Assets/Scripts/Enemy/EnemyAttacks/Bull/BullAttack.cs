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
    //public System.Random rnd = new System.Random();

    NavMeshAgent agent;

    string attackName;
    string animationName;

    bool active;

    int damage;
    float range;
    float boxStart;
    float boxEnd;

    int nextAttack;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
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

        if (attackName == "Attack_02" || attackName == "Attack_03")
        {
            Debug.Log("BullAttack: Rotating Attack");

        }
        else if (attackName == "ActivateSlow")
        {
            ActivateSlows();
        }
    }

    public void ActivateSlows()
    {
        if (!active)
        {
            //Random a = new Random();
            var sequence = Enumerable.Range(0, slows.Length).OrderBy(n => n * n * (new System.Random()).Next());
            var result = sequence.Distinct().Take(1);
            List<int> list = result.ToList();
            Debug.Log("BullAttack List: " + list);
            int t = 0;
            foreach (GameObject slow in slows.ToList())
            {

                if (list.Contains(t))
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


}
