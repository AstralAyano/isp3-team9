using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthSplitterDamage : MonoBehaviour
{
    [SerializeField]
    private ScriptablePlayerStats playerStats;

    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0.5f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log(other.gameObject.GetComponent<EnemyController>().gameObject.name);
            other.gameObject.GetComponent<EnemyController>().TakeDamage(playerStats.chosenStats.attack * 2);
        }
    }
}
