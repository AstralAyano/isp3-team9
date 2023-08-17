using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballUlt : MonoBehaviour
{
    private Rigidbody2D rb;

    private float angleInRad;
    private Vector2 dir;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        angleInRad = transform.localEulerAngles.z * Mathf.Deg2Rad;
        dir = new Vector2(Mathf.Cos(angleInRad), Mathf.Sin(angleInRad));
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(dir * 0.1f, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (GameObject.FindWithTag("FireBall2ndRadius") && GetComponentInChildren<CircleCollider2D>())
        {
            GetComponentInChildren<CircleCollider2D>().gameObject.SetActive(true);
        }
    }
}
