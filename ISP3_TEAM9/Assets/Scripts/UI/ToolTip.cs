using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    public string objName;
    [TextArea] public string objDesc;

    private void OnMouseEnter()
    {
        ToolTipManager.instance.SetAndShowToolTip(objName, objDesc);
    }

    private void OnMouseExit()
    {
        ToolTipManager.instance.HideToolTip();
    }
}
