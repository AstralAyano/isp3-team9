using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyController : MonoBehaviour
{
    public enum State
    {
        IDLE,
        PATROL,
        ATTACK,
        DEAD
    }

    [SerializeField] protected Animator ar;
    [SerializeField] protected State currentState;
    [SerializeField] protected List<GameObject> waypoints = new List<GameObject>();
    [SerializeField] protected EnemyPathFinding enemyPF;

    [SerializeField] protected float health = 50;
    [SerializeField] public float speed = 100f;

    protected int targetIndex;

    protected float idleTime;

    protected bool attackToResolve = false;
    protected float attackTimer = 0;

    public void ChangeState(State next)
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

    protected void Idle()
    {
        // play anim


        // start timer
        idleTime += Time.deltaTime;

        // swap to partrol after waiting
        if (idleTime >= 1.5f)
        {
            idleTime = 0f;
            ChangeState(State.PATROL);
        }
    }

    protected void Patrol()
    {
        // play anim


        // if reached waypoint, swap to idle
        if (enemyPF.reachedEndOfPath)
        {
            ChangeState(State.IDLE);
        }
    }

    protected void Attack()
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
