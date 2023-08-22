using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public enum npcType
    {
        Tutorial = 0,
        Skill = 1,
        Barbarian = 2,
        Paladin = 3,
        Archer = 4,
        Mage = 5
    }

    [Header("NPCs")]
    [SerializeField] private GameObject[] npcObj;

    [Header("References")]
    [SerializeField] private UIHUDController uiHUD;
    [SerializeField] private GameObject bookUI;
    [SerializeField] private GameObject[] hudUI;
    [SerializeField] private CanvasGroup dialogueUI;
    [SerializeField] private Image dialoguePortrait;
    [SerializeField] private TMP_Text dialogueName;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Image autoButton;

    [Header("Player")]
    [SerializeField] private ScriptablePlayerStats playerStats;
    [SerializeField] private GameObject classPrompt;
    [SerializeField] private npcType currNPC;

    [Header("Tile Maps")]
    [SerializeField] private GameObject[] tents;
    [SerializeField] private GameObject[] tentDetails;
    [SerializeField] private GameObject[] building;
    [SerializeField] private GameObject[] buildingDetails;

    [Header("Dialogue Variables")]
    [SerializeField] private Sprite[] npcPortrait;

    [Header("Variables")]
    [SerializeField] private float typeSpeed;

    [Header("First Time Lines")]
    private List<string[]> firstLinesArrays;
    [SerializeField] private string[] tutorialFirst;
    [SerializeField] private string[] skillFirst;
    [SerializeField] private string[] barbarianFirst;
    [SerializeField] private string[] paladinFirst;
    [SerializeField] private string[] archerFirst;
    [SerializeField] private string[] mageFirst;

    [Header("Repeated Lines")]
    private List<string[]> repeatedLinesArrays;
    [SerializeField] private string[] tutorialLines;
    [SerializeField] private string[] skillLines;
    [SerializeField] private string[] barbarianLines;
    [SerializeField] private string[] paladinLines;
    [SerializeField] private string[] archerLines;
    [SerializeField] private string[] mageLines;

    private string[] thisDialogueLines;
    public int currLineNo = 0;
    public int maxLineNo = 0;
    private bool dialogueStarted = false;
    private bool playLine = false;
    private bool typingLine = false;
    private bool stopTyping = false;
    private bool autoPlay = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != null)
        {
            Destroy(this);
        }

        firstLinesArrays = new List<string[]>();
        firstLinesArrays.Add(tutorialFirst);
        firstLinesArrays.Add(skillFirst);
        firstLinesArrays.Add(barbarianFirst);
        firstLinesArrays.Add(paladinFirst);
        firstLinesArrays.Add(archerFirst);
        firstLinesArrays.Add(mageFirst);

        repeatedLinesArrays = new List<string[]>();
        repeatedLinesArrays.Add(tutorialLines);
        repeatedLinesArrays.Add(skillLines);
        repeatedLinesArrays.Add(barbarianLines);
        repeatedLinesArrays.Add(paladinLines);
        repeatedLinesArrays.Add(archerLines);
        repeatedLinesArrays.Add(mageLines);

        dialogueUI.gameObject.SetActive(false);
        classPrompt.SetActive(false);

        hudUI = GameObject.FindGameObjectsWithTag("HUD");
    }

    public void TriggerDialogue(npcType triggeredNPC)
    {
        currLineNo = 0;

        dialoguePortrait.sprite = npcPortrait[(int)triggeredNPC];
        dialogueName.text = triggeredNPC.ToString();

        switch (triggeredNPC)
        {
            case npcType.Tutorial:
                dialogueName.text = "Johna Tan";
                break;
            case npcType.Skill:
                dialogueName.text = "Goku Jo";
                break;
        }

        for (int i = 0; i < hudUI.Length; i++)
        {
            hudUI[i].SetActive(false);
        }

        bookUI.SetActive(false);
        dialogueUI.gameObject.SetActive(true);

        playLine = true;

        currNPC = triggeredNPC;

        if (PlayerPrefs.GetInt(triggeredNPC.ToString() + "Dialogue", 1) == 1)
        {
            PlayerPrefs.SetInt(triggeredNPC.ToString() + "Dialogue", 0);

            FirstTimeDialogue(triggeredNPC);
        }
        else
        {
            RepeatedDialogue(triggeredNPC);
        }
    }

    void FirstTimeDialogue(npcType triggeredNPC)
    {
        maxLineNo = firstLinesArrays[(int)triggeredNPC].Length;
        thisDialogueLines = firstLinesArrays[(int)triggeredNPC];

        dialogueStarted = true;
    }

    void RepeatedDialogue(npcType triggeredNPC)
    {
        maxLineNo = repeatedLinesArrays[(int)triggeredNPC].Length;
        thisDialogueLines = repeatedLinesArrays[(int)triggeredNPC];

        dialogueStarted = true;
    }

    void ResetFirstTime()
    {
        PlayerPrefs.DeleteAll();

        foreach (npcType npc in Enum.GetValues(typeof(npcType)))
        {
            PlayerPrefs.DeleteKey(npc.ToString() + "Dialogue");
        }
    }

    private IEnumerator DisplayLine(string currLine)
    {
        WaitForSeconds letterDelay = new WaitForSeconds(typeSpeed);
        dialogueText.text = "";
        typingLine = true;

        foreach (char letter in currLine.ToCharArray())
        {
            if (!stopTyping)
            {
                dialogueText.text += letter;
            }
            else
            {
                stopTyping = false;
                typingLine = false;

                dialogueText.text = currLine;

                StopCoroutine("DisplayLine");
            }
            
            yield return letterDelay;
        }

        typingLine = false;

        if (autoPlay)
        {
            yield return new WaitForSeconds(2);

            NextLineOrSkipTyping();
        }
    }

    public void NextLineOrSkipTyping()
    {
        if (typingLine)
        {
            stopTyping = true;
        }
        else
        {
            playLine = true;
        }
    }

    public void ToggleAutoplay()
    {
        if (!autoPlay)
        {
            autoPlay = true;
            autoButton.color = new Color32(128, 128, 128, 255);
        }
        else
        {
            autoPlay = false;
            autoButton.color = new Color32(255, 255, 255, 255);
        }
        
        if (!typingLine)
        {
            playLine = true;
        }
    }

    void StartNextLine()
    {
        if (dialogueStarted)
        {
            if (currLineNo <= maxLineNo)
            {
                if (playLine && !typingLine)
                {   
                    currLineNo++;
                    
                    try
                    {
                        StartCoroutine("DisplayLine", thisDialogueLines[currLineNo - 1]);
                    }
                    catch
                    {
                        // Ignore Error Please
                    }

                    playLine = false;
                }
            }
            else if (currLineNo > maxLineNo)
            {
                autoButton.color = new Color32(255, 255, 255, 255);
                currLineNo = 0;
                maxLineNo = 0;
                playLine = false;
                typingLine = false;
                stopTyping = false;
                autoPlay = false;

                dialogueStarted = false;
                dialogueUI.gameObject.SetActive(false);
                
                for (int i = 0; i < hudUI.Length; i++)
                {
                    hudUI[i].SetActive(true);
                }

                classPrompt.SetActive(true);

                Time.timeScale = 0;
            }
        }
    }

    public void PromptYes()
    {
        switch (currNPC)
        {
            case npcType.Barbarian:
                playerStats.chosenClass = ScriptablePlayerStats.playerClass.Barbarian;
                npcObj[0].SetActive(false);
                npcObj[1].SetActive(true);
                npcObj[2].SetActive(true);
                npcObj[3].SetActive(true);
                break;
            case npcType.Paladin:
                playerStats.chosenClass = ScriptablePlayerStats.playerClass.Paladin;
                npcObj[0].SetActive(true);
                npcObj[1].SetActive(false);
                npcObj[2].SetActive(true);
                npcObj[3].SetActive(true);
                break;
            case npcType.Archer:
                playerStats.chosenClass = ScriptablePlayerStats.playerClass.Archer;
                npcObj[0].SetActive(true);
                npcObj[1].SetActive(true);
                npcObj[2].SetActive(false);
                npcObj[3].SetActive(true);
                break;
            case npcType.Mage:
                playerStats.chosenClass = ScriptablePlayerStats.playerClass.Mage;
                npcObj[0].SetActive(true);
                npcObj[1].SetActive(true);
                npcObj[2].SetActive(true);
                npcObj[3].SetActive(false);
                break;
        }

        Time.timeScale = 1;

        classPrompt.SetActive(false);

        uiHUD.GetSkillUltNames();
    }

    public void PromptNo()
    {
        Time.timeScale = 1;

        classPrompt.SetActive(false);
    }

    void Update()
    {
        StartNextLine();

        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            ResetFirstTime();

            Debug.Log("Reset Dialogue to First Time interaction.");
        }
    }
}
