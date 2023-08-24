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

    public void NotInCombatRange(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerHitbox") && !audSrc[0].isPlaying)
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

    public void InCombatRange(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerHitbox") &&
            audSrc[0].isPlaying && !audSrc[1].isPlaying && !audSrc[2].isPlaying)
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

    private IEnumerator PlayCombatBGM(int clip)
    {
        fadeOut = true;

        if (fadeOut)
        {
            while (audSrc[0].volume > 0)
            {
                audSrc[0].volume -= Time.unscaledDeltaTime * 5;
            }
            while (audSrc[1].volume > 0)
            {
                audSrc[1].volume -= Time.unscaledDeltaTime * 5;
            }
            while (audSrc[2].volume > 0)
            {
                audSrc[2].volume -= Time.unscaledDeltaTime * 5;
            }

            audSrc[0].Pause();
            audSrc[1].Stop();
            audSrc[2].Stop();

            fadeIn = true;
            fadeOut = false;
        }

        audSrc[clip].Play();

        if (fadeIn)
        {
            while (audSrc[clip].volume < 1)
            {
                audSrc[clip].volume += Time.unscaledDeltaTime * 5;
            }

            fadeIn = false;
        }

        yield return null;
    }
}
