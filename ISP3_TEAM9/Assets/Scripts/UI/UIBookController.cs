using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;

public class UIBookController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animController;
    [SerializeField] private Button[] rightButtons;
    [SerializeField] private Button[] leftButtons;
    [SerializeField] private GameObject[] rightIcons;

    [Header("Pages")]
    [SerializeField] private GameObject statusPage;
    [SerializeField] private GameObject inventoryPage;
    [SerializeField] private GameObject skillPage;
    [SerializeField] private GameObject settingPage;

    [Header("Sprites")]
    [SerializeField] private Sprite[] bookmark;

    [Header("Variables")]
    [SerializeField] private string currPage = "Status";
    [SerializeField] private Vector2 buttonStartPos;
    [SerializeField] private Vector2[] buttonCurrPos;
    [SerializeField] private Vector2 iconStartPos;
    [SerializeField] private Vector2[] iconCurrPos;

    private int currPageNo = 0;
    private int nextPageNo = 0;
    private float xOffset = 30;

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
    }

    public void BookButtons(int page_type)
    {
        statusPage.SetActive(false);
        inventoryPage.SetActive(false);
        skillPage.SetActive(false);
        settingPage.SetActive(false);

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
}
