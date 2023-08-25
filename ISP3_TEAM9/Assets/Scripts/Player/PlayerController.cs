using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private ScriptablePlayerStats playerStats;

    private Animator animator;

    [SerializeField]
    private RuntimeAnimatorController[] animControllers; //Store animator controllers

    [Header("Audio")]
    public AudioSource[] audSrc;
    public List<AudioClip> clips;
    
    private Rigidbody2D rb;
    private Vector2 moveDir;
    private Vector2 lookDir;
    private float lookAngle;

    [SerializeField]
    private GameObject arrowPrefab;

    [SerializeField]
    private GameObject MagicArrowPrefab;
    [SerializeField]
    private GameObject FireBallPrefab;
    [SerializeField]
    private GameObject ArcaneShotPrefab;
    [SerializeField]
    private GameObject magicPrefab;
    [SerializeField]
    private GameObject PaladinAuraPrefab;
    [SerializeField]
    private GameObject barbarianUltPrefab;

    private float attackCooldownTimer = 0f;
    private float skillCooldownTimer = 0f;
    private float maxSkillCooldownTimer = 0f;
    private float skillDurationTimer = 0f;
    private float ultCharge = 0f;
    private const int maxUltCharge = 100;
    public bool ultChargeAdded;
    private bool isSkillsCleared = true;

    float interactRange = 5f;

    float MaxArrow = 0;

    private string mageAttacktype;

    private bool PalaActivateUlt = false;
    private float IntervalOfHeal;
    private int MaxHealCount = 0;

    public bool IsHealthMax;
    public bool IsSpeedPotionActive;
    public bool IsAtkPotionActive;
    public bool IsAtkSpdPotionActive;
    public bool IsDefensePotionActive;

    private float SpdPotionDuration;
    private float AtkPotionDuration;
    private float DefensePotionDuration;
    private float AtkSpdPotionDuration;

    private float atkIncreasedBy = 0;
    private float atkSpdIncreasedBy = 0;
    private float defIncreasedBy = 0;
    private float moveSpdIncreasedBy = 0;

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

        audSrc = GetComponentsInChildren<AudioSource>();

        SetAnimator();
    }

    public void SetAnimator()
    {
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

    private void Update()
    {
        if (playerStats.chosenStats.health <= 0)
        {
            return;
        }

        InteractWithNPC();

        if (PlayerInBattle())
        {
            try
            {
                Transform audManager = GameObject.Find("LoadReferences").GetComponent<Transform>();
                audManager.SendMessage("InCombatRange", SendMessageOptions.DontRequireReceiver);
            }
            catch (Exception)
            {
                //Debug
            }
        }
        else if (!PlayerInBattle())
        {
            try
            {
                Transform audManager = GameObject.Find("LoadReferences").GetComponent<Transform>();
                audManager.SendMessage("NotInCombatRange", SendMessageOptions.DontRequireReceiver);
            }
            catch (Exception)
            {
                //Debug
            }
        }

        if ((Mathf.Abs(rb.velocity.x) >= 0.01f || Mathf.Abs(rb.velocity.y) >= 0.01f) && (!Input.anyKeyDown))
        {
            currentState = playerStates.Walk;
        }
        else if (!Input.anyKeyDown)
        {
            currentState = playerStates.Idle;
        }

        if (playerStats.chosenClass == ScriptablePlayerStats.playerClass.None)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            currentState = playerStates.Attack;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            currentState = playerStates.Skill;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            currentState = playerStates.Ultimate;
        }

        if (PalaActivateUlt)
        {
            sr.color = Color.green;
            IntervalOfHeal += Time.deltaTime;
            if (MaxHealCount < 6)
            {
                if (IntervalOfHeal >= 0.5f)
                {
                    PlaySound(2);
                    int HealAmt = 5;
                    playerStats.chosenStats.health += HealAmt;
                    MaxHealCount++;
                    IntervalOfHeal = 0;
                    Debug.Log("Heal");
                }
            }
            else if (MaxHealCount > 5)
            {
                PalaActivateUlt = false;
                IntervalOfHeal = 0;
                sr.color = Color.white;
                Debug.Log("Reset");
                MaxHealCount = 0;
            }
        }

        if (attackCooldownTimer <= 0f || skillCooldownTimer <= 0f)
        {
            ultChargeAdded = false;
        }

        CheckIsHealthMax();
        CheckIsAtkPotionActive();
        CheckIsAtkSpdPotionActive();
        CheckIsDefensePotionActive();
        CheckIsSpeedPotionActive();

        //Debug.Log(IsHealthMax);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //Get direction
        moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveDir = Vector2.ClampMagnitude(moveDir, 1); //To ensure vector is unit length

        //Calculate velocity 
        rb.velocity = moveDir * playerStats.chosenStats.moveSpeed;

        Debug.Log("Attack Interval: " + playerStats.chosenStats.attackInterval);

        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
        else
        {
            attackCooldownTimer = 0;
            animator.speed = 1;
        }
        if (skillCooldownTimer > 0)
        {
            skillCooldownTimer -= Time.deltaTime;
        }
        else
        {
            skillCooldownTimer = 0;
        }
        if (skillDurationTimer >= 0)
        {
            skillDurationTimer -= Time.deltaTime;
        }
        else
        {
            if (!isSkillsCleared)
            {
                ClearSkillEffects();
            }
            skillDurationTimer = 0;
        }
    }

    private void LateUpdate()
    {
        lookDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        UpdateAnim();
    }

    private bool PlayerInBattle()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 8f);
        foreach (var collider in colliders)
        {
            if (collider.gameObject.CompareTag("EnemyHitbox"))
            {
                return true;
            }
        }
        return false;
    }
    
    private void InteractWithNPC()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            float interactRange = 1.5f;
            Collider2D[] colArr = Physics2D.OverlapCircleAll(transform.position, interactRange);

            foreach (Collider2D col in colArr)
            {
                if (col.TryGetComponent(out NPCController npcController))
                {
                    npcController.Interact();
                }
            }
        }
    }

    private void UpdateAnim()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            switch (currentState)
            {
                case playerStates.Idle:
                    //Right
                    if (lookAngle < 45 && lookAngle > -45)
                    {
                        PlayAnim("AnimPlayerIdleRight");
                    }
                    //Left
                    else if (lookAngle > 135 || lookAngle < -135)
                    {
                        PlayAnim("AnimPlayerIdleLeft");
                    }
                    //Up
                    else if (lookAngle > 45 && lookAngle < 135)
                    {
                        PlayAnim("AnimPlayerIdleUp");
                    }
                    //Down
                    else if (lookAngle < -45 && lookAngle > -135)
                    {
                        PlayAnim("AnimPlayerIdleDown");
                    }
                    break;
                case playerStates.Walk:
                    //Right
                    if (lookAngle < 45 && lookAngle > -45)
                    {
                        PlayAnim("AnimPlayerWalkRight");
                    }
                    //Left
                    else if (lookAngle > 135 || lookAngle < -135)
                    {
                        PlayAnim("AnimPlayerWalkLeft");
                    }
                    //Up
                    else if (lookAngle > 45 && lookAngle < 135)
                    {
                        PlayAnim("AnimPlayerWalkUp");
                    }
                    //Down
                    else if (lookAngle < -45 && lookAngle > -135)
                    {
                        PlayAnim("AnimPlayerWalkDown");
                    }
                    break;
                case playerStates.Attack:
                    PlayerAttack();
                    break;
                case playerStates.Hurt:
                    PlayAnim("AnimPlayerHurt");
                    break;
                case playerStates.Skill:
                    PlayerSkill();
                    break;
                case playerStates.Ultimate:
                    PlayerUltimate();
                    break;
                case playerStates.Death:
                    PlayAnim("AnimPlayerDeath");
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
            
            animator.speed = Mathf.Clamp(1 - (playerStats.chosenStats.attackInterval - playerStats.chosenBaseStats.attackInterval), 0.01f, Mathf.Infinity);
        }

        //Play different attack animation based on chosenClass
        switch (playerStats.chosenClass)
        {
            case ScriptablePlayerStats.playerClass.Archer:
                //Right
                if (lookAngle < 45 && lookAngle > -45)
                {
                    PlayAnim("AnimPlayerShootRight");
                }
                //Left
                else if (lookAngle > 135 || lookAngle < -135)
                {
                    PlayAnim("AnimPlayerShootLeft");
                }
                //Up
                else if (lookAngle > 45 && lookAngle < 135)
                {
                    PlayAnim("AnimPlayerShootUp");
                }
                //Down
                else if (lookAngle < -45 && lookAngle > -135)
                {
                    PlayAnim("AnimPlayerShootDown");
                }
                break;
            case ScriptablePlayerStats.playerClass.Mage:
                //Right
                if (lookAngle < 45 && lookAngle > -45)
                {
                    PlayAnim("AnimPlayerCastRight");
                }
                //Left
                else if (lookAngle > 135 || lookAngle < -135)
                {
                    PlayAnim("AnimPlayerCastLeft");
                }
                //Up
                else if (lookAngle > 45 && lookAngle < 135)
                {
                    PlayAnim("AnimPlayerCastUp");
                }
                //Down
                else if (lookAngle < -45 && lookAngle > -135)
                {
                    PlayAnim("AnimPlayerCastDown");
                }
                mageAttacktype = "attack";
                break;
            default:
                //Right
                if (lookAngle < 45 && lookAngle > -45)
                {
                    PlayAnim("AnimPlayerSlashRight");
                }
                //Left
                else if (lookAngle > 135 || lookAngle < -135)
                {
                    PlayAnim("AnimPlayerSlashLeft");
                }
                //Up
                else if (lookAngle > 45 && lookAngle < 135)
                {
                    PlayAnim("AnimPlayerSlashUp");
                }
                //Down
                else if (lookAngle < -45 && lookAngle > -135)
                {
                    PlayAnim("AnimPlayerSlashDown");
                }
                PlayerAttack(playerStats.chosenStats.attack, .5f);
                break;
        }
    }

    private void PlayerSkill()
    {
        //Return if skill is still on cooldown
        if (skillCooldownTimer > 0)
        {
            return;
        }
        isSkillsCleared = false;

        switch (playerStats.chosenClass)
        {
            //Increase attack speed
            case ScriptablePlayerStats.playerClass.Archer:
                PlaySound(12);
                skillCooldownTimer = 20;
                SetMaxSkillCooldown(skillCooldownTimer);
                skillDurationTimer = 10;

                sr.color = new Color(139, 128, 0, 1f);
                playerStats.chosenStats.attackInterval -= 0.3f;
                animator.speed += 0.3f;
                break;
            //Shoot a lightning bolt
            case ScriptablePlayerStats.playerClass.Mage:
                skillCooldownTimer = 7;
                SetMaxSkillCooldown(skillCooldownTimer);
                skillDurationTimer = 0;
                PlayAnim("AnimPlayerCastDown");
                mageAttacktype = "skill";

                GainUltCharge(5f);
                break;
            case ScriptablePlayerStats.playerClass.Barbarian:
                PlaySound(10);
                skillCooldownTimer = 20;
                SetMaxSkillCooldown(skillCooldownTimer);
                skillDurationTimer = 10;

                sr.color = Color.red;
                playerStats.chosenStats.attack += 10;
                break;
            case ScriptablePlayerStats.playerClass.Paladin:
                PlaySound(11);
                skillCooldownTimer = 20;
                SetMaxSkillCooldown(skillCooldownTimer);
                skillDurationTimer = 10;

                PlayAnim("AnimPlayerCastDown");

                Instantiate(PaladinAuraPrefab, transform);

                sr.color = Color.cyan;
                playerStats.chosenStats.defense *= 125/100;
                break;
        }
    }

    private void PlayerUltimate()
    {
        //Return if ultimate is not charged
        if (ultCharge < maxUltCharge)
        {
            return;
        }

        switch (playerStats.chosenClass)
        {
            case ScriptablePlayerStats.playerClass.Archer:
                Collider2D[] colArr = Physics2D.OverlapCircleAll(transform.position, interactRange);
                int count = 0;
                foreach (Collider2D col in colArr)
                {
                    if (col.gameObject.CompareTag("Enemy"))
                    {
                        count++;
                    }
                }
                while (MaxArrow <= 5 && count > 0)
                {
                    foreach (Collider2D col in colArr)
                    {
                        if (col.gameObject.CompareTag("Enemy"))
                        {
                            if (MaxArrow <= 5)
                            {
                                GameObject arrow = Instantiate(MagicArrowPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(0, 0, lookAngle));
                                arrow.GetComponent<HomingMissile>().Target = col.gameObject;
                                
                                MaxArrow++;
                            }
                        }
                    }
                    ultCharge = 0;
                }
                MaxArrow = 0;
                break;
            case ScriptablePlayerStats.playerClass.Mage:
                PlayAnim("AnimPlayerCastDown");
                mageAttacktype = "ultimate";
                ultCharge = 0;
                break;
            case ScriptablePlayerStats.playerClass.Barbarian:
                //Right
                if (lookAngle < 45 && lookAngle > -45)
                {
                    PlayAnim("AnimPlayerUltRight");
                }
                //Left
                else if (lookAngle > 135 || lookAngle < -135)
                {
                    PlayAnim("AnimPlayerUltLeft");
                }
                //Up
                else if (lookAngle > 45 && lookAngle < 135)
                {
                    PlayAnim("AnimPlayerUltUp");
                }
                //Down
                else if (lookAngle < -45 && lookAngle > -135)
                {
                    PlayAnim("AnimPlayerUltDown");
                }
                ultCharge = 0;
                break;
            case ScriptablePlayerStats.playerClass.Paladin:
                PlayAnim("AnimPlayerCastDown");
                PalaActivateUlt = true;
                ultCharge = 0;
                break;
        }
    }

    private void ClearSkillEffects()
    {
        if (skillDurationTimer >= 0)
        {
            return;
        }
        isSkillsCleared = true;
        sr.color = Color.white;
        switch (playerStats.chosenClass)
        {
            case ScriptablePlayerStats.playerClass.Archer:
                playerStats.chosenStats.attackInterval += 0.3f;
                break;
            case ScriptablePlayerStats.playerClass.Barbarian:
                playerStats.chosenStats.attack -= 10;
                break;
            case ScriptablePlayerStats.playerClass.Paladin:
                playerStats.chosenStats.defense *= 100/125;
                break;
        }
    }

    public void PlayerTakeDamage(int dmg)
    {
        playerStats.chosenStats.health -= (int)(dmg * Mathf.Clamp((1 - playerStats.chosenStats.defense / 100), 0.1f, 1f));
        Debug.Log("Damage taken: " + (int)(dmg * Mathf.Clamp((1 - playerStats.chosenStats.defense / 100), 0.1f, 1f)));
        if (playerStats.chosenStats.health > 0)
        {
            currentState = playerStates.Hurt;
        }
        else
        {
            currentState = playerStates.Death;
        }
    }

    public void GainXP(int amount)
    {
        playerStats.chosenStats.exp += amount;
    }
    
    public void GainHP(int percentage)
    {
        float multiplier = (float)percentage / 100;

        if (playerStats.chosenStats.health < playerStats.chosenStats.maxHealth)
        {
            playerStats.chosenStats.health += (int)((float)playerStats.chosenStats.maxHealth * multiplier);
        }
    }

    public void GainAttackBoost(int percentage)
    {
        if (IsAtkPotionActive)
        {
            IsAtkPotionActive = true;
            float multiplier = (float)percentage / 100;
            atkIncreasedBy = playerStats.chosenStats.attack * multiplier;
            playerStats.chosenStats.attack += (int)atkIncreasedBy;
        }
    }

    public void GainDefenseBoost(int percentage)
    {
        if (!IsDefensePotionActive)
        {
            IsDefensePotionActive = true;
            float multiplier = (float)percentage / 100;
            defIncreasedBy = playerStats.chosenStats.defense * multiplier;
            playerStats.chosenStats.defense += (int)defIncreasedBy;
        }
    }

    public void GainAtkSpdBoost(int percentage)
    {
        if (!IsAtkSpdPotionActive)
        {
            IsAtkSpdPotionActive = true;
            float multiplier = (float)percentage / 100;
            atkSpdIncreasedBy = playerStats.chosenStats.attackInterval * multiplier;
            playerStats.chosenStats.attackInterval -= (int)atkSpdIncreasedBy;
            animator.speed += (float)atkSpdIncreasedBy;
        }
    }

    public void GainMovementBoost(int percentage)
    {
        if (!IsSpeedPotionActive)
        {
            IsSpeedPotionActive = true;
            float multiplier = (float)percentage / 100;
            moveSpdIncreasedBy = playerStats.chosenStats.moveSpeed * multiplier;
            playerStats.chosenStats.moveSpeed += (int)moveSpdIncreasedBy;
        }
    }

    private void PlayerDeath()
    {
        Destroy(gameObject, 0);

        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("SceneMenu"))
        {
            UIController uiControl = GameObject.FindWithTag("UI").GetComponent<UIController>();
            uiControl.CheckNextScene(SceneManager.GetSceneByName("SceneEnd"));
        }

        //Switch to end scene
        SceneManager.LoadScene("SceneEnd");
    }

    private void ShootArrow()
    {
        Instantiate(arrowPrefab, transform.position, Quaternion.Euler(0, 0, lookAngle));
    }

    private void MageAttack()
    {
        switch (mageAttacktype)
        {
            case "attack":
                Instantiate(magicPrefab, transform.position, Quaternion.Euler(0, 0, lookAngle));
                PlaySound(7);
                break;
            case "skill":
                Instantiate(ArcaneShotPrefab, transform.position, Quaternion.Euler(0, 0, lookAngle));
                PlaySound(7);
                break;
            case "ultimate":
                Instantiate(FireBallPrefab, transform.position, Quaternion.Euler(0, 0, lookAngle));
                PlaySound(8);
                break;
        }
    }

    private void BarbarianUlt()
    {
        //Right
        if (lookAngle < 45 && lookAngle > -45)
        {
            Instantiate(barbarianUltPrefab, new Vector3(transform.position.x + 1, transform.position.y, transform.position.z), Quaternion.Euler(0, 0, -90));
        }
        //Left
        else if (lookAngle > 135 || lookAngle < -135)
        {
            Instantiate(barbarianUltPrefab, new Vector3(transform.position.x - 1, transform.position.y, transform.position.z), Quaternion.Euler(0, 0, 90));
        }
        //Up
        else if (lookAngle > 45 && lookAngle < 135)
        {
            Instantiate(barbarianUltPrefab, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
        }
        //Down
        else if (lookAngle < -45 && lookAngle > -135)
        {
            Instantiate(barbarianUltPrefab, new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), Quaternion.Euler(0, 0, 180));
        }
    }

    private void ClearPalaUlt()
    {

    }

    private void PlayerAttack(int dmg, float range)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + new Vector3(lookDir.x, lookDir.y, 0), range);
        foreach (var collider in colliders)
        {
            if (collider.gameObject.CompareTag("EnemyHitbox"))
            {
                collider.transform.parent.SendMessage("TakeDamage", dmg, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    public void GainUltCharge(float amount)
    {
        if (!ultChargeAdded && ultCharge < maxUltCharge)
        {
            ultChargeAdded = true;
            ultCharge += amount;
        }
    }

    public float GetUltCharge()
    {
        return ultCharge;
    }

    public float GetMaxUltCharge()
    {
        return maxUltCharge;
    }

    public void SetMaxSkillCooldown(float amount)
    {
        maxSkillCooldownTimer = amount;
    }

    public float GetMaxSkillCooldown()
    {
        return maxSkillCooldownTimer;
    }

    public float GetSkillCooldown()
    {
        return skillCooldownTimer;
    }

    void CheckIsHealthMax()
    {
        if (playerStats.chosenStats.health >= playerStats.chosenStats.maxHealth)
        {
            IsHealthMax = true;
        }
        else
        {
            IsHealthMax = false;
        }
    }

    void CheckIsAtkPotionActive()
    {
        if (IsAtkPotionActive)
        {
            AtkPotionDuration += Time.deltaTime;
            if (AtkPotionDuration >= 8)
            {
                IsAtkPotionActive = false;
                AtkPotionDuration = 0;

                playerStats.chosenStats.attack -= (int)atkIncreasedBy;
            }
        }
    }

    void CheckIsAtkSpdPotionActive()
    {
        if (IsAtkSpdPotionActive)
        {
            AtkSpdPotionDuration += Time.deltaTime;
            if (AtkSpdPotionDuration >= 15)
            {
                IsAtkSpdPotionActive = false;
                AtkSpdPotionDuration = 0;

                playerStats.chosenStats.attackSpeed += (int)atkSpdIncreasedBy;
                animator.speed -= (float)atkSpdIncreasedBy;
            }
        }
    }

    void CheckIsDefensePotionActive()
    {
        if (IsDefensePotionActive)
        {
            DefensePotionDuration += Time.deltaTime;
            if (DefensePotionDuration >= 10)
            {
                IsDefensePotionActive = false;
                DefensePotionDuration = 0;

                playerStats.chosenStats.defense -= (int)defIncreasedBy;
            }
        }
    }

    void CheckIsSpeedPotionActive()
    {
        if (IsSpeedPotionActive)
        {
            SpdPotionDuration += Time.deltaTime;
            if (SpdPotionDuration >= 15)
            {
                IsSpeedPotionActive = false;
                SpdPotionDuration = 0;

                playerStats.chosenStats.moveSpeed -= (int)moveSpdIncreasedBy;
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
