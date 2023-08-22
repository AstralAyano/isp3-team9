using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillNode : MonoBehaviour
{
    [SerializeField]
    private UIBookController book;
    [SerializeField]
    private List<UISkillNode> adjacentNodes = new List<UISkillNode>();
    private Button btn;

    [SerializeField] private int statIndex;

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
}
