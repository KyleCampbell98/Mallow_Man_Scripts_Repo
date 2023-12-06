using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

[RequireComponent(typeof(Rigidbody2D))]

public class Player_Movement : MonoBehaviour
{
    // Event Management
    public delegate void ChangedHoriValue(float previousHorizontalValue);
    public static ChangedHoriValue m_CheckForSlowing;
    public static ChangedHoriValue m_CheckForSpriteFlip;

    public delegate void AnimationNotification(string boolToSet, bool currentState);
    public static AnimationNotification m_AnimationNotification;

    public delegate void SetAnimTrigger(string triggerToSet);
    public static SetAnimTrigger m_SetAnimTrigger;

    public delegate void SetAnimState(bool currentState);
    public static SetAnimState m_SetGroundedState;

    public static event UnityAction PlayerHasJumped;

    [Header("Player Movement Configs")]
    [SerializeField] private float playerAcceleration;
    [SerializeField] private float maxVelocity = 30f;
    [SerializeField] private float isMovingThreshold = 1f; // the minimum threshold to detect whether the player is classed as stationary or not.

    [Header("Player Jump Configs (Add Force based)")]
    [SerializeField] private float calculatedJumpPower;
    [SerializeField] private float targetJumpHeight;
    [SerializeField] float maxJumpButtonHoldTime = 0.3f;
    [SerializeField] float jumpButtonCurrentHoldTime = 0f;


    [Header("Horizontal input axis")]
    [SerializeField] private float horizontalInput;

    [Header("Player State fields")]
    [SerializeField] private bool jumpInputTaken;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isFalling;
    [SerializeField] private bool isMoving;
    [SerializeField] private bool canTakeInput;
    [SerializeField] private bool playerIsDead;

    // Properties
    public float PlayerAcceleration { get { return playerAcceleration; } }
    public bool JumpInputTaken { get { return jumpInputTaken; } private set { jumpInputTaken = value; if (JumpInputTaken) { PlayerJump(); } } }
    public bool IsGrounded
    {
        get { return isGrounded; }
        set
        {
            isGrounded = value;
            if (value == true)
            {
                IsFalling = false;
                jumpButtonCurrentHoldTime = 0;
                m_SetAnimTrigger?.Invoke("Ground");
            }
            m_AnimationNotification?.Invoke(nameof(IsGrounded), value);

        }
    }
    public bool IsFalling { get { return isFalling; } private set { isFalling = value; } }
    public bool IsMoving { get { return isMoving; } private set { isMoving = value; m_AnimationNotification?.Invoke(nameof(IsMoving), value); } }
    public float HorizontalInput
    {
        get { return horizontalInput; }
        set
        {
            float previousHoriInputValue = horizontalInput;
            horizontalInput = value;
            m_CheckForSlowing?.Invoke(previousHoriInputValue);
            m_CheckForSpriteFlip?.Invoke(value);


        }
    }
    public bool CanTakeInput { get { return canTakeInput; } set { canTakeInput = value; if (value == false) { HorizontalInput = 0; } } }
    public float CalculatedJumpPower { get { return calculatedJumpPower; } private set { calculatedJumpPower = value; } }
    public bool PlayerIsDead {  get { return playerIsDead; } private set { playerIsDead = value; if (value == true) {
                PlayerRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
                m_SetAnimTrigger.Invoke(nameof(PlayerIsDead)); } } }

    // Component Properties
    public Rigidbody2D PlayerRigidBody { get; private set; }

    // String Constants
    private const string HorizontalAxis = "Horizontal";
    private const string JumpInput = "Jump";
    

    // Unity Method Execution

    void Start()
    {
        LocateComponentReferences();
        JumpHeightCalculator(targetJumpHeight);
        CanTakeInput = true;
        Event_Manager.EndLevelTrigger += SetPlayerControlAbility;
        
    }

    private void FixedUpdate()
    {
        PlayerHorizontalMovement(HorizontalInput);
    }

