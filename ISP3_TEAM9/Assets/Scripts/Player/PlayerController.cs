using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private ScriptablePlayerStats playerStats;

    private Animator animator;

    [SerializeField]
    private RuntimeAnimatorController[] animControllers; //Store animator controllers

    private Rigidbody2D rb;
    private Vector2 moveDir;
    private Vector2 lookDir;
    private float lookAngle;

    [SerializeField]
    private GameObject arrowPrefab;
    private GameObject spawnedArrow = null;

    public GameObject MagicArrowPrefab;

    public GameObject FireBallPrefab;

    private float attackCooldownTimer = 0f;
    private float skillCooldownTimer = 0f;
    private float skillDurationTimer = 0f;
    private float ultCharge = 0f;

    private SpriteRenderer sr;

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
        sr = GetComponent<SpriteRenderer>();

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
        if (Input.GetMouseButton(0))
        {
            currentState = playerStates.Attack;
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

        if (Input.GetKeyDown("e"))
        {
            currentState = playerStates.Skill;
        }
        else if (Input.GetKeyDown("q"))
        {
            currentState = playerStates.Ultimate;
            
        }

        if ((spawnedArrow != null) && (!spawnedArrow.GetComponent<ArrowLauncher>().enabled))
        {
            spawnedArrow.transform.position = transform.position;
        }

        if ((skillDurationTimer <= 0) && (skillDurationTimer > -1))
        {
            ClearSkillEffects();
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //Get direction
        moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveDir = Vector2.ClampMagnitude(moveDir, 1); //To ensure vector is unit length
        //Debug.Log(moveDir);

        //Calculate velocity 
        rb.velocity = moveDir * playerStats.chosenStats.moveSpeed;

        attackCooldownTimer -= Time.deltaTime;
        skillCooldownTimer -= Time.deltaTime;
        skillDurationTimer -= Time.deltaTime;
    }

    private void LateUpdate()
    {
        lookDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        UpdateAnim();
    }

    private void UpdateAnim()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            switch (currentState)
            {
                case playerStates.Idle:
                    //Moving right
                    if (lookAngle < 45 && lookAngle > -45)
                    {
                        PlayAnim("AnimPlayerIdleRight");
                    }
                    //Moving left
                    else if (lookAngle > 135 || lookAngle < -135)
                    {
                        PlayAnim("AnimPlayerIdleLeft");
                    }
                    //Moving up
                    else if (lookAngle > 45 && lookAngle < 135)
                    {
                        PlayAnim("AnimPlayerIdleUp");
                    }
                    //Moving down
                    else if (lookAngle < -45 && lookAngle > -135)
                    {
                        PlayAnim("AnimPlayerIdleDown");
                    }
                    break;
                case playerStates.Walk:
                    //Moving right
                    if (lookAngle < 45 && lookAngle > -45)
                    {
                        PlayAnim("AnimPlayerWalkRight");
                    }
                    //Moving left
                    else if (lookAngle > 135 || lookAngle < -135)
                    {
                        PlayAnim("AnimPlayerWalkLeft");
                    }
                    //Moving up
                    else if (lookAngle > 45 && lookAngle < 135)
                    {
                        PlayAnim("AnimPlayerWalkUp");
                    }
                    //Moving down
                    else if (lookAngle < -45 && lookAngle > -135)
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
                case playerStates.Skill:
                    PlayerSkill();
                    break;
                case playerStates.Ultimate:
                    PlayerUltimate();
                    break;
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
        //Return if cooldown is not over yet
        if (attackCooldownTimer > 0)
        {
            return;
        }
        else
        {
            attackCooldownTimer = playerStats.chosenStats.attackInterval;
        }

        //Play different attack animation based on chosenClass
        switch (playerStats.chosenClass)
        {
            case ScriptablePlayerStats.playerClass.Archer:
                //Moving right
                if (lookAngle < 45 && lookAngle > -45)
                {
                    PlayAnim("AnimPlayerShootRight");
                }
                //Moving left
                else if (lookAngle > 135 || lookAngle < -135)
                {
                    PlayAnim("AnimPlayerShootLeft");
                }
                //Moving up
                else if (lookAngle > 45 && lookAngle < 135)
                {
                    PlayAnim("AnimPlayerShootUp");
                }
                //Moving down
                else if (lookAngle < -45 && lookAngle > -135)
                {
                    PlayAnim("AnimPlayerShootDown");
                }
                break;
            case ScriptablePlayerStats.playerClass.Mage:
                
                return;
            default:
                //Moving right
                if (lookAngle < 45 && lookAngle > -45)
                {
                    PlayAnim("AnimPlayerSlashRight");
                }
                //Moving left
                else if (lookAngle > 135 || lookAngle < -135)
                {
                    PlayAnim("AnimPlayerSlashLeft");
                }
                //Moving up
                else if (lookAngle > 45 && lookAngle < 135)
                {
                    PlayAnim("AnimPlayerSlashUp");
                }
                //Moving down
                else if (lookAngle < -45 && lookAngle > -135)
                {
                    PlayAnim("AnimPlayerSlashDown");
                }
                break;
        }

        
    }


    private void PlayerSkill()
    {
        switch(playerStats.chosenClass)
        {
            case ScriptablePlayerStats.playerClass.Archer:
                //Return if skill is still on cooldown
                if (skillCooldownTimer > 0)
                {
                    return;
                }
                else if (skillCooldownTimer <= 0)
                {
                    skillCooldownTimer = 20;
                }
                skillDurationTimer = 10;

                sr.color = Color.yellow;
                playerStats.chosenStats.attackInterval -= 0.3f;
                animator.speed += 0.3f;
                break;
            case ScriptablePlayerStats.playerClass.Mage:
                PlayAnim("AnimPlayerCastRight");
                break;
            case ScriptablePlayerStats.playerClass.Barbarian:
                //Return if skill is still on cooldown
                if (skillCooldownTimer > 0)
                {
                    return;
                }
                else if (skillCooldownTimer <= 0)
                {
                    skillCooldownTimer = 20;
                }
                skillDurationTimer = 10;

                sr.color = Color.red;
                playerStats.chosenStats.attack += 10;
                break;
            case ScriptablePlayerStats.playerClass.Paladin:
                PlayAnim("AnimPlayerCastRight");
                break;
        }
    }

    private void PlayerUltimate()
    {
        switch (playerStats.chosenClass)
        {
            case ScriptablePlayerStats.playerClass.Archer:
                for (int i = 0; i < 6; i++)
                {
                    if (i == 1)
                    {
                        Instantiate(MagicArrowPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                    }
                    else if (i == 2)
                    {
                        Instantiate(MagicArrowPrefab, new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z), Quaternion.identity);
                    }
                    else if (i == 3)
                    {
                        Instantiate(MagicArrowPrefab, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
                    }
                    else if (i == 4)
                    {
                        Instantiate(MagicArrowPrefab, new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z), Quaternion.identity);
                    }
                    else if (i == 5)
                    {
                        Instantiate(MagicArrowPrefab, new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z), Quaternion.identity);
                    }
                }
                break;
            case ScriptablePlayerStats.playerClass.Mage:
                PlayAnim("AnimPlayerCastRight");
                break;
            case ScriptablePlayerStats.playerClass.Barbarian:
                //Moving right
                if (lookAngle < 45 && lookAngle > -45)
                {
                    PlayAnim("AnimPlayerUltRight");
                }
                //Moving left
                else if (lookAngle > 135 || lookAngle < -135)
                {
                    PlayAnim("AnimPlayerUltLeft");
                }
                //Moving up
                else if (lookAngle > 45 && lookAngle < 135)
                {
                    PlayAnim("AnimPlayerUltUp");
                }
                //Moving down
                else if (lookAngle < -45 && lookAngle > -135)
                {
                    PlayAnim("AnimPlayerUltDown");
                }
                break;
            case ScriptablePlayerStats.playerClass.Paladin:
                PlayAnim("AnimPlayerCastRight");
                break;
        }
    }

    private void ClearSkillEffects()
    {
        switch (playerStats.chosenClass)
        {
            case ScriptablePlayerStats.playerClass.Archer:
                sr.color = Color.white;
                playerStats.chosenStats.attackInterval = 1f;
                animator.speed = 1f;
                break;
            case ScriptablePlayerStats.playerClass.Mage:
                break;
            case ScriptablePlayerStats.playerClass.Barbarian:
                sr.color = Color.white;
                playerStats.chosenStats.attack = 10;
                break;
            case ScriptablePlayerStats.playerClass.Paladin:
                break;
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

    //Called in animation events
    private void SpawnArrow()
    {
        spawnedArrow = Instantiate(arrowPrefab, transform.position, Quaternion.Euler(0, 0, lookAngle));
    }

    private void ShootArrow()
    {
        spawnedArrow.GetComponent<ArrowLauncher>().enabled = true;
    }

    private void SummonFireball()
    {
        Instantiate(FireBallPrefab, transform.position, Quaternion.Euler(0, 0, lookAngle));
    }

}
