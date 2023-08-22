using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasicMeleeEnemy : EnemyController
{
    private bool damagePlayer = false;
    private Collider2D triggerCollider = null;

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
        animToPlay = "AnimEnemy" + enemyPF.animDir + "Walk";
    }

    public override void PlayerInAttackRange(Collider2D other)
    {
        if (attackDelay > attackCD && !enemyPF.attackToResolve && currentState == State.ATTACK && currentState != State.DEAD)
        {
            if (other.gameObject.CompareTag("PlayerHitbox"))
            {
                enemyPF.attackToResolve = true;

                damagePlayer = true;
                triggerCollider = other;
            }
        }
    }

    //Called in animation events
    private void DamagePlayer()
    {
        if ((triggerCollider != null) && (damagePlayer))
        {
            // call TakeDamage func in player using the child collider (PlayerHitbox)
            triggerCollider.gameObject.GetComponentInParent<PlayerController>().PlayerTakeDamage(10);
            //Debug.Log("Hit");
            damagePlayer = false;
        }
    }
}
