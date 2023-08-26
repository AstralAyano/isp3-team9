using UnityEngine;

public class BasicRangedEnemy : EnemyController
{
    [SerializeField]
    private GameObject Arrow;

    private Vector2 lookDir;
    private float lookAngle;

    void Start()
    {
        GameObject tempGO = GameObject.Find("DungeonContentContainer");
        spawnPos.transform.parent = tempGO.transform;
        enemyPF.target = spawnPos.transform;

        health += (UIController.floorNum - 1) * 15;
        attack += (UIController.floorNum - 1) * 3;
        expDropped += (UIController.floorNum - 1) * 1;
    }

    void Update()
    {
        if (isStunned)
        {
            return;
        }

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
                lookDir = other.gameObject.transform.position - transform.position;
                lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
                enemyPF.attackToResolve = true; 

                Instantiate(Arrow, transform.position, Quaternion.Euler(0, 0, lookAngle));
            }
        }
    }
}
