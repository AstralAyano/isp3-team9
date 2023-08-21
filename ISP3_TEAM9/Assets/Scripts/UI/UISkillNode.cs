using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillNode : MonoBehaviour
{
    [SerializeField]
    private List<UISkillNode> adjacentNodes = new List<UISkillNode>();
    private SpriteRenderer sr;

    private bool isActivatable = false;
    public bool isActivated = false;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (isActivated)
        {
            isActivatable = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < adjacentNodes.Count; i++)
        {
            if (adjacentNodes[i].isActivated)
            {
                isActivatable = true;
            }
        }
    }

    public void ActivateNode()
    {
        if (isActivatable)
        {
            isActivated = true;
        }
    }
}