    void Update()
    {
        if (CanTakeInput)
        {
            CapturePlayerHorizontalInput();
            PlayerJumpInputCheck();
        }
        CheckIfFalling();
        CheckIfMoving();

    }

    // Input Capture Methods
    private void PlayerJumpInputCheck()
    {
        if (Input.GetButtonDown(JumpInput) && IsGrounded)
        {
            JumpInputTaken = true;

        }

        if (JumpInputTaken && Input.GetButton(JumpInput))
        {
            jumpButtonCurrentHoldTime += Time.deltaTime;
        }

        if (!IsGrounded)
        {
            if (Input.GetButtonUp(JumpInput) || jumpButtonCurrentHoldTime >= maxJumpButtonHoldTime)
            {
                EndJumpProcess();
            }
        }
    }

    private void CapturePlayerHorizontalInput()
    {
        if (HorizontalInput != Input.GetAxisRaw(HorizontalAxis)) // This line seems to prevent the Horizontal input being set every frame, and only sets it when the current value doesnt equal the new value
        {
            HorizontalInput = Input.GetAxisRaw(HorizontalAxis);
        }
    }

    // Movement Execution Methodd
    private void PlayerJump()
    {
        if (IsGrounded)
        {
            PlayerHasJumped?.Invoke();
            m_SetAnimTrigger?.Invoke(JumpInput);
            PlayerRigidBody.AddForce(Vector2.up * calculatedJumpPower, ForceMode2D.Impulse);

        }
    }

    private void PlayerHorizontalMovement(float inputAxis)
    {
        if (inputAxis != 0)
        {

            if (MathF.Abs(PlayerRigidBody.velocity.x) > maxVelocity)
            {
                Mathf.Clamp(PlayerRigidBody.velocity.x, Mathf.Sign(inputAxis) * maxVelocity, maxVelocity);
            }
            else { PlayerRigidBody.AddForce(new Vector2(inputAxis * PlayerAcceleration, 0), ForceMode2D.Force); }
        }

    }

    // Movenment Logic Methods 
    public void EndJumpProcess()
    {
        JumpInputTaken = false;
        jumpButtonCurrentHoldTime = 0;
        IsFalling = true;
    }
    private void CheckIfFalling()
    {
        if (PlayerRigidBody.velocity.y < 0 && !IsGrounded)
        {
            IsFalling = true;
        }
        else if (IsGrounded)
        {
            IsFalling = false;
        }
    }
    private void CheckIfMoving()
    {
        if (Mathf.Abs(PlayerRigidBody.velocity.x) <= isMovingThreshold)
        {
            IsMoving = false;
        }
        else { IsMoving = true; }
    }


    // Internal Script Logic
    private void LocateComponentReferences()
    {
        PlayerRigidBody = GetComponent<Rigidbody2D>();
    }
    public void JumpHeightCalculator(float targetHeightForCalculation)
    {
        CalculatedJumpPower = MathF.Sqrt(targetHeightForCalculation * -2 * (Physics2D.gravity.y * PlayerRigidBody.gravityScale));
    }
    private void SetPlayerControlAbility(Event_Manager.LevelScenarioState currentState)
    {
        switch (currentState)
        {
            case Event_Manager.LevelScenarioState.InPlay:
                CanTakeInput = true;
                break;

            case Event_Manager.LevelScenarioState.LevelComplete:
                CanTakeInput = false;
                m_SetAnimTrigger("Enter_Portal");
                break;
            case Event_Manager.LevelScenarioState.LevelSelectLoad:
                CanTakeInput = false;
                m_SetAnimTrigger("Enter_Portal");
                break;
            case Event_Manager.LevelScenarioState.GameOver:
                CanTakeInput = false;
                PlayerIsDead = true;
                break;
            default: // Handles LevelComplete And Game Over scenarios.
                CanTakeInput = false; 
                
                break;
        }

    }



    private void OnDestroy()
    {
        m_AnimationNotification = null;
        m_SetAnimTrigger = null;
        m_SetGroundedState = null;
        m_CheckForSpriteFlip = null;
        m_CheckForSlowing = null;
        
    }

}
