using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerStats
{
    public int health;
    public int maxHealth;
    public int attack;
    public int defense;
    public float attackInterval; //Attack Speed in seconds
    public int moveSpeed;
    public int projectileSpeed; //For mage and archer
}

[CreateAssetMenu]
public class ScriptablePlayerStats : ScriptableObject
{
    public enum playerClass
    {
        Barbarian = 0,
        Mage = 1,
        Archer = 2,
        Paladin = 3
    }

    public playerClass chosenClass; //Change this to change class
    public playerStats chosenStats;

    //Dictionaries to store all class stats
    public Dictionary<playerClass, playerStats> baseStats = new Dictionary<playerClass, playerStats>()
    {
        { playerClass.Barbarian, new playerStats{ maxHealth = 80, attack = 10, defense = 2, attackInterval = 0.5f, moveSpeed = 4, projectileSpeed = 0} },
        { playerClass.Mage, new playerStats{ maxHealth = 60, attack = 15, defense = 1, attackInterval = 1.5f, moveSpeed = 6, projectileSpeed = 4} },
        { playerClass.Archer, new playerStats{ maxHealth = 60, attack = 10, defense = 1, attackInterval = 1f, moveSpeed = 7, projectileSpeed = 5} },
        { playerClass.Paladin, new playerStats{ maxHealth = 100, attack = 7, defense = 4, attackInterval = 0.5f, moveSpeed = 5, projectileSpeed = 0} }
    };

    public Dictionary<playerClass, playerStats> currentStats = new Dictionary<playerClass, playerStats>()
    {
        { playerClass.Barbarian, new playerStats{ health = 80, maxHealth = 80, attack = 10, defense = 2, attackInterval = 0.5f, moveSpeed = 4, projectileSpeed = 0} },
        { playerClass.Mage, new playerStats{ health = 60, maxHealth = 60, attack = 15, defense = 1, attackInterval = 1.5f, moveSpeed = 6, projectileSpeed = 4} },
        { playerClass.Archer, new playerStats{ health = 60, maxHealth = 60, attack = 10, defense = 1, attackInterval = 1f, moveSpeed = 7, projectileSpeed = 5} },
        { playerClass.Paladin, new playerStats{ health = 100, maxHealth = 100, attack = 7, defense = 4, attackInterval = 0.5f, moveSpeed = 5, projectileSpeed = 0} }
    };

    private void OnEnable()
    {
        chosenStats = currentStats[chosenClass];
    }
}
