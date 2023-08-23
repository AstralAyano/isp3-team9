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

    [Header("Class Type")]
    public string classResonance;

    [Header("Stat Changes")]
    public int health;
    public int attack;
    public int defense;
    public int attackSpeed;
    public int moveSpeed;
    public int projectileSpeed;

    [Header("Stat Changes")]
    public int healthMulti;
    public int attackMulti;
    public int defenseMulti;
    public int attackSpeedMulti;
    public int moveSpeedMulti;
    public int projectileSpeedMulti;

    [Header("Item Percentage Buff")]
    public int HealPercent;
    public int AttackPercent;
    public int DefensePercent;
    public int MovementSpdPercent;
    public int AttackSpdPercent;

    //public ItemType type;
    //public ActionType actionType;
    //public Vector2Int range = new Vector2Int(5, 4);
}