using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using TMPro;

public class UIMenuController : MonoBehaviour
{
    [SerializeField] private CanvasGroup uiCanvasGroup;

    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text companyText;
    [SerializeField] private TMP_Text versionText;

    [SerializeField] private Light2D globalLight;

    [SerializeField] private float fadeDuration;

    private static Material textBaseMaterial;
    private static Material textHighlightMaterial;

    private bool fadeInLight = false;
    private bool fadeInTitle = false;
    private bool fadeInCompanyVer = false;
    private bool maxGlowReached = false;

    private float counter = 0;
    private float titleCounter = 0;

    void Awake()
    {
        uiCanvasGroup = GetComponent<CanvasGroup>();

        uiCanvasGroup.alpha = 0;

        titleText.alpha = 0;
        companyText.alpha = 0;
        versionText.alpha = 0;

        globalLight.intensity = 0;

        textBaseMaterial = titleText.fontSharedMaterial;
        textHighlightMaterial = new Material(textBaseMaterial);
        textHighlightMaterial.SetFloat(ShaderUtilities.ID_GlowPower, 0.75f);
    }

    void Start()
    {
        StartCoroutine(MenuFadeIn());
    }

    void Update()
    {
        FadeIn();

        TitleGlow();
    }

    private IEnumerator MenuFadeIn()
    {
        WaitForSeconds delay = new WaitForSeconds(0.5f);
        WaitForSeconds fade = new WaitForSeconds(fadeDuration);
        
        yield return delay;

        fadeInLight = true;

        yield return fade;

        fadeInTitle = true;

        yield return fade;

        fadeInCompanyVer = true;
    }

    void FadeIn()
    {
        if (fadeInLight)
        {
            counter += Time.deltaTime;

            if (globalLight.intensity < 0.5f)
            {
                globalLight.intensity = Mathf.Lerp(0, 0.5f, counter / fadeDuration);
            }
            else
            {
                uiCanvasGroup.alpha = 1;
                counter = 0;
                fadeInLight = false;
            }
        }
        
        if (fadeInTitle)
        {
            counter += Time.deltaTime;

            if (titleText.alpha < 1.0f)
            {
                titleText.alpha = Mathf.Lerp(0, 1, counter / fadeDuration);
            }
            else
            {
                counter = 0;
                fadeInTitle = false;
            }
        }
        
        if (fadeInCompanyVer)
        {
            counter += Time.deltaTime;

            if (companyText.alpha < 1.0f || versionText.alpha < 1.0f)
            {
                companyText.alpha = Mathf.Lerp(0, 1, counter / fadeDuration);
                versionText.alpha = Mathf.Lerp(0, 1, counter / fadeDuration);
            }
            else
            {
                counter = 0;
                fadeInCompanyVer = false;
            }
        }
    }

    void TitleGlow()
    {
        if (!maxGlowReached && titleCounter >= 0.4f)
        {
            maxGlowReached = true;
        }
        else if (maxGlowReached && titleCounter <= 0f)
        {
            StartCoroutine(DelayBeforeGlow());
        }

        if (maxGlowReached)
        {
            titleCounter -= Time.deltaTime * 0.25f;
            titleText.fontSharedMaterial.SetFloat(ShaderUtilities.ID_GlowPower, titleCounter);
            titleText.UpdateMeshPadding();
        }
        else if (!maxGlowReached)
        {
            titleCounter += Time.deltaTime * 0.25f;
            titleText.fontSharedMaterial.SetFloat(ShaderUtilities.ID_GlowPower, titleCounter);
            titleText.UpdateMeshPadding();
        }
    }

    private IEnumerator DelayBeforeGlow()
    {
        yield return new WaitForSeconds(1);

        maxGlowReached = false;
    }
}
