using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class BasicRangedEnemy : EnemyController
{
    [SerializeField]
    private GameObject Arrow;

    protected Vector2 lookDir;
    protected float lookAngle;

    void Start()
    {
        GameObject tempGO = GameObject.Find("DungeonContentContainer");
        spawnPos.transform.parent = tempGO.transform;
        enemyPF.target = spawnPos.transform;
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
        lookDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
    }

    protected override void Attack()
    {
        if (attackToResolve) // else wait till attack over
        {
            attackTimer += Time.deltaTime;

            // play anim
            ar.Play("AnimEnemy" + animDir + "Attack");

            if (attackTimer > 1.0f)
            {
                attackToResolve = false;
                attackTimer = 0;
            }
        }
        else
        {
            // play anim
            ar.Play(animToPlay);
        }
    }

    public override void PlayerInAttackRange(Collider2D other)
    {
        if (!attackToResolve && currentState == State.ATTACK && currentState != State.DEAD)
        {
            if (other.gameObject.CompareTag("PlayerHitbox"))
            {
                attackToResolve = true;

                Instantiate(Arrow, transform.position, Quaternion.Euler(0, 0, lookAngle));
            }
        }
    }
}
