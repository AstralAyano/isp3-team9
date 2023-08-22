using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalaAuraDestroy : MonoBehaviour
{
    private float TimerSkillOver = 0f;

    // Update is called once per frame
    void Update()
    {
        TimerSkillOver += Time.deltaTime;
        if (TimerSkillOver >= 1.3f)
        {
            Destroy(gameObject);
        }
    }
}
