using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable Item")]
public class Item : ScriptableObject
{
    public enum types
    {
        Artefact,
        Consumable
    }

    public Sprite image;
    public bool stackable = true;

    public types itemType;
    public string itemName;
    [TextArea] public string itemDesc;

    [Header("Stat Changes")]
    public int health;
    public int maxHealth;
    public int attack;
    public int defense;
    public int attackSpeed;
    public int moveSpeed;
    public int projectileSpeed;

    //public ItemType type;
    //public ActionType actionType;
    //public Vector2Int range = new Vector2Int(5, 4);
}