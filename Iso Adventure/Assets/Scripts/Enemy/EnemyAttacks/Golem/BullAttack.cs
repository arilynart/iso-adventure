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
    public EnemyDissolve dissolve;

    string attackName;
    string animationName;

    bool active;
    public bool jumpTurn = false;

    public float stepDelay;

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
        dissolve = GetComponentInChildren<EnemyDissolve>();
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
            StartCoroutine(dissolve.ActivateDissolve(2, 0, 1.5f, 0));
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
        else if (stats.activeAttack == stats.lockedAttacks[2])
        {
            StartCoroutine(dissolve.ActivateDissolve(0, 2, 0.2f, 0));
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
        jumpTurn = true;
        float startHeight = root.transform.position.y;
        float time = 0;
        GetComponent<CapsuleCollider>().enabled = false;
        while (time < 0.9167f)
        {
            
            time += Time.deltaTime;
            yield return null;
        }
        jumpTurn = false;
        GetComponent<CapsuleCollider>().enabled = true;
        GetComponent<NavMeshAgent>().SetDestination(transform.position);
        GetComponent<NavMeshAgent>().speed = 0;
        GetComponent<NavMeshAgent>().acceleration = 0;
        while (time > 0.9167f && time < stats.animator.GetCurrentAnimatorStateInfo(0).length)
        {
            time += Time.deltaTime;
            yield return null;
        }
        GetComponent<CapsuleCollider>().enabled = true;
    }
}
