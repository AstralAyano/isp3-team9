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
    
    public void ActivateNode(int statToUpgrade)
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
            if (statToUpgrade >= 0 && statToUpgrade <= 5)
            {
                book.IncreaseStat(statToUpgrade);
            }
        }
    }
}
