using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : EnemyController
{
    private int attackToUse;

    private Collider2D triggerCollider = null;

    [SerializeField] private GameObject ladderPrefab;
    private bool isLadderSpawned = false;

    void Start()
    {
        GameObject tempGO = GameObject.Find("DungeonContentContainer");
        spawnPos.transform.parent = tempGO.transform;
        enemyPF.target = spawnPos.transform;
        attackToUse = Random.Range(1, 3);

        health += (UIController.floorNum - 1) * 75;
        attack += (UIController.floorNum - 1) * 3;
        expDropped += (UIController.floorNum - 1) * 10;
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
            case State.DEAD:
                BossDeath();
                break;
        }
        animToPlay = "AnimEnemy" + enemyPF.animDir + "Walk";
    }

    private void BossDeath()
    {
        if (!isLadderSpawned)
        {
            isLadderSpawned = true;
            Instantiate(ladderPrefab, transform.position, Quaternion.identity);
        }
    }

    public override void PlayerInAttackRange(Collider2D other)
    {
        if (attackDelay > attackCD && !enemyPF.attackToResolve && currentState == State.ATTACK && currentState != State.DEAD)
        {
            if (other.gameObject.CompareTag("PlayerHitbox"))
            {
                enemyPF.attackToResolve = true;

                //damagePlayer = true;
                triggerCollider = other;
            }
        }
    }

    private void BossAttack()
    {
        if (enemyPF.attackToResolve)
        {
            attackTimer += Time.deltaTime;

            ar.Play("AnimEnemy" + enemyPF.animDir + "Attack" + attackToUse);

            //Second attack is spinning, so the player constantly takes damage
            if (attackToUse == 2)
            {
                // call TakeDamage func in player using the child collider (PlayerHitbox)
                triggerCollider.gameObject.GetComponentInParent<PlayerController>().PlayerTakeDamage(attack/10);
            }

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

    //Called in animation events
    public void DamagePlayer()
    {
        if (enemyPF.attackToResolve)
        {
            // call TakeDamage func in player using the child collider (PlayerHitbox)
            triggerCollider.gameObject.GetComponentInParent<PlayerController>().PlayerTakeDamage(attack);
            //Debug.Log("Hit");
            //damagePlayer = false;
        }
    }
}
