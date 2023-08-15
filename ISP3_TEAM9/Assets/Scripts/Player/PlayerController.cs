using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private ScriptablePlayerStats playerStats;

    private Animator animator;

    [SerializeField]
    private RuntimeAnimatorController[] animControllers; //Store animator controllers

    private Vector2 currentPos;
    private Vector2 moveDir;
    private Vector2 movement;
    private Vector2 newPos;

    public enum playerStates
    {
        Idle,
        Walk,
        Attack,
        Hurt,
        Skill,
        Ultimate,
        Death
    }

    public playerStates currentState;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

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
        }
    }

    private void Update()
    {
        if (newPos != currentPos)
        {
            currentState = playerStates.Walk;
            Debug.Log("Walking");
        }
        else
        {
            currentState = playerStates.Idle;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        currentPos = transform.position;

        moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveDir = Vector2.ClampMagnitude(moveDir, 1); //To ensure vector is unit length

        //Calculate new position 
        movement = moveDir * playerStats.chosenStats.moveSpeed;
        newPos = currentPos + movement * Time.fixedDeltaTime;
        transform.position = newPos;
    }

    private void LateUpdate()
    {
        updateAnim();
    }

    private void updateAnim()
    {
        switch (currentState)
        {
            case playerStates.Idle:
                //Find the correct animation
                for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++)
                {
                    //Any class's animation can be played this way
                    if (animator.runtimeAnimatorController.animationClips[i].ToString().Contains("AnimPlayerIdle"))
                    {
                        //ToString() does not give only the name, so need to get rid of the part at the end
                        animator.Play(animator.runtimeAnimatorController.animationClips[i].ToString().Replace(" (UnityEngine.AnimationClip)", ""));
                    }
                }
                break;
            case playerStates.Walk:
                //Moving right
                if (Input.GetKeyDown("d"))
                {
                    //Find the correct animation
                    for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++)
                    {
                        //Any class's animation can be played this way
                        if (animator.runtimeAnimatorController.animationClips[i].ToString().Contains("AnimPlayerWalkRight"))
                        {
                            //ToString() does not give only the name, so need to get rid of the part at the end
                            animator.Play(animator.runtimeAnimatorController.animationClips[i].ToString().Replace(" (UnityEngine.AnimationClip)", ""));
                        }
                    }
                }
                //Moving left
                if (Input.GetKeyDown("a"))
                {
                    //Find the correct animation
                    for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++)
                    {
                        if (animator.runtimeAnimatorController.animationClips[i].ToString().Contains("AnimPlayerWalkLeft"))
                        {
                            //This way, any class's animation can be played
                            //ToString() does not give only the name, so need to get rid of the part at the end
                            animator.Play(animator.runtimeAnimatorController.animationClips[i].ToString().Replace(" (UnityEngine.AnimationClip)", ""));
                        }
                    }
                }
                //Moving up
                if (Input.GetKeyDown("w"))
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
                //Moving down
                if (Input.GetKeyDown("s"))
                {
                    //Find the correct animation
                    for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++)
                    {
                        if (animator.runtimeAnimatorController.animationClips[i].ToString().Contains("AnimPlayerWalkDown"))
                        {
                            //This way, any class's animation can be played
                            //ToString() does not give only the name, so need to get rid of the part at the end
                            animator.Play(animator.runtimeAnimatorController.animationClips[i].ToString().Replace(" (UnityEngine.AnimationClip)", ""));
                        }
                    }
                }
                break;
        }

    }
}
