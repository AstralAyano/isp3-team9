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
    [SerializeField] protected ParticleSystem hurtParticles;
    [SerializeField] protected AudioSource hurtdeathAS;
    [SerializeField] protected AudioClip[] hurtdeathClip;
    [SerializeField] protected AudioSource attackAS;
    [SerializeField] protected AudioClip[] attackClip;
    [SerializeField] protected State currentState;
    [SerializeField] protected EnemyPathFinding enemyPF;

    [SerializeField] protected float health = 50;
    [SerializeField] protected int expDropped = 10;
    public int attack = 10;
    [SerializeField] protected float attackCD = 0.5f;
    [SerializeField] protected float attackDuration = 0.5f;

    protected float attackTimer, attackDelay;
    [SerializeField] protected GameObject spawnPos;
    [HideInInspector] public string animToPlay = "AnimEnemyDownIdle";

    public void ChangeState(State next)
    {
        if (next == State.IDLE)
        {
            
        }
        else if (next == State.PATROL)
        {
            enemyPF.target = spawnPos.transform;
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
        ar.Play(animToPlay);
    }

    protected void Patrol()
    {
        // play anim
        ar.Play(animToPlay);

        // if reached waypoint, swap to idle
        if (enemyPF.reachedEndOfPath)
        {
            ChangeState(State.IDLE);
        }
    }

    protected void Attack()
    {
        if (enemyPF.attackToResolve) // else wait till attack over
        {
            attackTimer += Time.deltaTime;

            // play anim
            ar.Play("AnimEnemy" + enemyPF.animDir + "Attack");

            if (attackTimer > attackDuration)
            {
                enemyPF.attackToResolve = false;
                attackTimer = 0;
                attackDelay = 0;
            }
        }
        else
        {
            attackDelay += Time.deltaTime;
            // play anim
            ar.Play(animToPlay);
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

    public abstract void PlayerInAttackRange(Collider2D other); // called by child (AttackRange)
    public void PlayerExitAttackRange(Collider2D other) // called by child (AttackRange)
    {
        if (enemyPF.attackToResolve && currentState == State.ATTACK && currentState != State.DEAD)
        {
            if (other.gameObject.CompareTag("PlayerHitbox"))
            {
                // go back to following player
                enemyPF.attackToResolve = false;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if ((health - damage) <= 0 && currentState != State.DEAD)
        {
            ChangeState(State.DEAD);
            health = 0;

            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<PlayerController>().GainXP(expDropped);

            // play anim
            ar.Play("AnimEnemyDeath");

            hurtdeathAS.clip = hurtdeathClip[1];
            hurtdeathAS.Play();

            // destroy self
            Destroy(gameObject, 0.75f);
        }
        else
        {
            health -= damage;

            hurtdeathAS.clip = hurtdeathClip[0];
            hurtdeathAS.Play();

            // Play Particles
            hurtParticles.Play();

            // play anim

        }
    }

    public void PlayAttackSound(int clip)
    {
        attackAS.clip = attackClip[clip];
        attackAS.Play();
    }
}
