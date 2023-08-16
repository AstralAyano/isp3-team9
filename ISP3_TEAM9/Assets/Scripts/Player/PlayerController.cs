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
    private Vector2 prevDir; //To record player direction before idle

    [SerializeField]
    private GameObject[] arrowPrefab; 
    private List<Rigidbody2D> arrowPrefabrbs = new List<Rigidbody2D>(); //0 - Up, 1 - Left, 2 - Down, 3 - Right

    public GameObject MagicArrowPrefab;

    private int attackCooldownTimer = 0;
    private int skillCooldownTimer = 0;

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
            //SpawnArrow();
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
        }
        else if (Input.GetKeyDown("q"))
        {
            currentState = playerStates.Ultimate;
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

        if (ShootArrow())
        {
            for (int i = 0; i < arrowPrefabrbs.Count; i++)
            {
                if (arrowPrefabrbs[i].gameObject.name.Contains("Up"))
                {
                    arrowPrefabrbs[i].AddForce(new Vector2(0, playerStats.chosenStats.projectileSpeed));
                }
                else if (arrowPrefabrbs[i].gameObject.name.Contains("Left"))
                {
                    arrowPrefabrbs[i].AddForce(new Vector2(-playerStats.chosenStats.projectileSpeed, 0));
                }
                else if (arrowPrefabrbs[i].gameObject.name.Contains("Down"))
                {
                    arrowPrefabrbs[i].AddForce(new Vector2(0, -playerStats.chosenStats.projectileSpeed));
                }
                else
                {
                    arrowPrefabrbs[i].AddForce(new Vector2(playerStats.chosenStats.projectileSpeed, 0));
                }
            }
        }
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
                    //Moving right
                    if (prevDir.x > 0)
                    {
                        PlayAnim("AnimPlayerIdleRight");
                    }
                    //Moving left
                    else if (prevDir.x < 0)
                    {
                        PlayAnim("AnimPlayerIdleLeft");
                    }
                    //Moving up
                    else if (prevDir.y > 0)
                    {
                        PlayAnim("AnimPlayerIdleUp");
                    }
                    //Moving down
                    else if (prevDir.y < 0)
                    {
                        PlayAnim("AnimPlayerIdleDown");
                    }
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
                    prevDir = moveDir;
                    break;
                case playerStates.Attack:
                    PlayerAttack();
                    break;
                case playerStates.Hurt:
                    PlayerHurt();
                    break;
                case playerStates.Skill:
                    break;
                case playerStates.Ultimate:
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
        //Play different attack animation based on chosenClass
        switch (playerStats.chosenClass)
        {
            case ScriptablePlayerStats.playerClass.Archer:
                //Moving left
                if (prevDir.x < 0)
                {
                    PlayAnim("AnimPlayerShootLeft");
                }
                //Moving up
                else if (prevDir.y > 0)
                {
                    PlayAnim("AnimPlayerShootUp");
                }
                //Moving down
                else if (prevDir.y < 0)
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
                if (prevDir.x < 0)
                {
                    PlayAnim("AnimPlayerCastLeft");
                }
                //Moving up
                else if (prevDir.y > 0)
                {
                    PlayAnim("AnimPlayerCastUp");
                }
                //Moving down
                else if (prevDir.y < 0)
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
                if (prevDir.x < 0)
                {
                    PlayAnim("AnimPlayerSlashLeft");
                }
                //Moving up
                else if (prevDir.y > 0)
                {
                    PlayAnim("AnimPlayerSlashUp");
                }
                //Moving down
                else if (prevDir.y < 0)
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

    //Spawn archer arrows. Called in animation events
    private void SpawnArrow()
    {
        //left
        if (prevDir.x < 0)
        {
            arrowPrefabrbs.Add(Instantiate(arrowPrefab[1], transform).GetComponent<Rigidbody2D>());
        }
        //up
        else if (prevDir.y > 0)
        {
            arrowPrefabrbs.Add(Instantiate(arrowPrefab[0], transform).GetComponent<Rigidbody2D>());
        }
        //down
        else if (prevDir.y < 0)
        {
            arrowPrefabrbs.Add(Instantiate(arrowPrefab[2], transform).GetComponent<Rigidbody2D>());
        }
        //right
        else
        {
            arrowPrefabrbs.Add(Instantiate(arrowPrefab[3], transform).GetComponent<Rigidbody2D>());
        }
    }

    //Called in animation events
    private bool ShootArrow()
    {
        return true;
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
