using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Windows;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

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
    public static PlayerController player;

    [Header("State Machine")]
    [SerializeField] PlayerState playerState;

    [SerializeField] private int playerHealth;
    [SerializeField] private HealthUI healthUI;
    private bool iFrame = false;
    private float deathTime;

    Rigidbody2D rb;
    Animator anim;

    [SerializeField] PlayerSounds soundController;

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
        player = this;
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
        if (playerState != PlayerState.Jumping && playerState != PlayerState.Death && (facingRight != inputingRight) && (horizontalInput != 0))
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    // Remove (or add) health from the player.
    public void Damage(int dmg) {
        if (!iFrame)
        {
            playerHealth -= dmg;
            StartCoroutine(DamageTimer());
        }

        // Prevent player's health from exceeding the maximum.
        if (playerHealth > 100)
        {
            playerHealth = 100;
        }

        // Update health UI to reflect current health.
        healthUI.ChangeHealthText(playerHealth);
    }

    // If the player just landed on a surface, play the landing sound.
    private void CheckIfLanded()
    {
        if (playerState == PlayerState.Jumping)
        {
            soundController.LandSound();
        }
    }

    // Start playing walking sound if player started walking/sprinting. (currently unused)
    private void CheckIfStartedMoving()
    {
        if (playerState != PlayerState.Moving && playerState != PlayerState.Sprinting)
        {
            Debug.Log("started moving");
            soundController.WalkSound();
        }
    }

    // Stop playing walking sound if player stopped walking/sprinting. (currently unused)
    private void CheckIfStoppedMoving()
    {
        if (playerState == PlayerState.Moving && playerState == PlayerState.Sprinting)
        {
            soundController.StopWalkSound();
        }
    }

    IEnumerator DeathScreenTimer()
    {
        deathTime = 4.5f;
        while (deathTime > 0)
        {
            deathTime -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        SceneManager.LoadScene("Death");
    }

    // Check the current state of the player.
    private void StateChecker(float horizontalInput)
    {
        if (playerHealth <= 0)
        {
            SetPlayerState(PlayerState.Death);
            anim.Play("Player_death");

            // Start death screen timer.
            StartCoroutine(DeathScreenTimer());
        }
        else if (rb.linearVelocity.y != 0)
        {
            SetPlayerState(PlayerState.Jumping);
            anim.Play("Player_jump");
        }
        else if (horizontalInput == 0)
        {
            CheckIfLanded();
            SetPlayerState(PlayerState.Idle);
            anim.Play("Player_idle");
        }
        else if (UnityEngine.Input.GetKeyDown(KeyCode.LeftShift))
        {
            CheckIfLanded();
            SetPlayerState(PlayerState.Sprinting);
            anim.Play("Player_sprint");
        }
        else
        {
            CheckIfLanded();
            SetPlayerState(PlayerState.Moving);
            anim.Play("Player_run");
        }
    }

    // Checks if character should jump
    private void Jump()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Space) && playerState != PlayerState.Jumping && playerState != PlayerState.Death)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            soundController.JumpSound();
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

        if (playerState != PlayerState.Death)
        {
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
        
        Jump();
    }

    // Prevents player from being instantly killed by touching a mole.
    IEnumerator DamageTimer()
    {
        float cooldown = 2f;
        iFrame = true;
        while (cooldown > 0)
        {
            cooldown -= 1f;
            yield return new WaitForSeconds(1f);
        }
        iFrame = false;
    }

    void Update()
    {
        float horizontalInput = HorizontalInputChecker();
        Move(horizontalInput);
        StateChecker(horizontalInput);
        Flip(horizontalInput);
    }
}
