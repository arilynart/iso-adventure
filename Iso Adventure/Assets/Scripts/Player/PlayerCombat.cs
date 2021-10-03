using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Ludiq;
using Bolt;
using Arilyn.DeveloperConsole.Behavior;

public class PlayerCombat : MonoBehaviour
{
    PlayerController controller;
    PlayerDodge dodge;

    [SerializeField]
    private GameObject fireball;
    

    public Animator animator;
    public GameObject hurtBox;
    public int noOfPresses = 0;
    System.DateTime lastPressedTime;
    public float maxComboDelay = 1.25f;
    public LayerMask enemyLayers;
    public int attackDamage = 1;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        dodge = GetComponent<PlayerDodge>();
        deactivateHurtbox();
    }

    private void FixedUpdate()
    {
        if(System.DateTime.Now.Subtract(lastPressedTime).TotalSeconds > maxComboDelay)
        {
            noOfPresses = 0;
        }

    }

    public void BasicAttack()
    {
        if (!PlayerUnlocks.ATTACK) return;

        //if not already dodging or falling (based on collision with floor)
        if (controller.grounded == true)
        {
            if ((bool)Variables.Object(gameObject).Get("animLock") == true) return;
            if (controller.MouseActivityCheck())
            {
                transform.LookAt(controller.GetLookPoint());
            }
            //detect enemies in range of attack


            //animation controls go here
            lastPressedTime = System.DateTime.Now;
            if (noOfPresses == 0)
            {
                CustomEvent.Trigger(gameObject, "AttackButton1");
                StartCoroutine(AttackAnimation(0.13f, 0.32f));
                    
                Debug.Log("You attack once!");
            }                    
            else if (noOfPresses == 1)
            {
                CustomEvent.Trigger(gameObject, "AttackButton2");
                StartCoroutine(AttackAnimation(0.12f, 0.25f));
                Debug.Log("You attack twice!");
            }
            else
            {
            }
            return;
        }
    }

    public void Shoot()
    {
        if (!PlayerUnlocks.FIREBALL) return;

        if ((bool)Variables.Object(gameObject).Get("animLock") == true) return;
        Debug.Log("Shooting");
        
        Vector3 trajectory;
        if (DeveloperConsoleBehavior.PLAYER.MouseActivityCheck())
        {
            trajectory = DeveloperConsoleBehavior.PLAYER.mousePoint.transform.forward;
        }
        else trajectory = DeveloperConsoleBehavior.PLAYER.point;

        transform.forward = trajectory;
        Debug.Log("Aiming at: " + trajectory);
        GameObject ball = Instantiate(fireball, transform.position + new Vector3(0,0.5f,0), transform.rotation);
        ball.GetComponent<ShootFireball>().trajectory = trajectory;
        StartCoroutine(DestroyFireball(ball));

        CustomEvent.Trigger(gameObject, "ShootTrigger");
        StartCoroutine(AttackAnimation(0.13f, 0.32f));
    }

    public IEnumerator AttackAnimation(float hurtBoxStart, float hurtBoxEnd)
    {
        float time = 0;


        while (time < animator.GetCurrentAnimatorStateInfo(0).length)
        {
            if (time >= hurtBoxStart && time < hurtBoxEnd) activateHurtbox();
            if (time > hurtBoxEnd) deactivateHurtbox();

            time += Time.deltaTime;
            yield return null;
        }

        ReturnAttack();
    }

    IEnumerator DestroyFireball(GameObject ball)
    {
        yield return new WaitForSeconds(5);

        if (ball)
            Destroy(ball);
    }

    public void activateHurtbox()
    {
        hurtBox.GetComponent<Collider>().enabled = true;
        Debug.Log("Hurtbox on!");
    }

    public void deactivateHurtbox()
    {
        hurtBox.GetComponent<Collider>().enabled = false;
        Debug.Log("Hurtbox off!");
        
    }

    public void ReturnAttack()
    {
        Debug.Log("Returning");
        CustomEvent.Trigger(gameObject, "ReturnAttack");
    }

}
