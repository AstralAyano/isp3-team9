using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SceneRoomLighting : MonoBehaviour
{
    private Light2D thisLight;
    private bool fadeIn, fadeOut;

    void Awake()
    {
        thisLight = GetComponent<Light2D>();

        fadeIn = false;
        fadeOut = false;
    }

    void Update()
    {
        if (fadeIn && thisLight.intensity < 0.75f)
        {
            thisLight.intensity += Time.unscaledDeltaTime * 3;
        }
        else if (fadeIn && thisLight.intensity >= 0.75f)
        {
            fadeIn = false;
        }

        if (fadeOut && thisLight.intensity > 0f)
        {
            thisLight.intensity -= Time.unscaledDeltaTime * 3;
        }
        else if (fadeOut && thisLight.intensity <= 0f)
        {
            fadeOut = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            fadeIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            fadeOut = true;
        }
    }
}
