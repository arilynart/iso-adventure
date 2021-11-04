using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arilyn.State.PlayerState;

public class PlayerStateMachine : MonoBehaviour
{
    [Header("Fetched Components")]
    public PlayerState currentState;
    public PlayerController controller;
    public Rigidbody rb;

    [Header("Dodging")]
    public float dashSpeed = 12f;
    public float dashDuration = 0.7f;
    public float dashTime = 0.3f;

    public bool dashDelay;

    [Header("Combat")]
    public GameObject fireball;
    public GameObject sword;
    public GameObject slash;
    public Transform attackPoint;
    public Animator slashAnim;

    public float maxComboDelay = 1.25f;

    public int noOfPresses = 0;
    public int attackDamage = 1;
    public int manaDamage = 2;
    public int fireballCost = 4;

    public System.DateTime lastPressedTime;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //basic attack
        Gizmos.DrawWireSphere(attackPoint.position, 0.99f);
    }

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
        slashAnim = slash.GetComponent<Animator>();
        ChangeState(new IdleState(this));
    }

    private void Update()
    {
        currentState.LocalUpdate();
        if (controller.grounded) controller.airDodge = false;
    }

    private void FixedUpdate()
    {
        currentState.LocalFixedUpdate();
        if (System.DateTime.Now.Subtract(lastPressedTime).TotalSeconds > maxComboDelay)
        {
            noOfPresses = 0;
        }
    }

    public void ChangeState(PlayerState state)
    {
        currentState = state;
        StartCoroutine(currentState.EnterState());
        //Debug.Log("Current State: " + state);
    }

    public void Movement(Vector2 value)
    {
        currentState.Movement(value);
    }

    public void Dodge()
    {
        if (!controller.airDodge) currentState.Dodge();
    }

    public void BasicAttack()
    {
        if (PlayerUnlocks.BASIC_ATTACK && noOfPresses < 3) currentState.BasicAttack();
    }

    public void Shoot()
    {
        if (!PlayerUnlocks.FIREBALL || controller.mana.mana < fireballCost || PauseMenu.PAUSED) return;

        currentState.Shoot();

    }

    public void Heal()
    {
        if (controller.mana.mana < controller.health.healCost || controller.invuln) return;

        currentState.Heal();
    }

    public void Blink()
    {
        if (PlayerUnlocks.BLINK) currentState.Blink();
    }

    public void Interact()
    {
        currentState.Interact();
    }

    public void Death()
    {
        ChangeState(new DeathState(this));
    }

    public void CreateFireball()
    {
        Vector3 trajectory;
        if (controller.MouseActivityCheck())
        {
            trajectory = controller.mousePoint.transform.forward;
        }
        else trajectory = controller.point;

        //point at fireball
        transform.forward = trajectory;

        //Make fireball and assign trajectory
        GameObject ball = Instantiate(fireball, transform.position + new Vector3(0, 0.5f, 0), transform.rotation);
        ball.transform.forward = transform.forward;
        ball.GetComponent<ShootFireball>().trajectory = trajectory;
        StartCoroutine(DestroyFireball(ball));
    }

    IEnumerator DestroyFireball(GameObject ball)
    {
        yield return new WaitForSeconds(2);

        if (ball)
            Destroy(ball);
    }
}
