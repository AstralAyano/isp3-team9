using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasicMeleeEnemy : EnemyController
{
    void Start()
    {
        spawnPos.transform.parent = null;
        enemyPF.target = spawnPos.transform;
        Debug.Log(spawnPos);
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
}
