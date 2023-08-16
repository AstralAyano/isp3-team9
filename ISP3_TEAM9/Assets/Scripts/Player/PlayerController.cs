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

    private Rigidbody2D rb;
    private Vector2 moveDir;

    [SerializeField]
    private GameObject[] arrowPrefab;

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
        rb = GetComponent<Rigidbody2D>();

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
            SpawnArrow();
            //Debug.Log("Attacking");
        }
        else if (Mathf.Abs(rb.velocity.x) >= 0.01f || Mathf.Abs(rb.velocity.y) >= 0.01f)
        {
            currentState = playerStates.Walk;
            //Debug.Log("Walking");
        }
        else
        {
            currentState = playerStates.Idle;
            //Debug.Log("Idle");
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //Get direction
        moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveDir = Vector2.ClampMagnitude(moveDir, 1); //To ensure vector is unit length
        Debug.Log(moveDir);

        //Calculate velocity 
        rb.velocity = moveDir * playerStats.chosenStats.moveSpeed;
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
                    PlayAnim("AnimPlayerIdleDown");
                    break;
                case playerStates.Walk:
                    //Moving right
                    if (moveDir.x > 0)
                    {
                        PlayAnim("AnimPlayerWalkRight");
                    }
                    //Moving left
                    else if (moveDir.x < 0)
                    {
                        PlayAnim("AnimPlayerWalkLeft");
                    }
                    //Moving up
                    else if (moveDir.y > 0)
                    {
                        PlayAnim("AnimPlayerWalkUp");
                    }
                    //Moving down
                    else if (moveDir.y < 0)
                    {
                        PlayAnim("AnimPlayerWalkDown");
                    }
                    break;
                case playerStates.Attack:
                    PlayerAttack();
                    break;
                case playerStates.Hurt:
                    PlayerHurt();
                    break;
                //case playerStates.Skill:
                //    break;
                //case playerStates.Ultimate:
                //    break;
                case playerStates.Death:
                    PlayerDeath();
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
                //Moving left
                if (moveDir.x < 0)
                {
                    PlayAnim("AnimPlayerShootLeft");
                }
                //Moving up
                else if (moveDir.y > 0)
                {
                    PlayAnim("AnimPlayerShootUp");
                }
                //Moving down
                else if (moveDir.y < 0)
                {
                    PlayAnim("AnimPlayerShootDown");
                }
                //Moving right
                else
                {
                    PlayAnim("AnimPlayerShootRight");
                }
                break;
            case ScriptablePlayerStats.playerClass.Mage:
                //Moving left
                if (moveDir.x < 0)
                {
                    PlayAnim("AnimPlayerCastLeft");
                }
                //Moving up
                else if (moveDir.y > 0)
                {
                    PlayAnim("AnimPlayerCastUp");
                }
                //Moving down
                else if (moveDir.y < 0)
                {
                    PlayAnim("AnimPlayerCastDown");
                }
                //Moving right
                else
                {
                    PlayAnim("AnimPlayerCastRight");
                }
                break;
            default:
                //Moving left
                if (moveDir.x < 0)
                {
                    PlayAnim("AnimPlayerSlashLeft");
                }
                //Moving up
                else if (moveDir.y > 0)
                {
                    PlayAnim("AnimPlayerSlashUp");
                }
                //Moving down
                else if (moveDir.y < 0)
                {
                    PlayAnim("AnimPlayerSlashDown");
                }
                //Moving right
                else
                {
                    PlayAnim("AnimPlayerSlashRight");
                }
                break;
        }

        
    }

    //Spawn archer arrows
    private void SpawnArrow()
    {
        //Return if class is not archer or shooting animation is not done
        if ((playerStats.chosenClass != ScriptablePlayerStats.playerClass.Archer) ||
            ((animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0) && 
            (animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)))
        {
            return;
        }

        //Moving left
        if (moveDir.x < 0)
        {
            Rigidbody2D arrowLeftrb = Instantiate(arrowPrefab[1], transform).GetComponent<Rigidbody2D>();
            arrowLeftrb.AddForce(new Vector2(-10 * playerStats.chosenStats.projectileSpeed, 0));
        }
        //Moving up
        else if (moveDir.y > 0)
        {

            Rigidbody2D arrowUprb = Instantiate(arrowPrefab[0], transform).GetComponent<Rigidbody2D>();
            arrowUprb.AddForce(new Vector2(0, 10 * playerStats.chosenStats.projectileSpeed));
        }
        //Moving down
        else if (moveDir.y < 0)
        {
            Rigidbody2D arrowDownrb = Instantiate(arrowPrefab[2], transform).GetComponent<Rigidbody2D>();
            arrowDownrb.AddForce(new Vector2(0, -10 * playerStats.chosenStats.projectileSpeed));
        }
        //Moving right
        else
        {
            Rigidbody2D arrowRightrb = Instantiate(arrowPrefab[3], transform).GetComponent<Rigidbody2D>();
            arrowRightrb.AddForce(new Vector2(10 * playerStats.chosenStats.projectileSpeed, 0));
        }
    }

    private void PlayerHurt()
    {
        PlayAnim("AnimPlayerHurt");
        //playerStats.chosenStats.health -= ;
    }

    private void PlayerDeath()
    {
        PlayAnim("AnimPlayerHurt");
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            Destroy(gameObject, 0);
            //Switch scene

        }
    }
}
