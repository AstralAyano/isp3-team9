using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToolTipManager : MonoBehaviour
{
    public static ToolTipManager instance;

    public TextMeshProUGUI selectedName;
    public TextMeshProUGUI selectedDesc;

    private CanvasGroup cg;
    private bool fadeIn = false, fadeOut = false;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        gameObject.SetActive(false);
        cg = gameObject.GetComponent<CanvasGroup>();
        cg.alpha = 0.0f;
    }

    void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(1.75f, 0.75f, 0);

        if (fadeIn)
        {
            cg.alpha += Time.deltaTime * 4;

            if (cg.alpha >= 1.0f)
            {
                fadeIn = false;
            }
        }

        if (fadeOut)
        {
            cg.alpha -= Time.deltaTime * 6;

            if (cg.alpha <= 0.0f)
            {
                fadeOut = false;
                gameObject.SetActive(false);
                selectedName.text = string.Empty;
                selectedDesc.text = string.Empty;
            }
        }
    }

    public void SetAndShowToolTip(string name, string desc)
    {
        fadeOut = false;
        cg.alpha = 0.0f;
        gameObject.SetActive(true);
        selectedName.text = name;
        selectedDesc.text = desc;
        fadeIn = true;
    }

    public void HideToolTip()
    {
        fadeIn = false;
        fadeOut = true;
    }
}
