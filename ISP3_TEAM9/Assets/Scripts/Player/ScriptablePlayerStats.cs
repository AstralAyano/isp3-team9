using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBaseStats
{
    public int health;
    public int maxHealth;
    public int attack;
    public int defense;
    public float attackInterval; //Attack Speed in seconds
    public int moveSpeed;
    public int projectileSpeed; //For mage and archer
}

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

public class playerStatPoints
{
    public int health;
    public int defense;
    public int attackPower;
    public int attackSpeed;
    public int moveSpeed;
    public int projectileSpeed; //For mage and archer
}

public class playerStatMultipliers
{
    public int health;
    public int defense;
    public int attackPower;
    public int attackSpeed;
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
    public playerBaseStats chosenBaseStats;
    public playerStats chosenStats;
    public playerStatPoints chosenStatPoints;
    public playerStatMultipliers chosenStatMultipliers;

    //Dictionaries to store all class stats
    public Dictionary<playerClass, playerBaseStats> baseStats = new Dictionary<playerClass, playerBaseStats>()
    {
        { playerClass.Barbarian, new playerBaseStats{ maxHealth = 80, attack = 10, defense = 2, attackInterval = 0.5f, moveSpeed = 4, projectileSpeed = 0} },
        { playerClass.Mage, new playerBaseStats{ maxHealth = 60, attack = 15, defense = 1, attackInterval = 1.5f, moveSpeed = 6, projectileSpeed = 4} },
        { playerClass.Archer, new playerBaseStats{ maxHealth = 60, attack = 10, defense = 1, attackInterval = 1f, moveSpeed = 7, projectileSpeed = 5} },
        { playerClass.Paladin, new playerBaseStats{ maxHealth = 100, attack = 7, defense = 4, attackInterval = 0.5f, moveSpeed = 5, projectileSpeed = 0} }
    };

    public Dictionary<playerClass, playerStats> currentStats = new Dictionary<playerClass, playerStats>()
    {
        { playerClass.Barbarian, new playerStats{ health = 80, maxHealth = 80, attack = 10, defense = 2, attackInterval = 0.5f, moveSpeed = 4, projectileSpeed = 0} },
        { playerClass.Mage, new playerStats{ health = 60, maxHealth = 60, attack = 15, defense = 1, attackInterval = 1.5f, moveSpeed = 6, projectileSpeed = 4} },
        { playerClass.Archer, new playerStats{ health = 60, maxHealth = 60, attack = 10, defense = 1, attackInterval = 1f, moveSpeed = 7, projectileSpeed = 5} },
        { playerClass.Paladin, new playerStats{ health = 100, maxHealth = 100, attack = 7, defense = 4, attackInterval = 0.5f, moveSpeed = 5, projectileSpeed = 0} }
    };

    public Dictionary<playerClass, playerStatPoints> currentStatPoints = new Dictionary<playerClass, playerStatPoints>()
    {
        { playerClass.Barbarian, new playerStatPoints{ health = 1, defense = 1, attackPower = 1, attackSpeed = 1, moveSpeed = 1, projectileSpeed = 1} },
        { playerClass.Mage, new playerStatPoints{ health = 1, defense = 1, attackPower = 1, attackSpeed = 1, moveSpeed = 1, projectileSpeed = 1} },
        { playerClass.Archer, new playerStatPoints{ health = 1, defense = 1, attackPower = 1, attackSpeed = 1, moveSpeed = 1, projectileSpeed = 1} },
        { playerClass.Paladin, new playerStatPoints{ health = 1, defense = 1, attackPower = 1, attackSpeed = 1, moveSpeed = 1, projectileSpeed = 1} }
    };
    
    public Dictionary<playerClass, playerStatMultipliers> statMultipliers = new Dictionary<playerClass, playerStatMultipliers>()
    {
        { playerClass.Barbarian, new playerStatMultipliers{ health = 1, defense = 1, attackPower = 1, attackSpeed = 1, moveSpeed = 1, projectileSpeed = 1} },
        { playerClass.Mage, new playerStatMultipliers{ health = 1, defense = 1, attackPower = 1, attackSpeed = 1, moveSpeed = 1, projectileSpeed = 1} },
        { playerClass.Archer, new playerStatMultipliers{ health = 1, defense = 1, attackPower = 1, attackSpeed = 1, moveSpeed = 1, projectileSpeed = 1} },
        { playerClass.Paladin, new playerStatMultipliers{ health = 1, defense = 1, attackPower = 1, attackSpeed = 1, moveSpeed = 1, projectileSpeed = 1} }
    };

    private void OnEnable()
    {
        chosenBaseStats = baseStats[chosenClass];
        chosenStats = currentStats[chosenClass];
        chosenStatPoints = currentStatPoints[chosenClass];
        chosenStatMultipliers = statMultipliers[chosenClass];
    }
}
