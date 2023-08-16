using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        transform.parent.SendMessage("PlayerInAttackRange", other, SendMessageOptions.DontRequireReceiver);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        transform.parent.SendMessage("PlayerExitAttackRange", other, SendMessageOptions.DontRequireReceiver);
    }
}
