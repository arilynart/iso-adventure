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
    PlayerMana mana;

    [SerializeField]
    private GameObject fireball;
    

    public Animator animator;
    public GameObject hurtBox;
    public int noOfPresses = 0;
    System.DateTime lastPressedTime;
    public float maxComboDelay = 1.25f;
    public LayerMask enemyLayers;
    public int attackDamage = 1;
    public int manaDamage = 2;
    public int fireballCost = 4;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        mana = GetComponent<PlayerMana>();

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
                StartCoroutine(PlayAnimation(0.13f, 0.32f));
                    
                Debug.Log("You attack once!");
            }                    
            else if (noOfPresses == 1)
            {
                CustomEvent.Trigger(gameObject, "AttackButton2");
                StartCoroutine(PlayAnimation(0.12f, 0.25f));
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
        if (mana.mana < fireballCost) return;
        if ((bool)Variables.Object(gameObject).Get("animLock") == true) return;
        Debug.Log("Shooting");

        mana.AddMana(-fireballCost);
        
        //calculate fireball trajectory
        Vector3 trajectory;
        if (DeveloperConsoleBehavior.PLAYER.MouseActivityCheck())
        {
            trajectory = DeveloperConsoleBehavior.PLAYER.mousePoint.transform.forward;
        }
        else trajectory = DeveloperConsoleBehavior.PLAYER.point;

        //point at fireball
        transform.forward = trajectory;
        Debug.Log("Aiming at: " + trajectory);

        //Make fireball and assign trajectory
        GameObject ball = Instantiate(fireball, transform.position + new Vector3(0,0.5f,0), transform.rotation);
        ball.GetComponent<ShootFireball>().trajectory = trajectory;
        StartCoroutine(DestroyFireball(ball));

        //enter fireball animation
        CustomEvent.Trigger(gameObject, "ShootTrigger");
        StartCoroutine(PlayAnimation(0.13f, 0.32f));
    }

    public IEnumerator PlayAnimation(float hurtBoxStart, float hurtBoxEnd)
    {
        float time = 0;

        while (time < animator.GetCurrentAnimatorStateInfo(0).length)
        {
            if (hurtBoxStart >= 0 || hurtBoxEnd >= 0)
            {
                if (time >= hurtBoxStart && time < hurtBoxEnd) activateHurtbox();
                if (time > hurtBoxEnd) deactivateHurtbox();
            }

            time += Time.deltaTime;
            yield return null;
        }

        ReturnAnimation();
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

    public void ReturnAnimation()
    {
        Debug.Log("Returning");
        CustomEvent.Trigger(gameObject, "Return");
    }

}
