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
        if (Input.GetMouseButtonDown(0))
        {
            currentState = playerStates.Attack;
            Debug.Log("Attacking");
        }
        else if (newPos != currentPos)
        {
            currentState = playerStates.Walk;
            Debug.Log("Walking");
        }
        else
        {
            currentState = playerStates.Idle;
            Debug.Log("Idle");
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
        UpdateAnim();
    }

    private void UpdateAnim()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            switch (currentState)
            {
                case playerStates.Idle:
                    PlayAnim("AnimPlayerIdle");
                    break;
                case playerStates.Walk:
                    //Moving right
                    if (Input.GetKey("d"))
                    {
                        PlayAnim("AnimPlayerWalkRight");
                    }
                    //Moving left
                    else if (Input.GetKey("a"))
                    {
                        PlayAnim("AnimPlayerWalkLeft");
                    }
                    //Moving up
                    else if (Input.GetKey("w"))
                    {
                        PlayAnim("AnimPlayerWalkUp");
                    }
                    //Moving down
                    else if (Input.GetKey("s"))
                    {
                        PlayAnim("AnimPlayerWalkDown");
                    }
                    break;
                case playerStates.Attack:
                    PlayerAttack();
                    break;
            }
        }
    }

    //Find a specific animation type by giving part of its name and play it e.g. AnimPlayerWalk for all class' walking animations
    private void PlayAnim(string animName)
    {
        //Find the correct animation
        for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++)
        {
            if (animator.runtimeAnimatorController.animationClips[i].ToString().Contains(animName))
            {
                //This way, any class's animation can be played
                //ToString() does not give only the name, so need to get rid of the part at the end
                animator.Play(animator.runtimeAnimatorController.animationClips[i].ToString().Replace(" (UnityEngine.AnimationClip)", ""));
            }
        }
    }

    private void PlayerAttack()
    {
        //Play different attack animation based on chosenClass
        switch (playerStats.chosenClass)
        {
            case ScriptablePlayerStats.playerClass.Archer:
                //Moving right
                //if (newPos.x - currentPos.x > 0)
                //{
                    PlayAnim("AnimPlayerShootRight");
                //}
                //Moving left
                if (newPos.x - currentPos.x < 0)
                {
                    PlayAnim("AnimPlayerShootLeft");
                }
                //Moving up
                else if (newPos.y - currentPos.y > 0)
                {
                    PlayAnim("AnimPlayerShootUp");
                }
                //Moving down
                else if (newPos.y - currentPos.y < 0)
                {
                    PlayAnim("AnimPlayerShootDown");
                }
                break;
            default:
                break;
        }

        
    }

    private void SpawnArrow()
    {

    }

    private void PlayerHurt()
    {
        PlayAnim("AnimPlayerHurt");
    }
}
