using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneShotPart2 : MonoBehaviour
{

    [SerializeField]
    private GameObject ArcaneGFX;

    private Rigidbody2D rb;

    private float angleInRad;
    private Vector2 dir;

    public float TimeBeforeActivate = 0;

    [SerializeField]
    private ScriptablePlayerStats playerStats;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();

        angleInRad = transform.localEulerAngles.z * Mathf.Deg2Rad;
        dir = new Vector2(Mathf.Cos(angleInRad), Mathf.Sin(angleInRad));
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(dir * playerStats.chosenStats.projectileSpeed/50, ForceMode2D.Impulse);
        TimeBeforeActivate += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") || TimeBeforeActivate == 1.5f)
        {
            gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            ArcaneGFX.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
            other.gameObject.GetComponent<EnemyController>().TakeDamage(playerStats.chosenStats.attack * 1.5f);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
