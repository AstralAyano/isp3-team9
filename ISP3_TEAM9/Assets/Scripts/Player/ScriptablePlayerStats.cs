using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerStats
{
    public int health;
    public int maxHealth;
    public int attack;
    public int defense;
    public float attackInterval; //(attackSpeed/10) + 1. For timers in code
    public float attackSpeed; //(attackInterval - 1) * 10. For display
    public float moveSpeed;
    public float projectileSpeed; //For mage and archer
    public int exp;
    public int level;
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
        Paladin = 3,
        None = 4
    }

    public playerClass chosenClass; //Change this to change class
    public playerStats chosenBaseStats;
    public playerStats chosenStats;
    public playerStats chosenStatPoints;
    public playerStatMultipliers chosenStatMultipliers;

    //Dictionaries to store all class stats
    public Dictionary<playerClass, playerStats> baseStats = new()
    {
        { playerClass.Barbarian, new playerStats{ health = 80, maxHealth = 80, attack = 25, defense = 3, attackInterval = 0.3f, moveSpeed = 4, projectileSpeed = 0, exp = 0, level = 1} },
        { playerClass.Mage, new playerStats{ health = 60, maxHealth = 60, attack = 15, defense = 2, attackInterval = 1f, moveSpeed = 5, projectileSpeed = 4, exp = 0, level = 1} },
        { playerClass.Archer, new playerStats{ health = 60, maxHealth = 60, attack = 10, defense = 2, attackInterval = 0.7f, moveSpeed = 6, projectileSpeed = 5, exp = 0, level = 1} },
        { playerClass.Paladin, new playerStats{ health = 100, maxHealth = 100, attack = 7, defense = 5, attackInterval = 0.3f, moveSpeed = 4, projectileSpeed = 0, exp = 0, level = 1} },
        { playerClass.None, new playerStats { health = 1, maxHealth = 1, attack = 0, defense = 0, attackInterval = 1f, moveSpeed = 3, projectileSpeed = 0, exp = 0, level = 1} }
    };

    public Dictionary<playerClass, playerStats> currentStats = new()
    {
        { playerClass.Barbarian, new playerStats{ health = 80, maxHealth = 80, attack = 25, defense = 3, attackInterval = 0.3f, moveSpeed = 4, projectileSpeed = 0, exp = 0, level = 1} },
        { playerClass.Mage, new playerStats{ health = 60, maxHealth = 60, attack = 15, defense = 2, attackInterval = 1f, moveSpeed = 6, projectileSpeed = 4, exp = 0, level = 1} },
        { playerClass.Archer, new playerStats{ health = 60, maxHealth = 60, attack = 10, defense = 2, attackInterval = 0.7f, moveSpeed = 7, projectileSpeed = 5, exp = 0, level = 1} },
        { playerClass.Paladin, new playerStats{ health = 100, maxHealth = 100, attack = 7, defense = 5, attackInterval = 0.3f, moveSpeed = 5, projectileSpeed = 0, exp = 0, level = 1} },
        { playerClass.None, new playerStats { health = 1, maxHealth = 1, attack = 0, defense = 0, attackInterval = 1f, moveSpeed = 3, projectileSpeed = 0, exp = 0, level = 1} }
    };

    public Dictionary<playerClass, playerStats> currentStatPoints = new()
    {
        { playerClass.Barbarian, new playerStats { health = 0, defense = 0, attack = 0, attackSpeed = 0, moveSpeed = 0, projectileSpeed = 0 } },
        { playerClass.Mage, new playerStats { health = 0, defense = 0, attack = 0, attackSpeed = 0, moveSpeed = 0, projectileSpeed = 0 } },
        { playerClass.Archer, new playerStats { health = 0, defense = 0, attack = 0, attackSpeed = 0, moveSpeed = 0, projectileSpeed = 0 } },
        { playerClass.Paladin, new playerStats { health = 0, defense = 0, attack = 0, attackSpeed = 0, moveSpeed = 0, projectileSpeed = 0 } },
        { playerClass.None, new playerStats { health = 0, defense = 0, attack = 0, attackSpeed = 0, moveSpeed = 0, projectileSpeed = 0 } }
    };
    
    public Dictionary<playerClass, playerStatMultipliers> statMultipliers = new()
    {
        { playerClass.Barbarian, new playerStatMultipliers{ health = .2f, defense = .5f, attackPower = .2f, attackSpeed = .05f, moveSpeed = .025f, projectileSpeed = .2f} },
        { playerClass.Mage, new playerStatMultipliers{ health = .2f, defense = .2f, attackPower = .2f, attackSpeed = .05f, moveSpeed = .025f, projectileSpeed = .2f} },
        { playerClass.Archer, new playerStatMultipliers{ health = .2f, defense = .2f, attackPower = .2f, attackSpeed = .05f, moveSpeed = .025f, projectileSpeed = .2f} },
        { playerClass.Paladin, new playerStatMultipliers{ health = .2f, defense = .2f, attackPower = .2f, attackSpeed = .05f, moveSpeed = .025f, projectileSpeed = .2f} },
        { playerClass.None, new playerStatMultipliers { health = .2f, defense = .2f, attackPower = .2f, attackSpeed = .05f, moveSpeed = .025f, projectileSpeed = .2f } }
    };

    private void OnEnable()
    {
        chosenBaseStats = baseStats[chosenClass];
        chosenStats = currentStats[chosenClass];
        chosenStatPoints = currentStatPoints[chosenClass];
        chosenStatMultipliers = statMultipliers[chosenClass];
    }

    public void UpdateClass()
    {
        chosenBaseStats = baseStats[chosenClass];
        chosenStats = currentStats[chosenClass];
        chosenStatPoints = currentStatPoints[chosenClass];
        chosenStatMultipliers = statMultipliers[chosenClass];
    }
}
