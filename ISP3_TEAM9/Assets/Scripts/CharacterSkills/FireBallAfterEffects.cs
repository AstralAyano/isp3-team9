using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallAfterEffects : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        transform.parent.SendMessage("FireBallAOE", other, SendMessageOptions.DontRequireReceiver);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        transform.parent.SendMessage("NotInAOE", other, SendMessageOptions.DontRequireReceiver);
    }
}
