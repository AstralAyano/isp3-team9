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
using UnityEngine.Audio;

public class UIBookController : MonoBehaviour
{
    [Header("Player Stats")]
    public PlayerController playerController;
    [SerializeField] private ScriptablePlayerStats playerStats;
    [SerializeField] private TMP_Text[] statsValueText;
    [SerializeField] private TMP_Text[] statusValueText;
    [SerializeField] private float[] statusValue;
    [SerializeField] private TMP_Text classText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] public int statPointAmt;
    [SerializeField] public int skillPointAmt;
    [SerializeField] public int maxExp;
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

    [Header("Skills")]
    [SerializeField] private TMP_Text nodeNameText;
    [SerializeField] private TMP_Text nodeDescText;

    [Header("Settings")]
    [SerializeField] private Resolution[] resolutions;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private List<Resolution> filteredResolutions;
    [Space(10)]
    [SerializeField] private Toggle fullscreenToggle;
    [Space(10)]
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private Volume globalBrightness;
    //[Space(10)]

    [Header("Settings Variables")]
    [SerializeField] public int defaultResolution;

    [Header("Unlockable Artefact Slots")]
    [SerializeField] private GameObject[] newArtefactSlots;

    private int currPageNo = 0;
    private int nextPageNo = 0;
    private float xOffset = 30;
    
    void Awake()
    {
        UIController.floorNum = 1;

        int currResolutionIndex = 0;
        float currRefreshRate;

        // Get and store all available resolutions that the monitor supports
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        // Resets the dropdown options in cases of monitor changes, and get the refresh rate of the current monitor
        resolutionDropdown.ClearOptions();
        currRefreshRate = Screen.currentResolution.refreshRate;

        // To filter the resolutions that allows the same refresh rate as the monitor
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRate == currRefreshRate)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();

        // To add the filtered resolution to a list and find the default resolution based on the screen size and refresh rate
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + " x " + filteredResolutions[i].height + " [" + filteredResolutions[i].refreshRate + " Hz]";
            options.Add(resolutionOption);

            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
            {
                currResolutionIndex = i;
            }
        }

        // Adds the list of filtered resolutions to the dropdown options
        resolutionDropdown.AddOptions(options);
        if (PlayerPrefs.HasKey("resolutionIndex"))
        {
            resolutionDropdown.value = PlayerPrefs.GetInt("resolutionIndex");
        }
        else
        {
            resolutionDropdown.value = currResolutionIndex;
        }
        resolutionDropdown.RefreshShownValue();

        // Sets the default resolution found in the for loop
        defaultResolution = currResolutionIndex;

        // Sets the toggle to true or false if PlayerPrefs exist
        if (PlayerPrefs.HasKey("fullscreen"))
        {
            string stringFullscreen = PlayerPrefs.GetString("fullscreen");

            if (stringFullscreen.ToLower() == "true")
            {
                fullscreenToggle.isOn = true;
            }
            else if (stringFullscreen.ToLower() == "false")
            {
                fullscreenToggle.isOn = false;
            }
        }

        GetReferences();

        gameObject.SetActive(false);
    }

    void Start()
    {
        // Getting the animator controller in children
        animController = GetComponentInChildren<Animator>();

        // Resets Skill Name and Description
        nodeNameText.text = "";
        nodeDescText.text = "";

        // Get the stats of player and set it to the Status and Stats Page
        GetPlayerStats();
        SetPlayerStatsInBook();
        UpdateStatusBars();
    }

    public void GetReferences()
    {
        // Find for objects with tag and try to get a component from it
        globalBrightness = transform.parent.GetComponent<Volume>();

        if (GameObject.FindWithTag("Player").TryGetComponent(out PlayerController componentPlayer))
        {
            playerController = componentPlayer;
        }

        // Sets Slider PlayerPrefs
        brightnessSlider.value = PlayerPrefs.GetFloat("brightness");

        Debug.Log("Passed through GetReferences()");
    }

    void GetPlayerStats()
    {
        // To check which class the player is currently playing
        // Then set the portrait in Inventory Page
        // Then hide Projectile Speed stat if its not a ranged class
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

        statusValue[0] = playerStats.chosenBaseStats.maxHealth;
        statusValue[1] = playerController.GetMaxUltCharge();
        statusValue[2] = maxExp;

        for (int i = 1; i < statusSliders.Length; i++)
        {
            statusSliders[i].maxValue = statusValue[i];
        }
    }

    void SetPlayerStatsInBook() 
    {
        // Sets the class of the player to the Stats Page
        classText.text = "Class : " + playerStats.chosenClass.ToString();

        // Sets the stats value to the text boxes
        for (int i = 0; i < statsValueText.Length; i++)
        {
            switch (i)
            {
                case 0:
                    statsValueText[i].text = (Mathf.CeilToInt(playerStats.chosenBaseStats.maxHealth * playerStats.chosenStatMultipliers.health) + playerStats.chosenStatPoints.health).ToString();
                    break;
                case 1:
                    statsValueText[i].text = (playerStats.chosenBaseStats.defense + playerStats.chosenStatPoints.defense).ToString();
                    break;
                case 2:
                    statsValueText[i].text = (Mathf.CeilToInt(playerStats.chosenBaseStats.attack * playerStats.chosenStatMultipliers.attackPower) + playerStats.chosenStatPoints.attack).ToString();
                    break;
                case 3:
                    statsValueText[i].text = ((int)(2 / playerStats.chosenBaseStats.attackInterval) + playerStats.chosenStatPoints.attackSpeed).ToString();
                    break;
                case 4:
                    statsValueText[i].text = (playerStats.chosenBaseStats.moveSpeed + playerStats.chosenStatPoints.moveSpeed).ToString();
                    break;
                case 5:
                    statsValueText[i].text = (playerStats.chosenBaseStats.projectileSpeed + playerStats.chosenStatPoints.projectileSpeed).ToString();
                    break;
            }
        }

        // Notification message banner if there is stat point(s) availble
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

    public void IncreaseStat(int type)
    {
        // Adds a stat point to a stat and updates the stat value
        if (statPointAmt > 0)
        {
            statPointAmt--;
            float valueIncrease;
            switch (type)
            {
                case 0:
                    playerStats.chosenStatPoints.health++;
                    valueIncrease = playerStats.chosenStatMultipliers.health * (float)playerStats.chosenBaseStats.maxHealth;
                    playerStats.chosenStats.health += (int)valueIncrease;
                    playerStats.chosenStats.maxHealth += (int)valueIncrease;
                    break;
                case 1:
                    playerStats.chosenStatPoints.defense++;
                    valueIncrease = playerStats.chosenStatMultipliers.defense * (float)playerStats.chosenBaseStats.defense;
                    playerStats.chosenStats.defense += (int)valueIncrease;
                    break;
                case 2:
                    playerStats.chosenStatPoints.attack++;
                    valueIncrease = playerStats.chosenStatMultipliers.attackPower * (float)playerStats.chosenBaseStats.attack;
                    playerStats.chosenStats.attack += (int)valueIncrease;
                    break;
                case 3:
                    playerStats.chosenStatPoints.attackSpeed++;
                    playerStats.chosenStats.attackInterval -= playerStats.chosenStatMultipliers.attackSpeed * playerStats.chosenBaseStats.attackInterval;
                    break;
                case 4:
                    playerStats.chosenStatPoints.moveSpeed++;
                    valueIncrease = playerStats.chosenStatMultipliers.moveSpeed * playerStats.chosenBaseStats.moveSpeed;
                    playerStats.chosenStats.moveSpeed += valueIncrease;
                    break;
                case 5:
                    playerStats.chosenStatPoints.projectileSpeed++;
                    valueIncrease = playerStats.chosenStatMultipliers.projectileSpeed * playerStats.chosenBaseStats.projectileSpeed;
                    playerStats.chosenStats.projectileSpeed += valueIncrease;
                    break;
            }

            SetPlayerStatsInBook();
            UpdateStatusBars();
        }
    }

    public void ArtefactIncreaseStats(string classItem, int type, int amt, int multi)
    {
        int valueIncrease;
        
        if (classItem.ToLower() != playerStats.chosenClass.ToString().ToLower())
        {
            multi = 0;
        }

        switch (type)
        {
            case 0:
                playerStats.chosenStatPoints.health += (amt + multi);
                valueIncrease = (int)(playerStats.chosenStatMultipliers.health * (float)playerStats.chosenBaseStats.maxHealth) * (amt + multi);
                playerStats.chosenStats.health += valueIncrease;
                playerStats.chosenStats.maxHealth += valueIncrease;
                break;
            case 1:
                playerStats.chosenStatPoints.defense += (amt + multi);
                valueIncrease = (int)(playerStats.chosenStatMultipliers.defense * (float)playerStats.chosenBaseStats.defense) * (amt + multi);
                playerStats.chosenStats.defense += valueIncrease;
                break;
            case 2:
                playerStats.chosenStatPoints.attack += (amt + multi);
                valueIncrease = (int)(playerStats.chosenStatMultipliers.attackPower * (float)playerStats.chosenBaseStats.attack) * (amt + multi);
                playerStats.chosenStats.attack += valueIncrease;
                break;
            case 3:
                playerStats.chosenStatPoints.attackSpeed += (amt + multi);
                playerStats.chosenStats.attackInterval += playerStats.chosenStatMultipliers.attackSpeed * playerStats.chosenBaseStats.attackInterval * (amt + multi);
                break;
            case 4:
                playerStats.chosenStatPoints.moveSpeed += (amt + multi);
                valueIncrease = (int)(playerStats.chosenStatMultipliers.moveSpeed * (float)playerStats.chosenBaseStats.moveSpeed) * (amt + multi);
                playerStats.chosenStats.moveSpeed += valueIncrease;
                break;
            case 5:
                playerStats.chosenStatPoints.projectileSpeed += (amt + multi);
                valueIncrease = (int)(playerStats.chosenStatMultipliers.projectileSpeed * (float)playerStats.chosenBaseStats.projectileSpeed) * (amt + multi);
                playerStats.chosenStats.projectileSpeed += valueIncrease;
                break;
        }

        SetPlayerStatsInBook();
        UpdateStatusBars();
    }

    public void ArtefactDecreaseStats(string classItem, int type, int amt, int multi)
    {
        int valueIncrease;

        if (classItem.ToLower() != playerStats.chosenClass.ToString().ToLower())
        {
            multi = 0;
        }

        switch (type)
        {
            case 0:
                playerStats.chosenStatPoints.health -= (amt + multi);
                valueIncrease = (int)(playerStats.chosenStatMultipliers.health * (float)playerStats.chosenBaseStats.maxHealth) * (amt + multi);
                //playerStats.chosenStats.health -= valueIncrease;
                playerStats.chosenStats.maxHealth -= valueIncrease;
                break;
            case 1:
                playerStats.chosenStatPoints.defense -= (amt + multi);
                valueIncrease = (int)(playerStats.chosenStatMultipliers.defense * (float)playerStats.chosenBaseStats.defense) * (amt + multi);
                playerStats.chosenStats.defense -= valueIncrease;
                break;
            case 2:
                playerStats.chosenStatPoints.attack -= (amt + multi);
                valueIncrease = (int)(playerStats.chosenStatMultipliers.attackPower * (float)playerStats.chosenBaseStats.attack) * (amt + multi);
                playerStats.chosenStats.attack -= valueIncrease;
                break;
            case 3:
                playerStats.chosenStatPoints.attackSpeed -= (amt + multi);
                playerStats.chosenStats.attackInterval -= playerStats.chosenStatMultipliers.attackSpeed * playerStats.chosenBaseStats.attackInterval * (amt + multi);
                break;
            case 4:
                playerStats.chosenStatPoints.moveSpeed -= (amt + multi);
                valueIncrease = (int)(playerStats.chosenStatMultipliers.moveSpeed * (float)playerStats.chosenBaseStats.moveSpeed) * (amt + multi);
                playerStats.chosenStats.moveSpeed -= valueIncrease;
                break;
            case 5:
                playerStats.chosenStatPoints.projectileSpeed -= (amt + multi);
                valueIncrease = (int)(playerStats.chosenStatMultipliers.projectileSpeed * (float)playerStats.chosenBaseStats.projectileSpeed) * (amt + multi);
                playerStats.chosenStats.projectileSpeed -= valueIncrease;
                break;
        }

        SetPlayerStatsInBook();
        UpdateStatusBars();
    }

    void UpdateStatusBars()
    {
        levelText.text = "Lvl : " + playerStats.chosenStats.level;

        statusSliders[0].maxValue = playerStats.chosenStats.maxHealth;
        statusSliders[0].value = playerStats.chosenStats.health;
        statusValueText[0].text = statusSliders[0].value.ToString() + "/" + statusSliders[0].maxValue.ToString();

        statusSliders[1].value = playerController.GetUltCharge();
        statusValueText[1].text = statusSliders[1].value.ToString() + "/" + statusSliders[1].maxValue.ToString();

        while (playerStats.chosenStats.exp >= statusSliders[2].maxValue)
        {
            playerStats.chosenStats.exp -= (int)statusSliders[2].maxValue;
            playerStats.chosenStats.level++;
            statPointAmt += 2;
            skillPointAmt++;
        }

        statusSliders[2].value = playerStats.chosenStats.exp;
        statusValueText[2].text = statusSliders[2].value.ToString() + "/" + statusSliders[2].maxValue.ToString();

        levelText.text = "Lvl : " + playerStats.chosenStats.level.ToString();
        CheckLevel();
    }

    void CheckLevel()
    {
        if (playerStats.chosenStats.level == 15)
        {
            newArtefactSlots[0].tag = "ArtefactSlot";
            newArtefactSlots[0].GetComponent<Image>().color = new Color32(0, 0, 0, 0);
        }
        else if (playerStats.chosenStats.level == 30)
        {
            newArtefactSlots[1].tag = "ArtefactSlot";
            newArtefactSlots[1].GetComponent<Image>().color = new Color32(0, 0, 0, 0);
        }
        else if (playerStats.chosenStats.level == 45)
        {
            newArtefactSlots[2].tag = "ArtefactSlot";
            newArtefactSlots[2].GetComponent<Image>().color = new Color32(0, 0, 0, 0);
        }
        else if (playerStats.chosenStats.level == 60)
        {
            newArtefactSlots[3].tag = "ArtefactSlot";
            newArtefactSlots[3].GetComponent<Image>().color = new Color32(0, 0, 0, 0);
        }
    }

    void HideProjectileSpeedStat()
    {
        // Hides the projectile stat if not needed and changes the stat grid layout gaps
        statsGroup.spacing = new Vector2(0, 22.5f);
        projectileStat.SetActive(false);
    }

    void ShowProjectileSpeedStat()
    {
        // Show the projectile stat if needed and changes the stat grid layout gaps
        statsGroup.spacing = new Vector2(0, 10);
        projectileStat.SetActive(true);
    }

    public void BookButtons(int page_type)
    {
        // Defaults all pages to be false
        statusPage.SetActive(false);
        inventoryPage.SetActive(false);
        skillPage.SetActive(false);
        settingPage.SetActive(false);

        // Get and update all status bars and stats
        GetPlayerStats();
        SetPlayerStatsInBook();
        UpdateStatusBars();

        // To get the page number and flip left or right
        // Page Number : 1 to 4
        // Page Type : 0 is Left, 1 is Right
        int page = page_type / 10;
        int type = page_type % 10;

        // Sets the nextPageNo to the Page Number (Button pressing is to flip the page to next page etc)
        nextPageNo = page;

        // Check which page the player wants to go to
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

        // Start the flipping animation based on the type (left or right)
        StartCoroutine(AnimationStart(type));
    }

    private IEnumerator AnimationStart(int dir)
    {
        // Hides all the buttons (left and right)
        HideAllButtons(true);

        // Resets the position of the icons in the buttons
        for (int i = 0; i < rightButtons.Length; i++)
        {
            buttonCurrPos[i] = rightButtons[i].GetComponent<RectTransform>().anchoredPosition;
            buttonCurrPos[i].Set(buttonStartPos.x, buttonCurrPos[i].y);
            rightButtons[i].gameObject.GetComponent<RectTransform>().anchoredPosition = buttonCurrPos[i];

            iconCurrPos[i] = rightIcons[i].GetComponent<RectTransform>().anchoredPosition;
            iconCurrPos[i].Set(iconStartPos.x, iconCurrPos[i].y);
            rightIcons[i].GetComponent<RectTransform>().anchoredPosition = iconCurrPos[i];
        }

        // Calculates the number of time to flip depending on current page and next page
        int timeToFlip = Mathf.Abs(nextPageNo - currPageNo);

        // Loops the amount of time to flip
        for (int i = 0; i < timeToFlip; i++)
        {
            // Change the animation speed's multiplier based on the number of time to flip
            // This allows the animation to always be the same amount of time
            animController.SetFloat("SpeedMultiplier", timeToFlip);

            // Checks which direction to flip and triggers the animation
            switch (dir)
            {
                case 0:
                    animController.SetTrigger("flipLeft");
                    break;
                case 1:
                    animController.SetTrigger("flipRight");
                    break;
            }

            // Adds a delay to each flip based on 1 divided by the number of times to flip
            yield return new WaitForSecondsRealtime(1f / timeToFlip);
        }

        // Checks which page is flipped to
        // Then sets the page to active and shows all buttons based on left or right
        // Also sets the icon in button position to fit the binded sprite
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
        // Hides the buttons
        // Loops based on right button's length (4)
        for (int i = 0; i < rightButtons.Length; i++)
        {
            rightButtons[i].gameObject.SetActive(false);

            // Checks if left is also to be hidden
            if (includeLeft && i < leftButtons.Length)
            {
                leftButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void ShowAllButtons(bool includeLeft)
    {
        // Shows the buttons
        // Loops based on right button's length (4)
        for (int i = 0; i < rightButtons.Length; i++)
        {
            rightButtons[i].gameObject.SetActive(true);

            // Checks if left is also to be be shown
            if (includeLeft && i < leftButtons.Length)
            {
                leftButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void HUDButtons(int page)
    {
        // When player presses the HUD buttons to open the book

        // Sets all pages to inactive
        statusPage.SetActive(false);
        inventoryPage.SetActive(false);
        skillPage.SetActive(false);
        settingPage.SetActive(false);

        // Get and update all status bars and stats in Status and Stats Page
        GetPlayerStats();
        SetPlayerStatsInBook();
        UpdateStatusBars();

        // Checks which page they opened into
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

        // Just sets both currPageNo and nextPageNo so no animation would occur in AnimationStart coroutine
        currPageNo = page;
        nextPageNo = page;

        StartCoroutine(AnimationStart(0));
    }

    public void ResolutionApply(int resolutionIndex)
    {
        // Get the resolution based on the player's selection with index
        Resolution resolution = filteredResolutions[resolutionIndex];

        // Sets the applied resolution to 3 playerprefs to allow loading of options
        PlayerPrefs.SetInt("resolutionIndex", resolutionIndex);
        PlayerPrefs.SetInt("resolutionWidth", resolution.width);
        PlayerPrefs.SetInt("resolutionHeight", resolution.height);
        Debug.Log(PlayerPrefs.GetInt("resolutionIndex"));

        // Applies the resolution the player selected
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        Debug.Log("Resolution Applied : " + resolutionIndex);
    }

    public void FullscreenApply()
    {
        // Sets the fullscreen option based on the toggle boolean
        PlayerPrefs.SetString("fullscreen", fullscreenToggle.isOn.ToString());

        Screen.fullScreen = fullscreenToggle.isOn;
        Debug.Log("Fullscreen Applied : " + fullscreenToggle.isOn);
    }

    public void BrightnessApply()
    {
        // Try to get the ColorAdjustment component and sets the post exposure value based on the brightness slider's value
        if (globalBrightness.profile.TryGet<ColorAdjustments>(out ColorAdjustments colorAdjust))
        {
            PlayerPrefs.SetFloat("brightness", brightnessSlider.value);
            colorAdjust.postExposure.value = brightnessSlider.value;

            Debug.Log("Brightness Applied : " + PlayerPrefs.GetFloat("brightness"));
        }
    }

    public void ShowNodeNameDesc(string name, string desc)
    {
        nodeNameText.text = name;
        nodeDescText.text = desc;
    }

    public void ResetNodeNameDesc()
    {
        nodeNameText.text = "";
        nodeDescText.text = "";
    }
}
