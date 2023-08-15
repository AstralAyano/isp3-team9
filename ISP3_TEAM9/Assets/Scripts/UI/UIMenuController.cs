using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMenuController : MonoBehaviour
{
    [SerializeField] private CanvasGroup uiCanvasGroup;

    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text companyText;
    [SerializeField] private TMP_Text versionText;

    //[SerializeField] private 

    void Awake()
    {
        uiCanvasGroup.alpha = 0;

        titleText.alpha = 0;
        companyText.alpha = 0;
        versionText.alpha = 0;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private IEnumerator MenuFadeIn()
    {
        WaitForSeconds delay = new WaitForSeconds(0.5f);

        yield return delay;

        //if ()
    }
}
