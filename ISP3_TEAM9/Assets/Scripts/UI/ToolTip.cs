using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    public enum types
    {
        None,
        Artefact,
        Consumable
    }

    public string objName;
    public types objType;
    [TextArea] public string objDesc;

    private void OnMouseEnter()
    {
        ToolTipManager.instance.SetAndShowToolTip(objName, objType.ToString(), objDesc);
    }

    private void OnMouseExit()
    {
        ToolTipManager.instance.HideToolTip();
    }
}
