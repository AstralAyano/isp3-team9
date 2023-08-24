using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballUlt : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private float angleInRad;
    private Vector2 dir;

    private float LastEF = 0.0f;
    private float DmgOverTimeTimer = 0.0f; 
    private bool IsExploded;
    private bool ActivateTimer = false;

    [SerializeField]
    private GameObject secondCollider;

    [SerializeField]
    private GameObject FireBallGFX;

    [SerializeField]
    private GameObject FireBallGFX2;

    [SerializeField]
    private ScriptablePlayerStats playerStats;

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
        rb.AddForce(dir * playerStats.chosenStats.projectileSpeed, ForceMode2D.Impulse);
        if (ActivateTimer)
        {
            LastEF += Time.deltaTime;
            if (LastEF > 5f)
            {
                Debug.Log("die fire part 2");
                Destroy(gameObject);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            IsExploded = true;
            secondCollider.SetActive(true);
            FireBallGFX.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
            FireBallGFX2.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.3f);
            ActivateTimer = true;
            Debug.Log("die fire part 1");
            other.gameObject.GetComponent<EnemyController>().TakeDamage(playerStats.chosenStats.attack * 2);
        }
        if (other.gameObject.CompareTag("Wall"))
        {
            gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            IsExploded = true;
            secondCollider.SetActive(true);
            FireBallGFX.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
            FireBallGFX2.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.3f);
            ActivateTimer = true;
            Debug.Log("die fire part 1");
        }
    }

    private void FireBallAOE(Collider2D other)
    {
        if (IsExploded)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                DmgOverTimeTimer += Time.deltaTime;
                if (DmgOverTimeTimer >= 1.5f)
                {
                    other.gameObject.GetComponent<EnemyController>().TakeDamage(5);
                    DmgOverTimeTimer = 0;
                }
            }
            //do the if gameobject.comparetag<"enemy">
            //if enemy, set a timer for enemy to dmg over time
        }

    }
    private void NotInAOE(Collider2D other)
    {
        if (IsExploded)
        {
            //stop dmg over time LLLLLL
        }
    }
}
