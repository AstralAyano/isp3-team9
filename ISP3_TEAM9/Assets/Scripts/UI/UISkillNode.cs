using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillNode : MonoBehaviour
{
    [SerializeField]
    private List<UISkillNode> adjacentNodes = new List<UISkillNode>();
    private Button btn;

    [SerializeField]
    private bool isActivatable = false;
    public bool isActivated = false;

    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();

        if (isActivated)
        {
            for (int i = 0; i < adjacentNodes.Count; i++)
            {
                adjacentNodes[i].isActivatable = true;
            }
            ColorBlock cb = btn.colors;
            cb.normalColor = new Color32(150, 150, 150, 255);
            cb.highlightedColor = new Color32(150, 150, 150, 255);
            cb.selectedColor = new Color32(150, 150, 150, 255);
            btn.colors = cb;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateNode()
    {
        if (isActivatable)
        {
            isActivated = true;
            for (int i = 0; i < adjacentNodes.Count; i++)
            {
                adjacentNodes[i].isActivatable = true;
            }
            ColorBlock cb = btn.colors;
            cb.normalColor = new Color32(150, 150, 150, 255);
            cb.highlightedColor = new Color32(150, 150, 150, 255);
            cb.selectedColor = new Color32(150, 150, 150, 255);
            btn.colors = cb;
        }
    }
}
