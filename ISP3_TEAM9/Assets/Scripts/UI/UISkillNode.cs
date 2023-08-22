using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISkillNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private UIBookController book;
    [SerializeField]
    private List<UISkillNode> adjacentNodes = new List<UISkillNode>();
    private Button btn;

    [SerializeField] private int statIndex;
    [SerializeField] private string statName;
    [SerializeField] private string statDesc;

    [SerializeField] private Sprite[] spriteLists;
    [SerializeField] private Image[] childrenImage;

    [SerializeField]
    private bool isActivatable = false;
    public bool isActivated = false;

    void Start()
    {
        btn = GetComponent<Button>();

        // for first node
        if (isActivated)
        {
            for (int i = 0; i < adjacentNodes.Count; i++)
            {
                adjacentNodes[i].isActivatable = true;
            }
            ColorBlock cb = btn.colors;
            Color selectedColor = new Color32(0, 255, 0, 255);
            cb.normalColor = selectedColor;
            cb.highlightedColor = selectedColor;
            cb.selectedColor = selectedColor;
            cb.pressedColor = selectedColor;
            btn.colors = cb;
            book.statPointAmt++;
        }

        childrenImage = GetComponentsInChildren<Image>();

        for (int i = 0; i < childrenImage.Length; i++)
        {
            if (childrenImage[i].gameObject != this.gameObject)
            {
                childrenImage[i].sprite = spriteLists[statIndex];
            }
        }

        CheckNode();
    }

    void Update()
    {
        if (isActivatable && !isActivated)
        {
            ColorBlock cb = btn.colors;
            Color unlockedColor = new Color32(220, 220, 220, 255);
            cb.normalColor = unlockedColor;
            cb.highlightedColor = new Color32(200, 200, 200, 255);
            cb.selectedColor = unlockedColor;                           
            btn.colors = cb;
        }
    }
    
    public void ActivateNode()
    {
        if (isActivatable && !isActivated)
        {
            isActivated = true;
            for (int i = 0; i < adjacentNodes.Count; i++)
            {
                adjacentNodes[i].isActivatable = true;
            }
            ColorBlock cb = btn.colors;
            Color selectedColor = new Color32(0, 255, 0, 255);
            cb.normalColor = selectedColor;
            cb.highlightedColor = selectedColor;
            cb.selectedColor = selectedColor;
            cb.pressedColor = selectedColor;
            btn.colors = cb;

            book.statPointAmt++;
            if (statIndex >= 0 && statIndex <= 5)
            {
                book.IncreaseStat(statIndex);
            }
        }
    }

    void CheckNode()
    {
        switch (statIndex)
        {
            case 0:
                statName = "Health Node";
                statDesc = "Activating this node increases the player's max health.";
                break;
            case 1:
                statName = "Defense Node";
                statDesc = "Activating this node increases the player's defensive capabilities.";
                break;
            case 2:
                statName = "Attack Node";
                statDesc = "Activating this node increases the player's offensive capabilities.";
                break;
            case 3:
                statName = "Attack Speed Node";
                statDesc = "Activating this node increases the player's attack speed.";
                break;
            case 4:
                statName = "Movement Speed Node";
                statDesc = "Activating this node increases the player's movement speed.";
                break;
            case 5:
                statName = "Projectile Speed Node";
                statDesc = "Activating this node increases the player's projectile speed.";
                break;
            case 6:
                statName = "Stat Point Node";
                statDesc = "Activating this node provides 1 stat point to the player.";
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        book.ShowNodeNameDesc(statName, statDesc);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        book.ResetNodeNameDesc();
    }
}
