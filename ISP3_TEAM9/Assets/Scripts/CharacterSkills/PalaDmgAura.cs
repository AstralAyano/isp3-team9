using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalaDmgAura : MonoBehaviour
{
    [SerializeField]
    private ScriptablePlayerStats playerStats;

    private float timer = 0f;

    private EnemyController enemy;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log(other.gameObject.GetComponent<EnemyController>().gameObject.name);
            other.gameObject.GetComponent<EnemyController>().TakeDamage(playerStats.chosenStats.attack);
        }
    }
}
