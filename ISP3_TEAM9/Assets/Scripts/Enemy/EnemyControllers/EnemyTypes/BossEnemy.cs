using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : EnemyController
{
    private int attackToUse;

    void Start()
    {
        GameObject tempGO = GameObject.Find("DungeonContentContainer");
        spawnPos.transform.parent = tempGO.transform;
        enemyPF.target = spawnPos.transform;
        attackToUse = Random.Range(1, 3);
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
                BossAttack();
                break;
        }
        animToPlay = "AnimEnemy" + enemyPF.animDir + "Walk";
    }

    public override void PlayerInAttackRange(Collider2D other)
    {
        if (attackDelay > attackCD && !enemyPF.attackToResolve && currentState == State.ATTACK && currentState != State.DEAD)
        {
            if (other.gameObject.CompareTag("PlayerHitbox"))
            {
                enemyPF.attackToResolve = true;

                // call TakeDamage func in player using the child collider (PlayerHitbox)
                other.gameObject.GetComponentInParent<PlayerController>().PlayerTakeDamage(15);
            }
        }
    }

    private void BossAttack()
    {
        if (enemyPF.attackToResolve)
        {
            attackTimer += Time.deltaTime;

            ar.Play("AnimEnemy" + enemyPF.animDir + "Attack" + attackToUse);

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
            newRandomAttack();
        }
    }

    private void newRandomAttack()
    {
        attackToUse = Random.Range(1, 3);
    }
}
