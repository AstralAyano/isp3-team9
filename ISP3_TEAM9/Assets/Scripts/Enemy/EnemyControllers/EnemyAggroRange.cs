using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroRange : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Enter Aggro");
        transform.parent.SendMessage("PlayerWithinAggro", other, SendMessageOptions.DontRequireReceiver);
        /*Transform audManager = GameObject.Find("SceneLoader").GetComponent<Transform>();
        audManager.SendMessage("InCombatRange", other, SendMessageOptions.DontRequireReceiver);*/
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("Exit Aggro");
        transform.parent.SendMessage("PlayerExitAggro", other, SendMessageOptions.DontRequireReceiver);
        /*Transform audManager = GameObject.Find("SceneLoader").GetComponent<Transform>();
        audManager.SendMessage("NotInCombatRange", other, SendMessageOptions.DontRequireReceiver);*/
    }
}
