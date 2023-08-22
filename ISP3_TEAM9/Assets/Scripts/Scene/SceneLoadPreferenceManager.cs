using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SceneLoadPreferenceManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private VolumeProfile volumeProfile;

    [Header("Variables")]
    [SerializeField] private UIBookController bookController;
    [SerializeField] private bool canUse = false;

    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private bool localFullscreenBool;

    void Start()
    {
        if (canUse)
        {
            // RESOLUTION
            if (PlayerPrefs.HasKey("resolutionIndex"))
            {
                int localResIndex = PlayerPrefs.GetInt("resolutionIndex");
                int localResWidth = PlayerPrefs.GetInt("resolutionWidth");
                int localResHeight = PlayerPrefs.GetInt("resolutionHeight");

                Screen.SetResolution(localResWidth, localResHeight, Screen.fullScreen);

                Debug.Log("Loaded PlayerPrefs (ResolutionIndex) : " + localResIndex);

                //resolutionDropdown.value = localResIndex;
                //resolutionDropdown.RefreshShownValue();
            }
            else
            {
                bookController.ResetButton("Graphics");
            }

            // FULLSCREEN
            if (PlayerPrefs.HasKey("fullscreen"))
            {
                string localFullscreen = PlayerPrefs.GetString("fullscreen");

                if (localFullscreen.ToLower() == "true")
                {
                    localFullscreenBool = true;
                }
                else if (localFullscreen.ToLower() == "false")
                {
                    localFullscreenBool = false;
                }

                Screen.fullScreen = localFullscreenBool;

                Debug.Log("Loaded PlayerPrefs (FullscreenBool) : " + localFullscreenBool);
            }
            else
            {
                Screen.fullScreen = true;
            }

            // BRIGHTNESS
            if (PlayerPrefs.HasKey("brightness"))
            {
                float localBrightness = PlayerPrefs.GetFloat("brightness");

                ColorAdjustments colorAdjust;

                if(!volumeProfile.TryGet(out colorAdjust)) throw new System.NullReferenceException(nameof(colorAdjust));

                colorAdjust.postExposure.Override(localBrightness);

                Debug.Log("Loaded PlayerPrefs (Brightness) : " + localBrightness);

                /*Volume globalBrightness;

                if (GameObject.FindWithTag("PostProcessor").TryGetComponent(out Volume componentVol))
                {
                    globalBrightness = componentVol;

                    
                }*/
            }

            // VOLUME
            /*if (PlayerPrefs.HasKey("masterVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("masterVolume");

                volumeText.text = $"{localVolume * 100:0}";
                volumeSlider.value = localVolume;
                AudioListener.volume = localVolume;

                Debug.Log("Loaded Player Prefs.");
            }
            else
            {
                bookController.ResetButton("Audio");
            }*/
        }
    }
}
