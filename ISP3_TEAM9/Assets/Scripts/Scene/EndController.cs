using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using TMPro;
using UnityEngine.SceneManagement;

public class EndController : MonoBehaviour
{
    [Header("Canvas Group")]
    [SerializeField] private CanvasGroup uiCanvasGroup;

    [Header("Grid Group")]
    [SerializeField] private GameObject gridMenu;

    [Header("Text")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text statsText;

    [Header("Buttons")]
    [SerializeField] private GameObject menuButton;
    [SerializeField] private GameObject exitButton;

    [Header("Light")]
    [SerializeField] private Light2D globalLight;

    [Header("Variables")]
    [SerializeField] private float fadeDuration;

    private static Material textBaseMaterial;
    private static Material textHighlightMaterial;

    private bool fadeInLight = false;
    private bool fadeInTitle = false;
    private bool fadeInStats = false;
    private bool fadeInMenuButton = false;
    private bool fadeInExitButton = false;
    public bool fadeOutLight = false;
    public bool fadeOutCanvasGroup = false;
    private bool maxGlowReached = false;

    private float counter = 0;
    private float titleCounter = 0;

    public static int timeSpent;
    public static int roomsCleared;

    void Awake()
    {
        GameObject gameTimer = FindObjectOfType<GameTimer>().gameObject;

        timeSpent = (int)GameTimer.timer;
        Destroy(gameTimer, 0);

        gridMenu.SetActive(true);

        uiCanvasGroup = uiCanvasGroup.GetComponent<CanvasGroup>();

        statsText.text = "	     Stats:\n" + 
                         "--------------------------\n" +
                         "Time Taken:\n" + DisplayTime() + "\n" +
                         "Rooms Cleared: " + roomsCleared + "\n" +
                         "" + "|\n" +
                         "--------------------------\n";

        titleText.alpha = 0;
        statsText.alpha = 0;

        textBaseMaterial = titleText.fontSharedMaterial;
        textHighlightMaterial = new Material(textBaseMaterial);
        textHighlightMaterial.SetFloat(ShaderUtilities.ID_GlowPower, 0f);

        menuButton.GetComponent<Button>().interactable = false;
        menuButton.GetComponent<CanvasGroup>().alpha = 0;

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

        fadeInMenuButton = true;

        yield return fade;

        fadeInExitButton = true;

        yield return fade;

        fadeInStats = true;
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

        if (fadeInMenuButton)
        {
            counter += Time.deltaTime;

            if (menuButton.GetComponent<CanvasGroup>().alpha < 1.0f)
            {
                menuButton.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, counter / fadeDuration);
            }
            else
            {
                counter = 0;
                fadeInMenuButton = false;
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

        if (fadeInStats)
        {
            counter += Time.deltaTime;

            if (statsText.alpha < 1.0f)
            {
                statsText.alpha = Mathf.Lerp(0, 1, counter / fadeDuration);
            }
            else
            {
                counter = 0;
                fadeInStats = false;

                menuButton.GetComponent<Button>().interactable = true;
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

    private string DisplayTime()
    {
        int totalTimeLeftSecs = timeSpent;
        //Get hours
        int hrs = totalTimeLeftSecs / 3600;
        //Subtract hours
        totalTimeLeftSecs -= hrs * 3600;
        //Get minutes
        int mins = totalTimeLeftSecs/60;
        //Subtract minutes
        totalTimeLeftSecs -= mins * 60;

        return hrs.ToString() + " hrs " + mins.ToString() + " mins " + totalTimeLeftSecs.ToString() + " secs ";
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("SceneMenu");
    }

}
