using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Moving,
        Sprinting,
        Idle,
        Jumping,
        Death
    }

    [Header("Movement")]
    [SerializeField] float moveSpeed = 10;
    [SerializeField] float sprintMod = 1.5f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float sprintJumpMod = 1.5f;
    public float jumpBuffer = 0.8f;
    private float jumpBufferTime;

    [Header("State Machine")]
    [SerializeField] PlayerState playerState;

    private float playerHealth;

    Rigidbody2D rb;
    Animator anim;

    // Getter function for current player state;
    public PlayerState GetPlayerState() { return playerState; }

    // Setter function to change current player state. Keeping private for now until functionality is needed for outside classes.
    private void SetPlayerState(PlayerState newState) { playerState = newState; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get rigidbody component.
        rb.linearDamping = 0.5f; // Steadier jumping.
        anim = GetComponent<Animator>(); // Get animator component.
        playerHealth = 100; // Set health to maximum.
    }

    // Obtain the current horizontal direction being inputted by the player.
    private float HorizontalInputChecker()
    {
        return UnityEngine.Input.GetAxis("Horizontal");
    }

    // Flip the sprite whenever the player starts moving in a direction opposite to where they were facing.
    private void Flip(float horizontalInput)
    {
        bool facingRight = transform.localScale.x > 0;
        bool inputingRight = horizontalInput > 0;
        if (playerState != PlayerState.Jumping && (facingRight != inputingRight) && (horizontalInput != 0))
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    // Check the curretn state of the player.
    private void StateChecker(float horizontalInput)
    {
        if (playerHealth <= 0)
        {
            SetPlayerState(PlayerState.Death);
            anim.Play("Player_death");
        }
        else if (rb.linearVelocity.y != 0)
        {
            SetPlayerState(PlayerState.Jumping);
            anim.Play("Player_jump");
        }
        else if (horizontalInput == 0)
        {
            SetPlayerState(PlayerState.Idle);
            anim.Play("Player_idle");
        }
        else if (UnityEngine.Input.GetKeyDown(KeyCode.LeftShift))
        {
            SetPlayerState(PlayerState.Sprinting);
            anim.Play("Player_sprint");
        }
        else
        {
            SetPlayerState(PlayerState.Moving);
            anim.Play("Player_run");
        }
    }

    // Checks if character should jump/
    private void Jump()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Space) && playerState != PlayerState.Jumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    // Handles movement.
    private void Move(float horizontalInput)
    {
        // Sprint if left shift is pressed.
        if (UnityEngine.Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed *= sprintMod;
            jumpForce *= sprintJumpMod; rb.linearDamping += 0.2f;
        }
        // End sprint when left shift is released.
        if (UnityEngine.Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed /= sprintMod;
            jumpForce /= sprintJumpMod; rb.linearDamping -= 0.2f;
        }

        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        Jump();
    }

    void Update()
    {
        float horizontalInput = HorizontalInputChecker();
        Move(horizontalInput);
        StateChecker(horizontalInput);
        Flip(horizontalInput);
    }
}
