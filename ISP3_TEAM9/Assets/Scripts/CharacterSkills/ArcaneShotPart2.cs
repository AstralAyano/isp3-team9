using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneShotPart2 : MonoBehaviour
{
    [SerializeField]
    private GameObject secondCollider;

    [SerializeField]
    private GameObject ArcaneGFX;

    private Rigidbody2D rb;

    private float angleInRad;
    private Vector2 dir;

    public float TimeBeforeActivate = 0;

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
        TimeBeforeActivate += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") || TimeBeforeActivate == 1.5f)
        {
            gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            secondCollider.SetActive(true);
            //ArcaneGFX.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        }
    }
}
