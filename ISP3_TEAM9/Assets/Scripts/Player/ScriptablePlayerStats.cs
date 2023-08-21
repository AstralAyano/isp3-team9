using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerStats
{
    public int health;
    public int maxHealth;
    public int attack;
    public int defense;
    public float attackInterval; //(float)(attackSpeed/10) + 1. For timers in code
    public int attackSpeed; //(int)(attackInterval - 1) * 10. For display
    public int moveSpeed;
    public int projectileSpeed; //For mage and archer
}

public class playerStatMultipliers
{
    public float health;
    public float defense;
    public float attackPower;
    public float attackSpeed;
    public float moveSpeed;
    public float projectileSpeed; //For mage and archer
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
    public playerStats chosenBaseStats;
    public playerStats chosenStats;
    public playerStats chosenStatPoints;
    public playerStatMultipliers chosenStatMultipliers;

    //Dictionaries to store all class stats
    public Dictionary<playerClass, playerStats> baseStats = new()
    {
        { playerClass.Barbarian, new playerStats{ maxHealth = 80, attack = 10, defense = 2, attackInterval = 0.5f, moveSpeed = 4, projectileSpeed = 0} },
        { playerClass.Mage, new playerStats{ maxHealth = 60, attack = 15, defense = 1, attackInterval = 1.5f, moveSpeed = 6, projectileSpeed = 4} },
        { playerClass.Archer, new playerStats{ maxHealth = 60, attack = 10, defense = 1, attackInterval = 1f, moveSpeed = 7, projectileSpeed = 5} },
        { playerClass.Paladin, new playerStats{ maxHealth = 100, attack = 7, defense = 4, attackInterval = 0.5f, moveSpeed = 5, projectileSpeed = 0} }
    };

    public Dictionary<playerClass, playerStats> currentStats = new()
    {
        { playerClass.Barbarian, new playerStats{ health = 80, maxHealth = 80, attack = 10, defense = 2, attackInterval = 0.5f, moveSpeed = 4, projectileSpeed = 0} },
        { playerClass.Mage, new playerStats{ health = 60, maxHealth = 60, attack = 15, defense = 1, attackInterval = 1.5f, moveSpeed = 6, projectileSpeed = 4} },
        { playerClass.Archer, new playerStats{ health = 60, maxHealth = 60, attack = 10, defense = 1, attackInterval = 1f, moveSpeed = 7, projectileSpeed = 5} },
        { playerClass.Paladin, new playerStats{ health = 100, maxHealth = 100, attack = 7, defense = 4, attackInterval = 0.5f, moveSpeed = 5, projectileSpeed = 0} }
    };

    public Dictionary<playerClass, playerStats> currentStatPoints = new()
    {
        { playerClass.Barbarian, new playerStats{ health = 1, defense = 1, attack = 1, attackSpeed = 1, moveSpeed = 1, projectileSpeed = 1} },
        { playerClass.Mage, new playerStats{ health = 1, defense = 1, attack = 1, attackSpeed = 1, moveSpeed = 1, projectileSpeed = 1} },
        { playerClass.Archer, new playerStats{ health = 1, defense = 1, attack = 1, attackSpeed = 1, moveSpeed = 1, projectileSpeed = 1} },
        { playerClass.Paladin, new playerStats{ health = 1, defense = 1, attack = 1, attackSpeed = 1, moveSpeed = 1, projectileSpeed = 1} }
    };
    
    public Dictionary<playerClass, playerStatMultipliers> statMultipliers = new()
    {
        { playerClass.Barbarian, new playerStatMultipliers{ health = 1f, defense = 1f, attackPower = 1f, attackSpeed = 1f, moveSpeed = 1f, projectileSpeed = 1f} },
        { playerClass.Mage, new playerStatMultipliers{ health = 1f, defense = 1f, attackPower = 1f, attackSpeed = 1f, moveSpeed = 1f, projectileSpeed = 1f} },
        { playerClass.Archer, new playerStatMultipliers{ health = 1f, defense = 1f, attackPower = 1f, attackSpeed = 1f, moveSpeed = 1f, projectileSpeed = 1f} },
        { playerClass.Paladin, new playerStatMultipliers{ health = 1f, defense = 1f, attackPower = 1f, attackSpeed = 1f, moveSpeed = 1f, projectileSpeed = 1f} }
    };

    private void OnEnable()
    {
        chosenBaseStats = baseStats[chosenClass];
        chosenStats = currentStats[chosenClass];
        chosenStatPoints = currentStatPoints[chosenClass];
        chosenStatMultipliers = statMultipliers[chosenClass];
    }
}
