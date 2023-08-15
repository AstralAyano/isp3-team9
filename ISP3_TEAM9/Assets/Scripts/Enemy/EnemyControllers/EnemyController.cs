using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public enum State
    {
        IDLE,
        PATROL,
        ATTACK,
        DEAD
    }

    [SerializeField] private Animator ar;
    [SerializeField] private State currentState;
    [SerializeField] private List<GameObject> waypoints = new List<GameObject>();
    [SerializeField] private EnemyPathFinding enemyPF; 

    [SerializeField] private float health;

    private int targetIndex;

    private float idleTime;

    private bool attackToResolve = false;
    private float attackTimer = 0;

    void Start()
    {
        ar = GetComponentInChildren<Animator>();

        targetIndex = 0;
        enemyPF.target = waypoints[targetIndex].transform;
    }

    void Update()
    {
        switch (currentState)
        {
            case State.IDLE:
                Idle();
                break;
            case State.PATROL:
                Patrol();
                break;
            case State.ATTACK:
                Attack();
                break;
        }
    }

    private void ChangeState(State next)
    {
        if (next == State.IDLE)
        {
            idleTime = 0.0f;
        }
        else if (next == State.PATROL)
        {
            targetIndex++;
            if (targetIndex > waypoints.Count - 1)
            {
                targetIndex = 0;
            }
            enemyPF.target = waypoints[targetIndex].transform;
            enemyPF.reachedEndOfPath = false;
        }
        else if (next == State.ATTACK)
        {
            enemyPF.target = GameObject.FindWithTag("Player").transform;
        }
        else if (next == State.DEAD)
        {
            Debug.Log("Dead");
        }

        currentState = next;
    }

    private void Idle()
    {
        // play anim


        // start timer
        idleTime += Time.deltaTime;

        // swap to partrol after waiting
        if (idleTime >= 2.0f)
        {
            idleTime = 0f;
            ChangeState(State.PATROL);
        }
    }

    private void Patrol()
    {
        // play anim


        // if reached waypoint, swap to idle
        if (enemyPF.reachedEndOfPath)
        {
            ChangeState(State.IDLE);
        }
    }

    private void Attack()
    {
        if (attackToResolve) // else wait till attack over
        {
            attackTimer += Time.deltaTime;

            if (attackTimer < 0.1f)
            {
                attackToResolve = false;
            }
        }
    }

    public void PlayerWithinAggro(Collider2D other) // called by child (AggroRange)
    {
        if (currentState != State.DEAD && other.gameObject.CompareTag("PlayerHitbox"))
        {
            ChangeState(State.ATTACK);
        }
    }
    public void PlayerExitAggro(Collider2D other) // called by child (AggroRange)
    {
        if (currentState != State.DEAD && other.gameObject.CompareTag("PlayerHitbox"))
        {
            ChangeState(State.PATROL);
        }
    }

    public void PlayerInAttackRange(Collider2D other) // called by child (AttackRange)
    {
        if (!attackToResolve && currentState == State.ATTACK && currentState != State.DEAD)
        {
            if (other.gameObject.CompareTag("PlayerHitbox"))
            {
                attackToResolve = true;

                // play anim


                // call TakeDamage func in player using the child collider (PlayerHitbox)
                //other.gameObject.GetComponentInParent<PlayerController>().TakeDamage(10);
            }
        }
    }
    public void PlayerExitAttackRange(Collider2D other) // called by child (AttackRange)
    {
        if (!attackToResolve && currentState == State.ATTACK && currentState != State.DEAD)
        {
            if (other.gameObject.CompareTag("PlayerHitbox"))
            {
                // go back to following player

            }
        }
    }

    public void TakeDamage(float damage)
    {
        if ((health - damage) <= 0)
        {
            ChangeState(State.DEAD);
            health = 0;

            // toggle collision with player
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("EnemyShield"), LayerMask.NameToLayer("Player"), true);

            // play anim


            // destroy self
            Destroy(gameObject, 0.75f);
        }
        else
        {
            health -= damage;

            // play anim

        }
    }
}
