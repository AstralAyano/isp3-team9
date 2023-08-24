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

    public void MasterVolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", masterSlider.value);
        audMix.SetFloat("masterVol", PlayerPrefs.GetFloat("masterVolume"));
    }

    public void BGMVolumeApply()
    {
        PlayerPrefs.SetFloat("bgmVolume", bgmSlider.value);
        audMix.SetFloat("bgmVol", PlayerPrefs.GetFloat("bgmVolume"));
    }

    public void SFXVolumeApply()
    {
        PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);
        audMix.SetFloat("sfxVol", PlayerPrefs.GetFloat("sfxVolume"));
    }
}
