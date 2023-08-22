using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalaDmgAura : MonoBehaviour
{
    private float TimerSkillOver = 0f;

    void Start()
    {
        TimerSkillOver = 2;
    }
    // Update is called once per frame
    void Update()
    {
        TimerSkillOver -= Time.deltaTime;
        if (TimerSkillOver <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyController>().TakeDamage(15);
        }

    }
}
