using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerStats
{
    public int health;
    public int attack;
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

    //Dictionary to store all class stats
    public Dictionary<playerClass, playerStats> stats = new Dictionary<playerClass, playerStats>()
    {
        { playerClass.Barbarian, new playerStats{ health = 80, attack = 10, attackInterval = 0.5f, moveSpeed = 4, projectileSpeed = 0} },
        { playerClass.Mage, new playerStats{ health = 60, attack = 15, attackInterval = 1.5f, moveSpeed = 7, projectileSpeed = 100} },
        { playerClass.Archer, new playerStats{ health = 60, attack = 10, attackInterval = 1f, moveSpeed = 8, projectileSpeed = 100} },
        { playerClass.Paladin, new playerStats{ health = 100, attack = 7, attackInterval = 0.5f, moveSpeed = 5, projectileSpeed = 0} }
    };

    private void OnEnable()
    {
        chosenStats = stats[chosenClass];
    }
}
