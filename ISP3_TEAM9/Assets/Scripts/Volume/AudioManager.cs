using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audMix;

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat("masterVolume");
        bgmSlider.value = PlayerPrefs.GetFloat("bgmVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");

        audMix.SetFloat("masterVol", PlayerPrefs.GetFloat("masterVolume"));
        audMix.SetFloat("bgmVol", PlayerPrefs.GetFloat("bgmVolume"));
        audMix.SetFloat("sfxVol", PlayerPrefs.GetFloat("sfxVolume"));
    }

    public void MasterVolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", masterSlider.value);
        audMix.SetFloat("masterVol", PlayerPrefs.GetFloat("masterVolume"));
        PlayerPrefs.Save();
    }

    public void BGMVolumeApply()
    {
        PlayerPrefs.SetFloat("bgmVolume", bgmSlider.value);
        audMix.SetFloat("bgmVol", PlayerPrefs.GetFloat("bgmVolume"));
        PlayerPrefs.Save();
    }

    public void SFXVolumeApply()
    {
        PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);
        audMix.SetFloat("sfxVol", PlayerPrefs.GetFloat("sfxVolume"));
        PlayerPrefs.Save();
    }
}
