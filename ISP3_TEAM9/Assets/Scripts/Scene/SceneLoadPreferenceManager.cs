using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Audio;

public class SceneLoadPreferenceManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private VolumeProfile volumeProfile;
    [SerializeField] private AudioMixer audMix;
    [SerializeField] private AudioMixerGroup[] audMixGrp;

    [Header("Variables")]
    [SerializeField] private UIBookController bookController;
    [SerializeField] private bool canUse = false;

    private bool localFullscreenBool;

    void Awake()
    {
        DontDestroyOnLoadManager.DestroyAll();
    }

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
            }

            // MASTER VOLUME
            if (PlayerPrefs.HasKey("masterVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("masterVolume");
                audMix.SetFloat("masterVol", localVolume);

                Debug.Log("Loaded PlayerPrefs (Master Vol) : " + localVolume);
            }
            else
            {
                audMix.SetFloat("masterVol", 0);
            }

            // BGM VOLUME
            if (PlayerPrefs.HasKey("bgmVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("bgmVolume");
                audMix.SetFloat("bgmVol", localVolume);

                Debug.Log("Loaded PlayerPrefs (BGM Vol) : " + localVolume);
            }
            else
            {
                audMix.SetFloat("bgmVol", 0);
            }

            // SFX VOLUME
            if (PlayerPrefs.HasKey("sfxVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("sfxVolume");
                audMix.SetFloat("sfxVol", localVolume);

                Debug.Log("Loaded PlayerPrefs (SFX Vol) : " + localVolume);
            }
            else
            {
                audMix.SetFloat("sfxVol", 0);
            }
        }
    }
}
