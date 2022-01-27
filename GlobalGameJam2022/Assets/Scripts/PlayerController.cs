using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;

    public PlayerSwitchingManager switchingManager;
    private bool canSwitch = true;
    private bool wantToSwitch = false;

    private bool started = false;

    //Store our controls
    private InputAction jumpAction;
    private InputAction moveAction;
    private InputAction switchAction;
    private InputAction lightAttack;
    private InputAction heavyAttack;

    private Rigidbody2D rb;

    public float jumpForce;
    public float movementModifier;

    public float inputX;

    private CircleCollider2D groundCheck;
    private bool onGround;

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
        jumpAction.performed += Jump;
        switchAction.performed += Switch;
        lightAttack.performed += LightAttack;
        heavyAttack.performed += HeavyAttack;

        inputX = moveAction.ReadValue<Vector2>().x;

        Update();
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
        if (onGround)
        {
            Debug.Log("Jump");
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void Switch(InputAction.CallbackContext context)
    {
        if(canSwitch)
        {
            switchingManager.Switch();
        }
        else
        {
            wantToSwitch = true;
        }
    }

    private void LightAttack(InputAction.CallbackContext context)
    {

    }

    private void HeavyAttack(InputAction.CallbackContext context)
    {

    }

    public void Move(InputAction.CallbackContext context)
    {
        inputX = context.ReadValue<Vector2>().x;
    }

    private void Update()
    {
        rb.velocity = new Vector2(inputX * movementModifier, rb.velocity.y);

        if(canSwitch && wantToSwitch)
        {
            wantToSwitch = false;
            switchingManager.Switch();
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

    public void setInputX(float newInput)
    {
        inputX = newInput;
    }

    public float getInputX()
    {
        return inputX;
    }
}
