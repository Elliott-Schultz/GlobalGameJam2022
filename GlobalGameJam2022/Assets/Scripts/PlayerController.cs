using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Input from player
    private PlayerInput playerInput;

    //Switching variables
    public PlayerSwitchingManager switchingManager;
    public bool canSwitch = true;
    private bool wantToSwitch = false;
    public bool turn;

    //Make sure start only gets called once
    private bool started = false;

    //Store our controls
    private InputAction jumpAction;
    private InputAction moveAction;
    private InputAction switchAction;
    private InputAction lightAttack;
    private InputAction heavyAttack;


    //Rigidbody of player
    private Rigidbody2D rb;

    //Movement modifiers
    public float jumpForce;
    public float movementModifier;

    public float inputX;

    //private CircleCollider2D groundCheck;
    private bool onGround;

    //Player health
    private int maxHealth;
    public int health;
    public bool isDead;
    public float hit;
    public bool canMove = true;

    //Knockback
    private float knockBack;
    public bool hitFromRight;
    private bool hitGroundAfterHit = true;

    //Animation
    private Animator playerAnimator;
    public bool facingRight = true;
    public bool dontFlip = false;

    #region Movement
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        jumpAction = playerInput.actions["Jump"];
        switchAction = playerInput.actions["Switch"];
        lightAttack = playerInput.actions["Light Attack"];
        heavyAttack = playerInput.actions["Heavy Attack"];
    }

    private void Start()
    {
        maxHealth = health;

        playerAnimator = GetComponent<Animator>();

        started = true;
        jumpAction.performed += Jump;
        switchAction.performed += Switch;
        lightAttack.performed += LightAttack;
        heavyAttack.performed += HeavyAttack;

        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        if (!started)
        {
            Start();
        }

        moveAction = playerInput.actions["Move"];
        moveAction.canceled += StopMoving;
        jumpAction.performed += Jump;
        switchAction.performed += Switch;
        lightAttack.performed += LightAttack;
        heavyAttack.performed += HeavyAttack;
    }

    private void OnDisable()
    {
        jumpAction.performed -= Jump;
        switchAction.performed -= Switch;
        lightAttack.performed -= LightAttack;
        heavyAttack.performed -= HeavyAttack;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (onGround && canMove)
        {
            playerAnimator.SetBool("OnGround", false);
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void Switch(InputAction.CallbackContext context)
    {
        if(turn && canSwitch)
        {
            Debug.Log("Switch");
            switchingManager.Switch();
        }
        else if(turn)
        {
            wantToSwitch = true;
        }
    }

    private void LightAttack(InputAction.CallbackContext context)
    {
        
    }

    private void HeavyAttack(InputAction.CallbackContext context)
    {
        rb.velocity = Vector2.zero;

        if (hitGroundAfterHit && onGround)
        {
            canSwitch = false;
            canMove = false;

            rb.velocity = Vector2.zero;

            playerAnimator.SetBool("HeavyAttack", true);
        }
        else
        {
            canSwitch = false;
            playerAnimator.SetBool("AirAttack", true);
        }
    }

    public void heavyDrop()
    {
        Debug.Log("Drop");
        canMove = false;
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(0, 2 * -jumpForce), ForceMode2D.Impulse);
    }

    public void Move(InputAction.CallbackContext context)
    {
        inputX = context.ReadValue<Vector2>().x;

        playerAnimator.SetFloat("Speed", Mathf.Abs(inputX));
    }

    public void StopMoving(InputAction.CallbackContext context)
    {
        //rb.velocity = new Vector2(0, rb.velocity.y);
    }

    private void Update()
    {
        if(!dontFlip)
        {
            if (rb.velocity.x > 0 && !facingRight)
            {
                Flip();
            }
            else if (rb.velocity.x < 0 && facingRight)
            {
                Flip();
            }
        }

        if (hit <= 0 && hitGroundAfterHit && canMove)
        {
            rb.velocity = new Vector2(inputX * movementModifier, rb.velocity.y);
        }
        else if (hit > 0)
        {
            rb.velocity = Vector2.zero;
            if (hitFromRight)
            {
                if (!facingRight)
                {
                    dontFlip = true;
                    Flip();
                }
                rb.AddForce(new Vector2(-knockBack, knockBack), ForceMode2D.Impulse);
                //rb.velocity = new Vector2(-knockBack, knockBack);
            }
            else
            {
                if (facingRight)
                {
                    dontFlip = true;
                    Flip();
                }
                rb.AddForce(new Vector2(knockBack, knockBack), ForceMode2D.Impulse);
                //rb.velocity = new Vector2(knockBack, knockBack);
            }
            hit -= Time.deltaTime;
        }

        if(canSwitch && wantToSwitch)
        {
            Debug.Log("Update Switch");
            wantToSwitch = false;
            switchingManager.Switch();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            if (health <= 0)
            {
                Die();
            }
            else
            {
                playerAnimator.SetBool("OnGround", true);
                if (!hitGroundAfterHit)
                {
                    canSwitch = true;
                    hitGroundAfterHit = true;
                    rb.velocity = Vector2.zero;
                    playerAnimator.SetBool("Hit", false);
                    dontFlip = false;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            onGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            onGround = false;
        }
    }

    public void SetTurn()
    {
        turn = true;
    }

    public void SetNotTurn()
    {
        turn = false;
    }

    #endregion

    public void changeHealth(int changeBy, bool isDamage, Vector2 positionOfDamageSource, float knockBack)
    {
        if (isDamage && hitGroundAfterHit)
        {
            playerAnimator.SetBool("HeavyAttack", false);
            playerAnimator.SetBool("AirAttack", false);

            health -= changeBy;
            if(health > 0)
            {
                this.knockBack = knockBack;
                if (positionOfDamageSource.x < transform.position.x)
                {
                    hitFromRight = false;
                }
                else
                {
                    hitFromRight = true;
                }
            }
        }
        else if(!isDamage)
        {
            health += changeBy;
            if (health >= maxHealth)
            {
                health = maxHealth;
            }
        }
        
        
    }

    public void Die()
    {
        isDead = true;
        switchingManager.Switch();
    }
    
    public bool getIsDead()
    {
        return isDead;
    }

    public void isHit(float hitLength)
    {
        if (hitGroundAfterHit)
        {
            hit = hitLength;
            hitGroundAfterHit = false;
            canSwitch = false;
            playerAnimator.SetBool("Hit", true);
        }
    }

    public void setFacingRight(bool facingRight)
    {
        this.facingRight = facingRight;
    }

    public void setDontFlip(bool dontFlip)
    {
        this.dontFlip = dontFlip;
    }

    public bool getFacingRight()
    {
        return facingRight;
    }

    public bool getDontFlip()
    {
        return dontFlip;
    }

    public void Flip()
    {
        Debug.Log("Flip");
        facingRight = !facingRight;
        transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    public void CanMove()
    {
        playerAnimator.SetBool("HeavyAttack", false);
        playerAnimator.SetBool("AirAttack", false);

        canMove = true;
        canSwitch = true;
    }

    public bool getHitGroundAfterHit()
    {
        return hitGroundAfterHit;
    }

    public void setHitGroundAfterHit(bool newHitGroundAfterHit)
    {
        hitGroundAfterHit = newHitGroundAfterHit;
    }
}
