using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyExplosion : MonoBehaviour
{
    private float DestroyTimer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("Start Explosion Destroy");
    }

    // Update is called once per frame
    void Update()
    {
        DestroyTimer += Time.deltaTime;
        if (DestroyTimer >= 1f)
        {
            Destroy(gameObject);
            Debug.Log("Destroy Explosion");
        }
    }


}
