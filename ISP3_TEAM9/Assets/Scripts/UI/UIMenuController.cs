using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using TMPro;
using Cinemachine;

public class UIMenuController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject lightObj;

    [Header("Canvas Group")]
    [SerializeField] private CanvasGroup uiCanvasGroup;

    [Header("Grid Group")]
    [SerializeField] private GameObject gridMenu;
    [SerializeField] private GameObject gridLoad;

    [Header("Text")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text companyText;
    [SerializeField] private TMP_Text versionText;
    
    [Header("Buttons")]
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject exitButton;

    [Header("Light")]
    [SerializeField] private Light2D globalLight;

    [Header("Variables")]
    [SerializeField] private float fadeDuration;

    private static Material textBaseMaterial;
    private static Material textHighlightMaterial;

    private bool fadeInLight = false;
    private bool fadeInTitle = false;
    private bool fadeInCompanyVer = false;
    private bool fadeInStartButton = false;
    private bool fadeInExitButton = false;
    public bool fadeOutLight = false;
    public bool fadeOutCanvasGroup = false;
    private bool maxGlowReached = false;

    private float counter = 0;
    private float titleCounter = 0;

    void Awake()
    {
        gridLoad.SetActive(false);
        gridMenu.SetActive(true);

        uiCanvasGroup = GetComponent<CanvasGroup>();

        titleText.alpha = 0;
        companyText.alpha = 0;
        versionText.alpha = 0;

        globalLight.intensity = 0;

        textBaseMaterial = titleText.fontSharedMaterial;
        textHighlightMaterial = new Material(textBaseMaterial);
        textHighlightMaterial.SetFloat(ShaderUtilities.ID_GlowPower, 0f);
    
        startButton.GetComponent<Button>().interactable = false;
        startButton.GetComponent<CanvasGroup>().alpha = 0;

        exitButton.GetComponent<Button>().interactable = false;
        exitButton.GetComponent<CanvasGroup>().alpha = 0;
    }

    void Start()
    {
        StartCoroutine(MenuFadeIn());
    }

    void Update()
    {
        FadeIn();
        FadeOut();

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

        fadeInStartButton = true;

        yield return fade;

        fadeInExitButton = true;

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
                globalLight.intensity = Mathf.Lerp(0f, 0.5f, counter / fadeDuration);
            }
            else
            {
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

        if (fadeInStartButton)
        {
            counter += Time.deltaTime;

            if (startButton.GetComponent<CanvasGroup>().alpha < 1.0f)
            {
                startButton.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, counter / fadeDuration);
            }
            else
            {
                counter = 0;
                fadeInStartButton = false;
            }
        }

        if (fadeInExitButton)
        {
            counter += Time.deltaTime;

            if (exitButton.GetComponent<CanvasGroup>().alpha < 1.0f)
            {
                exitButton.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, counter / fadeDuration);
            }
            else
            {
                counter = 0;
                fadeInExitButton = false;
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

                StartCoroutine(DelayBeforeTorch());
                
                startButton.GetComponent<Button>().interactable = true;
                exitButton.GetComponent<Button>().interactable = true;
            }
        }
    }

    void FadeOut()
    {
        if (fadeOutLight)
        {
            counter += Time.deltaTime;

            if (globalLight.intensity > 0f)
            {
                globalLight.intensity = Mathf.Lerp(0.5f, 0f, counter / fadeDuration);
            }
            else
            {
                counter = 0;
                fadeOutLight = false;
            }
        }

        if (fadeOutCanvasGroup)
        {
            counter += Time.deltaTime;

            if (uiCanvasGroup.alpha > 0.0f)
            {
                uiCanvasGroup.alpha = Mathf.Lerp(1, 0, counter / 0.5f);
            }
            else
            {
                counter = 0;
                fadeOutCanvasGroup = false;
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

    private IEnumerator DelayBeforeTorch()
    {
        yield return new WaitForSeconds(0.5f);
        
        lightObj.SetActive(true);
    }

    public IEnumerator DoorTouched()
    {
        fadeOutLight = true;

        yield return new WaitForSeconds(1.0f);

        gridMenu.SetActive(false);
        gridLoad.SetActive(true);

        GameObject.Find("Player").transform.position = new Vector3(0, -5, 0);

        fadeInLight = true;
        GameObject.Find("Player").GetComponent<PlayerMenuController>().lightStatus = 2;

        yield return new WaitForSeconds(1.0f);

        GameObject.Find("Player").GetComponent<SceneLoadMenuToLobby>().LoadScene("SceneLobby");
    }
}
