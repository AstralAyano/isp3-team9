using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHUDController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private ScriptablePlayerStats playerStats;

    [SerializeField] private Slider healthBar;

    void Start()
    {
        if (GameObject.FindWithTag("Player").TryGetComponent(out PlayerController componentPlayer))
        {
            playerController = componentPlayer;
        }
    }

    void Update()
    {
        healthBar.maxValue = playerStats.chosenStats.maxHealth;
        healthBar.value = playerStats.chosenStats.health;
    }
}
