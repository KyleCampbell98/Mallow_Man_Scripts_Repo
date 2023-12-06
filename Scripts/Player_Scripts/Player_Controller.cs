using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Player_Movement))]
/* 
 * This script modifies and extends the basic movement funtions carried out within my "Player_Movement" script.
 * So a "Player_Movement" component will always be required when using this script.
 */

public class Player_Controller : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Player_Movement playerMovementScript;

    [Header("Movement Modifier Configs")]
    [Range(2, 8f)][SerializeField] private float playerSlowdownDampening = 1f;
    [Range(150,300)][SerializeField] private float playerFallMultiplier = 1f;
    [Range(1, 500)][SerializeField] private float playerFallDampening = 1f;
    [SerializeField] private float currentTerminalVelocityYValue;

    [Header("Player Controller States")]
    [SerializeField] private bool isSlowing;
    [SerializeField] private bool hasReachedTerminalVelocity;

    // Properties
    public bool IsSlowing { get { return isSlowing; } private set { isSlowing = value; } }
    void Start()
    {
        LocateReferences();
        DelegateSubscriptions();

    }
    private void FixedUpdate()
    {
        EditSlowdown();
        EditGravity();
    }

    private void EditSlowdown()
    {
        if (IsSlowing)
        {
            if (playerMovementScript.HorizontalInput == 0 && Mathf.Abs(playerMovementScript.PlayerRigidBody.velocity.x) > 0.5)
            {
                playerMovementScript.PlayerRigidBody.AddForce(new Vector2(-playerMovementScript.PlayerRigidBody.velocity.x, 0) * playerSlowdownDampening, ForceMode2D.Force);
            }
            else if (playerMovementScript.HorizontalInput == 0 && Mathf.Abs(playerMovementScript.PlayerRigidBody.velocity.x) <= 0.5f)
            {
                playerMovementScript.PlayerRigidBody.velocity = new Vector2(0, playerMovementScript.PlayerRigidBody.velocity.y);
          
                IsSlowing = false;
            }
        }
        
     
        
    }
    private void CheckIsSlowing(float prevHorizontalInpuitValue)
    {
        if(MathF.Abs(prevHorizontalInpuitValue) == 1 ){ IsSlowing = true; }
     
    }
    private void EditGravity()
    {
        
        if (playerMovementScript.IsFalling)
        {
            playerMovementScript.PlayerRigidBody.AddForce(Vector2.down * playerFallMultiplier, ForceMode2D.Force);
          //  Debug.Log("Applying Downward thrust");
        }
        if(playerMovementScript.IsFalling && playerMovementScript.PlayerRigidBody.velocity.y <= currentTerminalVelocityYValue)
        {
          //  HasReachedTerminalVelocity = true;
           playerMovementScript.PlayerRigidBody.AddForce(Vector2.up * (playerFallMultiplier * 2), ForceMode2D.Force);
            Debug.Log("Applying upthrust for Terminal velocity");
        }
    }
    private void LocateReferences()
    {
        playerMovementScript = GetComponent<Player_Movement>();

    }
    private void DelegateSubscriptions()
    {
        Player_Movement.m_CheckForSlowing += CheckIsSlowing;
    }

    private void OnDestroy()
    {

        Player_Movement.m_CheckForSlowing -= CheckIsSlowing;
    }

}
