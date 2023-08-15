using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIButtonSprite : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Buttons")]
    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite[] buttonSprites;

    [SerializeField] private TMP_Text textObject;

    void Awake()
    {
        buttonImage = this.GetComponent<Image>();
        textObject = gameObject.GetComponentInChildren<TMP_Text>();
    }

    public void OnPointerDown(PointerEventData eventData) 
	{
		buttonImage.sprite = buttonSprites[1];
        textObject.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, -9);
	}

    public void OnPointerUp(PointerEventData eventData) 
	{
		buttonImage.sprite = buttonSprites[0];
        textObject.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0);
	}
}
