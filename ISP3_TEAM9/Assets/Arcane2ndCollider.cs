using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arcane2ndCollider : MonoBehaviour
{
    Vector3 EnemyPos;

    private int Limit = 0;

    [SerializeField]
    private GameObject ArcaneShotPrefab;

    private void Update()
    {
        if (Limit == 0)
        {
            Instantiate(ArcaneShotPrefab, EnemyPos, Quaternion.identity);
            Limit = 1;
        }
        else if (Limit == 1)
        {
            Destroy(gameObject);
        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyPos = other.gameObject.transform.position;
            
        }

    }
}
