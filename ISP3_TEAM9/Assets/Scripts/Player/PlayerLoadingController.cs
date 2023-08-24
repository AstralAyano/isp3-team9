using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoadingController : MonoBehaviour
{
    [SerializeField]
    private ScriptablePlayerStats playerStats;

    private Animator animator;

    [SerializeField]
    private RuntimeAnimatorController[] animControllers; //Store animator controllers

    [Header("Audio")]
    public AudioSource[] audSrc;
    public List<AudioClip> clips;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audSrc = GetComponentsInChildren<AudioSource>();

        //Set the animator controller to use
        switch (playerStats.chosenClass)
        {
            case ScriptablePlayerStats.playerClass.Barbarian:
                animator.runtimeAnimatorController = animControllers[0];
                break;
            case ScriptablePlayerStats.playerClass.Mage:
                animator.runtimeAnimatorController = animControllers[1];
                break;
            case ScriptablePlayerStats.playerClass.Archer:
                animator.runtimeAnimatorController = animControllers[2];
                break;
            case ScriptablePlayerStats.playerClass.Paladin:
                animator.runtimeAnimatorController = animControllers[3];
                break;
            default:
                animator.runtimeAnimatorController = animControllers[4];
                break;
        }
    }

    private void LateUpdate()
    {
        //Find the correct animation
        for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++)
        {
            if (animator.runtimeAnimatorController.animationClips[i].ToString().Contains("AnimPlayerWalkUp"))
            {
                //This way, any class's animation can be played
                //ToString() does not give only the name, so need to get rid of the part at the end
                animator.Play(animator.runtimeAnimatorController.animationClips[i].ToString().Replace(" (UnityEngine.AnimationClip)", ""));
            }
        }
    }

    public void PlaySound(int clip)
    {
        for (int i = 0; i < audSrc.Length; i++)
        {
            if (!audSrc[i].isPlaying)
            {
                audSrc[i].clip = clips[clip];
                audSrc[i].Play();
                break;
            }
        }
    }
}
