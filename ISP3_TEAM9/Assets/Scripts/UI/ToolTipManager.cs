using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToolTipManager : MonoBehaviour
{
    public static ToolTipManager instance;

    public TextMeshProUGUI selectedName;
    public TextMeshProUGUI selectedType;
    public TextMeshProUGUI selectedDesc;

    private CanvasGroup cg;
    private bool fadeIn = false, fadeOut = false;

    private Color artefactColor;
    private Color consumableColor;
    private Color skillColor;
    private Color ultimateColor;

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

        ColorUtility.TryParseHtmlString("#cb7a09", out artefactColor);
        ColorUtility.TryParseHtmlString("#5E7AE0", out consumableColor);
        ColorUtility.TryParseHtmlString("#1ea6a8", out skillColor);
        ColorUtility.TryParseHtmlString("#1ea6a8", out ultimateColor);
    }

    void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(1.4f, 0.6f, 0);

        if (fadeIn)
        {
            cg.alpha += Time.unscaledDeltaTime * 4;

            if (cg.alpha >= 1.0f)
            {
                fadeIn = false;
            }
        }

        if (fadeOut)
        {
            cg.alpha -= Time.unscaledDeltaTime * 6;

            if (cg.alpha <= 0.0f)
            {
                fadeOut = false;
                gameObject.SetActive(false);
                selectedName.text = string.Empty;
                selectedDesc.text = string.Empty;
            }
        }
    }

    public void SetAndShowToolTip(string name, string type, string desc)
    {
        fadeOut = false;
        cg.alpha = 0.0f;
        gameObject.SetActive(true);
        
        selectedName.text = name;
        selectedDesc.text = desc;

        if (type == "Artefact")
        {
            selectedType.color = artefactColor;
        }
        else if (type == "Consumable")
        {
            selectedType.color = consumableColor;
        }
        else if (type == "Skill")
        {
            selectedType.color = skillColor;
        }
        else if (type == "Ultimate")
        {
            selectedType.color = ultimateColor;
        }

        if (type != "None")
        {
            selectedType.text = type;
        }
        else
        {
            selectedType.text = "";
        }

        fadeIn = true;
    }

    public void HideToolTip()
    {
        fadeIn = false;
        fadeOut = true;
    }
}
