using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [Header("Objects")]
    public GameObject[] invBarSlot;
    public Vector3[] invBarSlotStartPos;
    public Image[] invBarImg;

    [Header("Parent Objects")]
    public GameObject invBarParent;
    public GameObject invParent;

    [Header("Buttons")]
    public Button openInvBtn;
    public Button closeInvBtn;

    [Header("Text")]
    public TMP_Text nameText;
    public TMP_Text descText;

    [Header("Variables")]
    public float invBarLocalX;
    public float invBarLocalY;

    private int slot = 0;

    public GameObject barInInv;

    [HideInInspector] CanvasGroup invBar;
    [HideInInspector] CanvasGroup invMain;

    private bool invBarFadeOut = false, invBarFadeIn = false;
    private bool invMainFadeOut = false, invMainFadeIn = false;

    private void Start()
    {
        invBar = invBarParent.GetComponent<CanvasGroup>();
        invMain = invParent.GetComponent<CanvasGroup>();

        for (int i = 0; i < 3; i++)
        {
            invBarSlotStartPos[i] = invBarSlot[i].transform.localPosition;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            slot = 0;
            ResetColor();
        }
        else if (Input.GetKeyDown("2"))
        {
            slot = 1;
            ResetColor();
        }
        else if (Input.GetKeyDown("3"))
        {
            slot = 2;
            ResetColor();
        }
        else if (Input.GetKeyDown("4"))
        {
            slot = 3;
            ResetColor();
        }

        if (invBarFadeOut)
        {
            openInvBtn.interactable = false;

            if (invBar.alpha >= 0f)
            {
                invBar.alpha -= Time.deltaTime;

                if (invBar.alpha == 0f)
                {
                    invBarFadeOut = false;
                    invBarParent.SetActive(false);
                    Time.timeScale = 0.0f;
                }
            }
        }

        if (invBarFadeIn)
        {
            if (invBar.alpha < 1f)
            {
                invBar.alpha += Time.deltaTime;

                if (invBar.alpha >= 1f)
                {
                    invBarFadeIn = false;
                    openInvBtn.interactable = true;
                }
            }
        }

        if (invMainFadeOut)
        {
            closeInvBtn.interactable = false;

            if (invMain.alpha >= 0f)
            {
                invMain.alpha -= Time.deltaTime;

                if (invMain.alpha == 0f)
                {
                    invMainFadeOut = false;
                    invParent.SetActive(false);
                }
            }
        }

        if (invMainFadeIn)
        {
            if (invMain.alpha < 1f)
            {
                invMain.alpha += Time.deltaTime;

                if (invMain.alpha >= 1f)
                {
                    invMainFadeIn = false;
                    closeInvBtn.interactable = true;
                }
            }
        }
    }

    public void ResetColor()
    {
        for (int i = 0; i < 4; i++)
        {
            invBarImg[i].color = new Color(invBarImg[i].color.r, invBarImg[i].color.g, invBarImg[i].color.b, 0f);
        }

        invBarImg[slot].color += new Color(0f, 0f, 0f, 1f);
    }

    public void OpenInventory()
    {
        float startX = invBarLocalX;

        for (int i = 0; i < 4; i++)
        {
            invBarSlot[i].transform.SetParent(invParent.transform);

            invBarSlot[i].GetComponent<RectTransform>().sizeDelta = new Vector2(85, 85);
            invBarSlot[i].transform.localPosition = new Vector3(startX, invBarLocalY, 0);
            invBarSlot[i].GetComponent<GridLayoutGroup>().cellSize = new Vector2(64, 64);

            startX += 89;
        }

        invParent.SetActive(true);

        invBarFadeOut = true;
        invMainFadeIn = true;
    }

    public void CloseInventory()
    {
        Time.timeScale = 1.0f;

        for (int i = 0; i < 4; i++)
        {
            invBarSlot[i].transform.SetParent(invBarParent.transform);

            invBarSlot[i].GetComponent<RectTransform>().sizeDelta = new Vector2(64, 64);
            invBarSlot[i].transform.localPosition = invBarSlotStartPos[i];
            invBarSlot[i].GetComponent<GridLayoutGroup>().cellSize = new Vector2(44, 44);
        }

        invBarParent.SetActive(true);

        invMainFadeOut = true;
        invBarFadeIn = true;
    }

    public void InventoryBarFadeIn()
    {
        invBarParent.SetActive(true);
        invBarFadeIn = true;
    }

    public void InventoryBarDisable()
    {
        invBarParent.SetActive(false);
        invMain.alpha = 0.0f;

    }
}
