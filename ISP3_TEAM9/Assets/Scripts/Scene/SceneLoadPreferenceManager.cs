using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneLoadPreferenceManager : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private UIBookController bookController;
    [SerializeField] private bool canUse = false;

    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private bool localFullscreenBool;

    void Start()
    {
        if (canUse)
            {
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
            }
    }
}
