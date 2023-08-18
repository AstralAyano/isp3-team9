using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneShot : MonoBehaviour
{
    public GameObject ChainLightningEffect;

    private Rigidbody2D rb;

    private float angleInRad;
    private Vector2 dir;

    void OnEnable()
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
            Instantiate(ChainLightningEffect, other.gameObject.transform.position, Quaternion.identity);
        }
    }

}
