using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHUDController : MonoBehaviour
{
    [Header("References")]
    public PlayerController playerController;
    [SerializeField] private ScriptablePlayerStats playerStats;

    [Header("Health, Skill and Ultimate Bar")]
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider skillSlider;
    [SerializeField] private Slider ultSlider;

    [Header("Skill and Ultimate ToolTip")]
    [SerializeField] private ToolTip skillToolTip;
    [SerializeField] private ToolTip ultToolTip;
    [SerializeField] private String skillName;
    [SerializeField] private String ultName;
    [SerializeField] private String skillDesc;
    [SerializeField] private String ultDesc;

    [Header("Skill and Ultimate Sprites")]
    [SerializeField] private Image skillIcon;
    [SerializeField] private Image ultIcon;
    [SerializeField] private Sprite[] skillSprites;
    [SerializeField] private Sprite[] ultSprites;

    void Start()
    {
        if (GameObject.FindWithTag("Player").TryGetComponent(out PlayerController componentPlayer))
        {
            playerController = componentPlayer;
        }

        GetSkillUltNames();
        SetToolTip();
    }

    void Update()
    {
        UpdateSliders();
    }

    public void GetSkillUltNames()
    {
        switch (playerStats.chosenClass)
        {
            case ScriptablePlayerStats.playerClass.Barbarian:
                skillIcon.sprite = skillSprites[0];
                ultIcon.sprite = ultSprites[0];

                skillName = "Frenzied Rage";
                skillDesc = "The player goes into a frenzied rage, increasing all damage dealt by the player but taking more damage while in this state.";
                
                ultName = "Earthsplitter";
                ultDesc = "The player swings their axe to rupture the ground in front of them, dealing a large amount of damage to enemies infront and stunning them briefly.";
                break;
            case ScriptablePlayerStats.playerClass.Paladin:
                skillIcon.sprite = skillSprites[1];
                ultIcon.sprite = ultSprites[1];

                skillName = "Holy Aura";
                skillDesc = "The player casts an aura around themself, increasing defense and dealing a medium amount of damage to nearby enemies.";
                
                ultName = "Divine Heal";
                ultDesc = "The player calls upon the divine spirits and seeks their aid in battle, healing the player and gaining a regenerative buff for a short period of time.";
                break;
            case ScriptablePlayerStats.playerClass.Archer:
                skillIcon.sprite = skillSprites[2];
                ultIcon.sprite = ultSprites[2];

                skillName = "Ranger Haste";
                skillDesc = "Increases attack speed.";
                
                ultName = "Yggdrasil Shot";
                ultDesc = "Draws back the bow and shoots 3 arrows that homes towards the nearest enemy. slowing targets hit for a period of time.";
                break;
            case ScriptablePlayerStats.playerClass.Mage:
                skillIcon.sprite = skillSprites[3];
                ultIcon.sprite = ultSprites[3];

                skillName = "Arcane Shot";
                skillDesc = "The player shoots a bolt of arcane energy in a form of a sphere, damaging enemies that it hits.";
                
                ultName = "Runic Fireball";
                ultDesc = "The player draws upon their arcane energies, casting a fireball with runic magic. Dealing a large amount of damage in a large lasting area.";
                break;
        }
    }

    public void SetToolTip()
    {
        skillToolTip.objName = skillName;
        skillToolTip.objType = ToolTip.types.Skill;
        skillToolTip.objDesc = skillDesc;

        ultToolTip.objName = ultName;
        ultToolTip.objType = ToolTip.types.Ultimate;
        ultToolTip.objDesc = ultDesc;
    }

    public void UpdateSliders()
    {
        healthBar.maxValue = playerStats.chosenStats.maxHealth;
        healthBar.value = playerStats.chosenStats.health;

        skillSlider.maxValue = playerController.GetMaxSkillCooldown();
        skillSlider.value = playerController.GetSkillCooldown();

        ultSlider.minValue = -playerController.GetMaxUltCharge();
        ultSlider.value = -playerController.GetUltCharge();
    }
}
