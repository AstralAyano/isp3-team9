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
    private GameObject spawnedArrow = null;

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

    private float attackCooldownTimer = 0f;
    private float skillCooldownTimer = 0f;
    private float maxSkillCooldownTimer = 0f;
    private float skillDurationTimer = 0f;
    private float ultCharge = 0f;
    private const int maxUltCharge = 100;
    public bool ultChargeAdded;

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
            Transform audManager = GameObject.Find("SceneLoader").GetComponent<Transform>();
            audManager.SendMessage("InCombatRange", SendMessageOptions.DontRequireReceiver);
        }
        else if (!PlayerInBattle())
        {
            Transform audManager = GameObject.Find("SceneLoader").GetComponent<Transform>();
            audManager.SendMessage("NotInCombatRange", SendMessageOptions.DontRequireReceiver);
        }

        if ((Mathf.Abs(rb.velocity.x) >= 0.01f || Mathf.Abs(rb.velocity.y) >= 0.01f) && (!Input.anyKeyDown))
        {
            currentState = playerStates.Walk;
            //Debug.Log("Walking");
        }
        else if (!Input.anyKeyDown)
        {
            currentState = playerStates.Idle;
            //Debug.Log("Idle");
        }

        if (playerStats.chosenClass == ScriptablePlayerStats.playerClass.None)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            currentState = playerStates.Attack;
            //Debug.Log("Attacking");
        }

        else if (Input.GetKeyDown(KeyCode.E))
        {
            currentState = playerStates.Skill;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            currentState = playerStates.Ultimate;
        }

        if ((skillDurationTimer <= 0) && (skillDurationTimer > -1))
        {
            ClearSkillEffects();
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

        Debug.Log(ultCharge);
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

        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
        if (skillCooldownTimer > 0)
        {
            skillCooldownTimer -= Time.deltaTime;
        }
        if (skillDurationTimer > 0)
        {
            skillDurationTimer -= Time.deltaTime;
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
                //Moving right
                if (lookAngle < 45 && lookAngle > -45)
                {
                    PlayAnim("AnimPlayerCastRight");
                }
                //Moving left
                else if (lookAngle > 135 || lookAngle < -135)
                {
                    PlayAnim("AnimPlayerCastLeft");
                }
                //Moving up
                else if (lookAngle > 45 && lookAngle < 135)
                {
                    PlayAnim("AnimPlayerCastUp");
                }
                //Moving down
                else if (lookAngle < -45 && lookAngle > -135)
                {
                    PlayAnim("AnimPlayerCastDown");
                }
                mageAttacktype = "attack";
                break;
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
                PlayerAttack(playerStats.chosenStats.attack, .5f, 10f);
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

        switch (playerStats.chosenClass)
        {
            //Increase attack speed
            case ScriptablePlayerStats.playerClass.Archer:
                PlaySound(12);
                skillCooldownTimer = 20;
                SetMaxSkillCooldown(skillCooldownTimer);
                skillDurationTimer = 10;

                sr.color = Color.yellow;
                playerStats.chosenStats.attackInterval -= 0.3f;
                animator.speed += 0.3f;
                GainUltCharge(20f);
                break;
            //Shoot a lightning bolt
            case ScriptablePlayerStats.playerClass.Mage:
                PlayAnim("AnimPlayerCastDown");
                mageAttacktype = "skill";
                skillCooldownTimer = 20;
                SetMaxSkillCooldown(skillCooldownTimer);
                skillDurationTimer = 10;

                GainUltCharge(10f);
                break;
            case ScriptablePlayerStats.playerClass.Barbarian:
                PlaySound(10);
                skillCooldownTimer = 20;
                SetMaxSkillCooldown(skillCooldownTimer);
                skillDurationTimer = 10;

                sr.color = Color.red;
                playerStats.chosenStats.attack += 10;
                GainUltCharge(10f);
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
                GainUltCharge(10f);
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
                PlayerAttack(playerStats.chosenStats.attack * 2, 1.5f, 0f);
                ultCharge = 0;
                break;
            case ScriptablePlayerStats.playerClass.Paladin:
                PlayAnim("AnimPlayerCastDown");
                PalaActivateUlt = true;
                //PaladinUlt();
                ultCharge = 0;
                break;
        }
    }

    private void ClearSkillEffects()
    {
        sr.color = Color.white;
        switch (playerStats.chosenClass)
        {
            case ScriptablePlayerStats.playerClass.Archer:
                playerStats.chosenStats.attackInterval = 1f;
                animator.speed = 1f;
                break;
            case ScriptablePlayerStats.playerClass.Barbarian:
                playerStats.chosenStats.attack = 10;
                break;
            case ScriptablePlayerStats.playerClass.Paladin:
                playerStats.chosenStats.defense *= 75/100;
                break;
        }
    }

    public void PlayerTakeDamage(int dmg)
    {
        playerStats.chosenStats.health -= dmg * (int)Mathf.Clamp((1 - playerStats.chosenStats.defense/100), 0.1f, 0.9f);
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
        float multiplier = percentage / 100;
        if (playerStats.chosenStats.health != playerStats.chosenStats.maxHealth)
        {
            playerStats.chosenStats.health += (int)((float)playerStats.chosenStats.maxHealth * multiplier);

        }
        // if player health not equal to max health
        // health +=
        // max health * multiplier
    }
    public void GainAttackBoost(int percentage)
    {
        float multiplier = percentage / 100;
        playerStats.chosenStats.attack += (int)((float)playerStats.chosenStats.attack * multiplier);
    }

    public void GainDefenseBoost(int percentage)
    {
        float multiplier = percentage / 100;
        playerStats.chosenStats.defense += (int)((float)playerStats.chosenStats.defense * multiplier);
    }

    public void GainAtkSpdBoost(int percentage)
    {
        float multiplier = percentage / 100;
        playerStats.chosenStats.attackInterval -= (int)((float)playerStats.chosenStats.attackInterval * multiplier);
    }

    public void GainMovementBoost(int percentage)
    {
        float multiplier = percentage / 100;
        playerStats.chosenStats.moveSpeed += (int)((float)playerStats.chosenStats.moveSpeed * multiplier);
    }

    private void PlayerDeath()
    {
        Destroy(gameObject, 0);
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

    private void ClearPalaUlt()
    {

    }

    private void PlayerAttack(int dmg, float range, float amount)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + new Vector3(lookDir.x, lookDir.y, 0), range);
        foreach (var collider in colliders)
        {
            if (collider.gameObject.CompareTag("EnemyHitbox"))
            {
                collider.transform.parent.SendMessage("TakeDamage", dmg, SendMessageOptions.DontRequireReceiver);
                GainUltCharge(amount);
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
        if (playerStats.chosenStats.maxHealth == playerStats.chosenStats.health)
        {
            IsHealthMax = true;
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
            }
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
