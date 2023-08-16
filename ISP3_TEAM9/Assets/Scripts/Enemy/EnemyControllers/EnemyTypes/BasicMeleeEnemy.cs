using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasicMeleeEnemy : EnemyController
{
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
}
