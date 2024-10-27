using System.Collections;
using System;
using System.Collections.Generic;
using System.Data;
using Unity.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    float xInput = 0;
    [Header("Movement")]
    public float moveSpeed = 25;
    private float baseSpeed;
    public float sprintMod = 1.5f;
    public float jumpForce = 1.2f;
    public float jumpBuffer = 0.8f;
    private float jumpBufferTime;
    [Header("State Machine")]
    [SerializeField]PlayerState playerState;
    [SerializeField] List<string> stateNames;
    [SerializeField] List<PlayerState> stateTypes;
    Rigidbody2D rb;
    Dictionary<string,PlayerState> states;
    // Start is called before the first frame update
    void Start()
    {
        SetStates();
        baseSpeed += moveSpeed;
        jumpBufferTime += jumpBuffer;
        rb = GetComponent<Rigidbody2D>();
        rb.linearDamping = 0.5f; //makes player jump more steady
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Jump();
        IdleCheck();
        jumpBufferTime += Time.deltaTime;
    }

    private void StateCheck(string mode)
    {
        if (states.ContainsKey(mode))
        {
            states.TryGetValue(mode, out PlayerState state);
            playerState = state;
        }
    }

    private void IdleCheck()
    {
        if(rb.linearVelocity.x == 0 && rb.linearVelocity.y == 0)
        {
            StateCheck("Idle");
        }

    }

    private void Movement()
    {
        //Start Sprint
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            moveSpeed *= sprintMod;
            jumpForce *= 1.2f; rb.linearDamping += 0.2f;
        }
        //End sprint
        if (Input.GetKeyUp(KeyCode.LeftShift)){
            moveSpeed /= sprintMod;
            jumpForce /= 1.2f; rb.linearDamping -= 0.2f;
        }
        //Left and right arrows, A, and D for movement controls
        xInput = Input.GetAxis("Horizontal");

        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);
        if (xInput !=0 && moveSpeed == baseSpeed && playerState != PlayerState.Jumping) 
            StateCheck("Moving");
        else if (xInput !=0 && moveSpeed != baseSpeed && playerState != PlayerState.Jumping)
            StateCheck("Sprinting");  
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && rb.linearVelocity.y == 0 && jumpBufferTime > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            StateCheck("Jumping");
            jumpBufferTime = jumpBuffer;
        }
        else
            jumpBufferTime -= Time.deltaTime;
    }

    private void SetStates()
    {
        states = new Dictionary<string, PlayerState>();

        for (int i = 0; i < stateNames.Count; i++)
        {
            if (i < stateTypes.Count)
            {
                states[stateNames[i]] = stateTypes[i];
            }
        }
    }
}

public enum PlayerState
{
    Moving,
    Sprinting,
    Idle,
    Jumping
}
