using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    [SerializeField]
    private ScriptablePlayerStats playerStats;

    [SerializeField]
    private BasicRangedEnemy rangedEnemy;

    private Rigidbody2D rb;

    private float angleInRad;
    private Vector2 dir;
    public bool shotByEnemy;
    public float speed;

    // Start is called before the first frame update
    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();

        angleInRad = transform.localEulerAngles.z * Mathf.Deg2Rad;
        dir = new Vector2(Mathf.Cos(angleInRad), Mathf.Sin(angleInRad));
    }

    private void Update()
    {
        Destroy(gameObject, 2f);
    }

    void FixedUpdate()
    {
        if (shotByEnemy)
        {
            rb.AddForce(dir * speed, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(dir * playerStats.chosenStats.projectileSpeed, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (shotByEnemy)
        {
            if (other.gameObject.CompareTag("PlayerHitbox"))
            {
                // call TakeDamage func in player using the child collider (PlayerHitbox)
                other.gameObject.GetComponentInParent<PlayerController>().PlayerTakeDamage(rangedEnemy.attack);
                Destroy(gameObject);
            }
            else if (other.gameObject.CompareTag("Wall"))
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (other.gameObject.CompareTag("EnemyHitbox"))
            {
                other.gameObject.GetComponentInParent<EnemyController>().TakeDamage(playerStats.chosenStats.attack);
                Destroy(gameObject);
            }
            else if (other.gameObject.CompareTag("Wall"))
            {
                Destroy(gameObject);
            }
        }
    }
}
