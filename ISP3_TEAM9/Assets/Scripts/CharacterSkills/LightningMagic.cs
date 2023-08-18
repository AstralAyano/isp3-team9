using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningMagic : MonoBehaviour
{

    private CircleCollider2D coll;

    public LayerMask enemyLayer;
    public float Damage;

    public GameObject ChainLightningEffect;

    public GameObject BeenStruck;

    public int AmountToChain;

    private GameObject StartObject;
    private GameObject EndObject;

    private Animator Ani;

    public ParticleSystem parti;

    private int SingleSpawns;   

    // Start is called before the first frame update
    void Start()
    {
        if (AmountToChain == 0)
        {
            Destroy(gameObject);
        }


        coll = GetComponent<CircleCollider2D>();

        Ani = GetComponent<Animator>();

        parti = GetComponent<ParticleSystem>();

        StartObject = gameObject;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (SingleSpawns != 0)
        { 
            if (enemyLayer == (enemyLayer | (1 << collision.gameObject.layer)) && !collision.GetComponentInChildren<EnemyStruckLC>())
            {
                EndObject = collision.gameObject;

                AmountToChain -= 1;
                Instantiate(ChainLightningEffect, collision.gameObject.transform.position, Quaternion.identity);

                Instantiate(BeenStruck, collision.gameObject.transform);

                collision.gameObject.GetComponent<EnemyController>().TakeDamage(Damage);

                Ani.StopPlayback();

                coll.enabled = false;

                SingleSpawns--;

                parti.Play();

                var emitParams = new ParticleSystem.EmitParams();
                emitParams.position = StartObject.transform.position;

                parti.Emit(emitParams, 1);

                emitParams.position = EndObject.transform.position;

                parti.Emit(emitParams, 1);

                emitParams.position = (StartObject.transform.position + EndObject.transform.position) / 2;

                Destroy(gameObject, 1f);
            }
        }
    }
}
