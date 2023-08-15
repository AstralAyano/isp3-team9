using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private float health;

    private int targetIndex;
    private Vector3 destination;
    private Vector3 dir;
    private Rigidbody2D rb;

    private float idleTime;

    private bool attackToResolve = false;
    private float attackTimer = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ar = GetComponentInChildren<Animator>();

        targetIndex = 0;

        destination = waypoints[targetIndex].transform.position;
        dir = (destination - transform.position).normalized;
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

        UpdateSpriteDirection();
    }

    private void ChangeState(State next)
    {
        if (next == State.IDLE)
        {
            idleTime = 0.0f;
        }
        else if (next == State.PATROL)
        {
            destination = waypoints[targetIndex].transform.position;
            dir = (destination - transform.position).normalized;
        }
        else if (next == State.ATTACK)
        {
            destination = GameObject.FindWithTag("Player").transform.position;
            dir = (destination - transform.position).normalized;
        }
        else if (next == State.DEAD)
        {
            Debug.Log("Dead");
        }

        currentState = next;
    }

    private void Idle()
    {
        ar.Play("Idle");

        idleTime += Time.deltaTime;

        if (idleTime >= 2.0f)
        {
            ChangeState(State.PATROL);
        }
    }

    private void Patrol()
    {
        dir = (destination - transform.position).normalized;

        rb.velocity = new Vector2(dir.x * 2, 0);
        ar.Play("Walk");

        if (Vector3.Distance(destination, transform.position) <= 0.75)
        {
            targetIndex++;
            if (targetIndex > waypoints.Count - 1)
            {
                targetIndex = 0;
            }

            ChangeState(State.IDLE);
        }
    }

    private void Attack()
    {
        destination = GameObject.FindWithTag("Player").transform.position;
        dir = (destination - transform.position).normalized;

        if (!attackToResolve)
        {
            rb.velocity = new Vector2(dir.x * 2, 0);
        }
        else if (attackToResolve)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer < 0.1f)
            {
                rb.velocity = new Vector2(dir.x * 8, 0);
            }
        }
    }

    void UpdateSpriteDirection()
    {
        if (dir.x >= 0.01f)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            transform.localScale = new Vector2(-1, 1);
        }
    }

    public void PlayerWithinAggro(Collider2D other)
    {
        if (currentState != State.DEAD && other.gameObject.CompareTag("PlayerHitbox"))
        {
            ChangeState(State.ATTACK);
        }
    }

    public void PlayerExitAggro(Collider2D other)
    {
        if (currentState != State.DEAD && other.gameObject.CompareTag("PlayerHitbox"))
        {
            ChangeState(State.IDLE);
        }
    }

    public void PlayerInAttackRange(Collider2D other)
    {
        if (!attackToResolve && currentState == State.ATTACK && currentState != State.DEAD)
        {
            if (other.gameObject.CompareTag("PlayerHitbox"))
            {
                attackToResolve = true;

                ar.Play("Shield");

                //other.gameObject.GetComponentInParent<PlayerController>().TakeDamage(10);
            }
        }
    }

    public void PlayerExitAttackRange(Collider2D other)
    {
        //Debug.Log("Test");
    }

    public void TakeDamage(float damage)
    {
        if ((health - damage) <= 0)
        {
            ChangeState(State.DEAD);
            health = 0;

            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("EnemyShield"), LayerMask.NameToLayer("Player"), true);
            ar.Play("Dead");
            //GameObject.FindWithTag("Player").GetComponentInChildren<SkillRange>().RemoveEnemy(this.gameObject);

            Destroy(gameObject, 0.75f);
        }
        else
        {
            health -= damage;
            ar.Play("Hurt");
        }
    }
}
