using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using System;

public class UIBookController : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private ScriptablePlayerStats playerStats;
    [SerializeField] private TMP_Text[] statsValueText;
    [SerializeField] private int[] statPoints;
    [SerializeField] private float[] statsValue;
    [SerializeField] private float[] statsBaseValue;
    [SerializeField] private float[] statsMultiplier;
    [SerializeField] private TMP_Text[] statusValueText;
    [SerializeField] private float[] statusValue;
    [SerializeField] private TMP_Text classText;
    [SerializeField] private int statPointAmt;
    [SerializeField] private TMP_Text StatPointsText;

    [Header("References")]
    [SerializeField] private Animator animController;
    [SerializeField] private Button[] rightButtons;
    [SerializeField] private Button[] leftButtons;
    [SerializeField] private GameObject[] rightIcons;
    [SerializeField] private Slider[] statusSliders;
    [SerializeField] private GridLayoutGroup statsGroup;
    [SerializeField] private GameObject projectileStat;

    [Header("Pages")]
    [SerializeField] private GameObject statusPage;
    [SerializeField] private GameObject inventoryPage;
    [SerializeField] private GameObject skillPage;
    [SerializeField] private GameObject settingPage;

    [Header("Sprites")]
    [SerializeField] private Image equipmentPlayer;
    [SerializeField] private Sprite[] playerSprites;
    [SerializeField] private Sprite[] bookmark;

    [Header("Variables")]
    [SerializeField] private string currPage = "Status";
    [SerializeField] private Vector2 buttonStartPos;
    [SerializeField] private Vector2[] buttonCurrPos;
    [SerializeField] private Vector2 iconStartPos;
    [SerializeField] private Vector2[] iconCurrPos;

    [Header("Settings")]
    [SerializeField] private Resolution[] resolutions;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private List<Resolution> filteredResolutions;
    [Space(10)]
    [SerializeField] private Toggle fullscreenToggle;
    [Space(10)]
    [SerializeField] private Slider brightnessSlider;
    private Volume globalBrightness;
    [Space(10)]
    [SerializeField] private TMP_Text volumeText;
    [SerializeField] private Slider volumeSlider;

    [Header("Settings Variables")]
    [SerializeField] public int defaultResolution;
    [SerializeField] private bool defaultFullscreen = true;
    [SerializeField] private float defaultVolume = 0.5f;
    [SerializeField] private float currentVolume;

    private int currPageNo = 0;
    private int nextPageNo = 0;
    private float xOffset = 30;

    void Awake()
    {
        currentVolume = defaultVolume;

        int currResolutionIndex = 0;
        float currRefreshRate;

        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        currRefreshRate = Screen.currentResolution.refreshRate;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRate == currRefreshRate)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();

        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + " x " + filteredResolutions[i].height + " [" + filteredResolutions[i].refreshRate + " Hz]";
            options.Add(resolutionOption);

            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
            {
                currResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        defaultResolution = currResolutionIndex;

        if (GameObject.FindWithTag("PostProcessor").TryGetComponent(out Volume componentVol))
        {
            globalBrightness = componentVol;
        }
    }

    void Start()
    {
        animController = GetComponentInChildren<Animator>();

        rightButtons[0].image.sprite = bookmark[0];

        for (int i = 1; i < rightButtons.Length; i++)
        {
            rightButtons[i].image.sprite = bookmark[1];
            leftButtons[i - 1].gameObject.SetActive(false);
        }

        currPageNo = 1;

        GetPlayerStats();
        SetPlayerStatsInBook();
        UpdateStatusBars();
        statsBaseValue[0] = statsValue[6];
    }

    void GetPlayerStats()
    {
        switch (playerStats.chosenClass)
        {
            case ScriptablePlayerStats.playerClass.Barbarian:
                equipmentPlayer.sprite = playerSprites[0];
                HideProjectileSpeedStat();
                break;
            case ScriptablePlayerStats.playerClass.Paladin:
                equipmentPlayer.sprite = playerSprites[1];
                HideProjectileSpeedStat();
                break;
            case ScriptablePlayerStats.playerClass.Archer:
                equipmentPlayer.sprite = playerSprites[2];
                ShowProjectileSpeedStat();
                break;
            case ScriptablePlayerStats.playerClass.Mage:
                equipmentPlayer.sprite = playerSprites[3];
                ShowProjectileSpeedStat();
                break;
        }

        // Get base player stat values
        playerStats.chosenBaseStats = playerStats.baseStats[playerStats.chosenClass];
        statsBaseValue[0] = playerStats.chosenBaseStats.health;
        statsBaseValue[1] = playerStats.chosenBaseStats.defense;
        statsBaseValue[2] = playerStats.chosenBaseStats.attack;
        statsBaseValue[3] = playerStats.chosenBaseStats.attackInterval;
        statsBaseValue[4] = playerStats.chosenBaseStats.moveSpeed;
        statsBaseValue[5] = playerStats.chosenBaseStats.projectileSpeed;
        // Get current player stat values
        playerStats.chosenStats = playerStats.currentStats[playerStats.chosenClass];
        statsValue[0] = playerStats.chosenStats.health;
        statsValue[1] = playerStats.chosenStats.defense;
        statsValue[2] = playerStats.chosenStats.attack;
        statsValue[3] = playerStats.chosenStats.attackInterval;
        statsValue[4] = playerStats.chosenStats.moveSpeed;
        statsValue[5] = playerStats.chosenStats.projectileSpeed;
        statsValue[6] = playerStats.chosenStats.maxHealth;
        // Get stat point multipliers
        playerStats.chosenStatMultipliers = playerStats.statMultipliers[playerStats.chosenClass];
        statsMultiplier[0] = playerStats.chosenStatMultipliers.health;
        statsMultiplier[1] = playerStats.chosenStatMultipliers.defense;
        statsMultiplier[2] = playerStats.chosenStatMultipliers.attackPower;
        statsMultiplier[3] = playerStats.chosenStatMultipliers.attackSpeed;
        statsMultiplier[4] = playerStats.chosenStatMultipliers.moveSpeed;
        statsMultiplier[5] = playerStats.chosenStatMultipliers.projectileSpeed;
        // Get stat points
        playerStats.chosenStatPoints = playerStats.currentStatPoints[playerStats.chosenClass];
        statPoints[0] = playerStats.chosenStatPoints.health;
        statPoints[1] = playerStats.chosenStatPoints.defense;
        statPoints[2] = playerStats.chosenStatPoints.attackPower;
        statPoints[3] = playerStats.chosenStatPoints.attackSpeed;
        statPoints[4] = playerStats.chosenStatPoints.moveSpeed;
        statPoints[5] = playerStats.chosenStatPoints.projectileSpeed;

        statusValue[0] = statsValue[6];

        for (int i = 1; i < statusSliders.Length; i++)
        {
            statusSliders[i].maxValue = statusValue[i];
        }
    }

    void SetPlayerStatsInBook() 
    {
        classText.text = "Class : " + playerStats.chosenClass.ToString();

        for (int i = 0; i < statsValueText.Length; i++)
        {
            statsValueText[i].text = statPoints[i].ToString();
        }

        if (statPointAmt == 1)
        {
            StatPointsText.gameObject.transform.parent.gameObject.SetActive(true);
            StatPointsText.text = "You have " + statPointAmt + " Stat Point Available !";
        }
        else if (statPointAmt > 1)
        {
            StatPointsText.gameObject.transform.parent.gameObject.SetActive(true);
            StatPointsText.text = "You have " + statPointAmt + " Stat Points Available !";
        }
        else
        {
            StatPointsText.gameObject.transform.parent.gameObject.SetActive(false);
        }
    }

    public void IncreaseStat(int type) // Adds a stat point to a stat and updates the stat value
    {
        if (statPointAmt > 0)
        {
            statPointAmt--;
            statPoints[type]++;

            // Update stat value
            statsValue[type] += statPoints[type] * statsMultiplier[type] * statsBaseValue[type];
            // If increased HP, increase Max HP too
            if (type == 0)
            {
                statsValue[6] += statPoints[type] * statsMultiplier[type] * statsBaseValue[type];
                statusSliders[0].value = statsValue[0];
                statusSliders[0].maxValue = statsValue[6];
            }
            SetPlayerStatsInBook();
            UpdateStatusBars();
        }
    }

    void UpdateStatusBars()
    {
        statusValueText[0].text = statusSliders[0].value.ToString() + "/" + statusSliders[0].maxValue.ToString();
        for (int i = 1; i < statusSliders.Length; i++)
        {
            statusSliders[i].value = playerStats.chosenStats.health;
            statusValueText[i].text = playerStats.chosenStats.health.ToString() + "/" + statusSliders[i].maxValue.ToString();
        }
    }

    void HideProjectileSpeedStat()
    {
        statsGroup.spacing = new Vector2(0, 22.5f);
        projectileStat.SetActive(false);
    }

    void ShowProjectileSpeedStat()
    {
        statsGroup.spacing = new Vector2(0, 10);
        projectileStat.SetActive(true);
    }

    public void BookButtons(int page_type)
    {
        statusPage.SetActive(false);
        inventoryPage.SetActive(false);
        skillPage.SetActive(false);
        settingPage.SetActive(false);

        GetPlayerStats();
        SetPlayerStatsInBook();
        UpdateStatusBars();

        int page = page_type / 10;
        int type = page_type % 10;

        nextPageNo = page;

        switch (page)
        {
            case 1:
                currPage = "Status";
                break;
            case 2:
                currPage = "Inventory";
                break;
            case 3:
                currPage = "Skill";
                break;
            case 4:
                currPage = "Setting";
                break;
        }

        StartCoroutine(AnimationStart(type));
    }

    private IEnumerator AnimationStart(int dir)
    {
        HideAllButtons(true);

        for (int i = 0; i < rightButtons.Length; i++)
        {
            buttonCurrPos[i] = rightButtons[i].GetComponent<RectTransform>().anchoredPosition;
            buttonCurrPos[i].Set(buttonStartPos.x, buttonCurrPos[i].y);
            rightButtons[i].gameObject.GetComponent<RectTransform>().anchoredPosition = buttonCurrPos[i];

            iconCurrPos[i] = rightIcons[i].GetComponent<RectTransform>().anchoredPosition;
            iconCurrPos[i].Set(iconStartPos.x, iconCurrPos[i].y);
            rightIcons[i].GetComponent<RectTransform>().anchoredPosition = iconCurrPos[i];
        }

        int timeToFlip = Mathf.Abs(nextPageNo - currPageNo);

        for (int i = 0; i < timeToFlip; i++)
        {
            animController.SetFloat("SpeedMultiplier", timeToFlip);

            switch (dir)
            {
                case 0:
                    animController.SetTrigger("flipLeft");
                    break;
                case 1:
                    animController.SetTrigger("flipRight");
                    break;
            }

            yield return new WaitForSeconds(1f / timeToFlip);
        }

        switch (currPage)
        {
            case "Status":
                rightButtons[0].gameObject.transform.localPosition -= new Vector3(xOffset, 0, 0);
                rightIcons[0].transform.localPosition += new Vector3(27.5f, 0, 0);

                ShowAllButtons(false);

                rightButtons[0].image.sprite = bookmark[0];
                rightButtons[1].image.sprite = bookmark[1];
                rightButtons[2].image.sprite = bookmark[1];
                rightButtons[3].image.sprite = bookmark[1];
                
                statusPage.SetActive(true);

                currPageNo = 1;
                break;
            case "Inventory":
                rightButtons[1].gameObject.transform.localPosition -= new Vector3(xOffset, 0, 0);
                rightIcons[1].transform.localPosition += new Vector3(27.5f, 0, 0);

                ShowAllButtons(false);

                rightButtons[0].gameObject.SetActive(false);
                rightButtons[1].image.sprite = bookmark[0];
                rightButtons[2].image.sprite = bookmark[1];
                rightButtons[3].image.sprite = bookmark[1];

                leftButtons[0].gameObject.SetActive(true);

                inventoryPage.SetActive(true);
                
                currPageNo = 2;
                break;
            case "Skill":
                rightButtons[2].gameObject.transform.localPosition -= new Vector3(xOffset, 0, 0);
                rightIcons[2].transform.localPosition += new Vector3(27.5f, 0, 0);
                
                ShowAllButtons(false);

                rightButtons[0].gameObject.SetActive(false);
                rightButtons[1].gameObject.SetActive(false);
                rightButtons[2].image.sprite = bookmark[0];
                rightButtons[3].image.sprite = bookmark[1];

                leftButtons[0].gameObject.SetActive(true);
                leftButtons[1].gameObject.SetActive(true);

                skillPage.SetActive(true);
                
                currPageNo = 3;
                break;
            case "Setting":
                rightButtons[3].gameObject.transform.localPosition -= new Vector3(xOffset, 0, 0);
                rightIcons[3].transform.localPosition += new Vector3(27.5f, 0, 0);

                ShowAllButtons(false);

                rightButtons[0].gameObject.SetActive(false);
                rightButtons[1].gameObject.SetActive(false);
                rightButtons[2].gameObject.SetActive(false);
                rightButtons[3].image.sprite = bookmark[0];

                leftButtons[0].gameObject.SetActive(true);
                leftButtons[1].gameObject.SetActive(true);
                leftButtons[2].gameObject.SetActive(true);

                settingPage.SetActive(true);
                
                currPageNo = 4;
                break;
        }
    }

    void HideAllButtons(bool includeLeft)
    {
        for (int i = 0; i < rightButtons.Length; i++)
        {
            rightButtons[i].gameObject.SetActive(false);

            if (includeLeft && i < leftButtons.Length)
            {
                leftButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void ShowAllButtons(bool includeLeft)
    {
        for (int i = 0; i < rightButtons.Length; i++)
        {
            rightButtons[i].gameObject.SetActive(true);

            if (includeLeft && i < leftButtons.Length)
            {
                leftButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void HUDButtons(int page)
    {
        statusPage.SetActive(false);
        inventoryPage.SetActive(false);
        skillPage.SetActive(false);
        settingPage.SetActive(false);

        GetPlayerStats();
        SetPlayerStatsInBook();
        UpdateStatusBars();

        switch (page)
        {
            case 1:
                currPage = "Status";
                break;
            case 2:
                currPage = "Inventory";
                break;
            case 3:
                currPage = "Skill";
                break;
            case 4:
                currPage = "Setting";
                break;
        }

        currPageNo = page;
        nextPageNo = page;

        StartCoroutine(AnimationStart(0));
    }

    public void ResolutionApply(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];

        PlayerPrefs.SetInt("resolutionIndex", resolutionIndex);
        PlayerPrefs.SetInt("resolutionWidth", resolution.width);
        PlayerPrefs.SetInt("resolutionHeight", resolution.height);
        Debug.Log(PlayerPrefs.GetInt("resolutionIndex"));

        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        Debug.Log("Resolution Applied : " + resolutionIndex);
    }

    public void FullscreenApply()
    {
        Screen.fullScreen = fullscreenToggle.isOn;
        Debug.Log("Fullscreen Applied : " + fullscreenToggle.isOn);
    }

    public void BrightnessApply()
    {
        ColorAdjustments colorAdjust;

        if (globalBrightness.profile.TryGet<ColorAdjustments>(out colorAdjust))
        {
            colorAdjust.postExposure.value = brightnessSlider.value;
        }
    }

    public void ResetButton(string settingType)
    {
        if (settingType == "Graphics")
        {
            Resolution resolution = filteredResolutions[defaultResolution];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

            resolutionDropdown.value = defaultResolution;
            resolutionDropdown.RefreshShownValue();

            fullscreenToggle.isOn = defaultFullscreen;

            Debug.Log($"Reset Applied : {resolution.width} x {resolution.height} : {fullscreenToggle.isOn}");
        }
    }
}
