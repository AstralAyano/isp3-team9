using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasicMeleeEnemy : EnemyController
{
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
    }
}
