using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLevelAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource[] audSrc;
    [SerializeField] private AudioClip currBGM;

    private bool fadeIn = false, fadeOut = false;
    private AudioSource audToFadeOut, audToFadeIn;

    void Start()
    {
        
    }

    void Update()
    {
        if (fadeOut)
        {
            if (audToFadeOut.volume > 0)
            {
                audToFadeOut.volume -= Time.unscaledDeltaTime * 2;
            }
            else
            {
                fadeOut = false;
                fadeIn = true;
                
                audToFadeOut.Pause();
                audToFadeIn.Play();
            }
        }

        if (fadeIn)
        {
            if (audToFadeIn.volume < 1)
            {
                audToFadeIn.volume += Time.unscaledDeltaTime * 2;
            }
            else
            {
                fadeIn = false;
            }
        }
    }

    public void NotInCombatRange()
    {
        if (!audSrc[0].isPlaying)
        {
            int index = 0;

            if (audSrc[1].isPlaying)
            {
                index = 1;
            }
            else if (audSrc[2].isPlaying)
            {
                index = 2;
            }
            
            audToFadeOut = audSrc[index];
            audToFadeIn = audSrc[0];
            
            audSrc[0].volume = 0;
            audSrc[0].Play();

            currBGM = audSrc[0].clip;

            fadeOut = true;
        }
    }

    public void InCombatRange()
    {
        if (audSrc[0].isPlaying && !audSrc[1].isPlaying && !audSrc[2].isPlaying)
        {
            int randBGM = Random.Range(0, 2);

            audToFadeOut = audSrc[0];
            audToFadeIn = audSrc[randBGM + 1];

            audSrc[randBGM + 1].volume = 0;
            audSrc[randBGM + 1].Play();

            currBGM = audSrc[randBGM + 1].clip;

            fadeOut = true;
        }
    }
}
