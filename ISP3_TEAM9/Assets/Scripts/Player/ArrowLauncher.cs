using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLauncher : MonoBehaviour
{
    private Rigidbody2D rb;

    private float angleInRad;
    private Vector2 dir;
    public bool shotByEnemy;

    // Start is called before the first frame update
    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();

        angleInRad = transform.localEulerAngles.z * Mathf.Deg2Rad;
        dir = new Vector2(Mathf.Cos(angleInRad), Mathf.Sin(angleInRad));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(dir * 4, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (shotByEnemy)
        {
            if (other.gameObject.CompareTag("PlayerHitbox"))
            {
                // call TakeDamage func in player using the child collider (PlayerHitbox)
                //other.gameObject.GetComponentInParent<PlayerController>().TakeDamage(10);
                Destroy(gameObject);
            }
            else if (other.gameObject.CompareTag("Wall"))
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Wall"))
            {
                Destroy(gameObject);
            }
        }
    }
}
