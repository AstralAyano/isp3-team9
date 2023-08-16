using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable Item")]
public class Item : ScriptableObject
{
    public Sprite image;
    public bool stackable = true;

    public string itemName;

    [TextArea]
    public string itemDesc;

    //public ItemType type;
    //public ActionType actionType;
    //public Vector2Int range = new Vector2Int(5, 4);
}