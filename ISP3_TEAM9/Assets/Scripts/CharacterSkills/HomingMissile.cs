using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HomingMissile : MonoBehaviour
{
    [SerializeField]
    private ScriptablePlayerStats playerStats;

    public float rotateSpeed = 200f;

    public GameObject explosionEffect;

    public GameObject Target;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Target != null)
        {
            Vector2 direction = (Vector2)Target.transform.position - rb.position;

            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            rb.angularVelocity = -rotateAmount * rotateSpeed;

            rb.velocity = transform.up * playerStats.chosenStats.projectileSpeed;
        }
        else
        {
            rb.angularVelocity = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemyHitbox"))
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
            other.gameObject.GetComponentInParent<EnemyController>().TakeDamage(playerStats.chosenStats.attack);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}