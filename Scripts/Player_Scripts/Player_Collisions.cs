using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player_Collisions : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] Player_Movement playerMovementScript;
    [SerializeField] BoxCollider2D[] playerBoxColliders;
    [SerializeField] BoxCollider2D playerGroundDetector;
    [SerializeField] BoxCollider2D headCollisionDetector;
   
    // String Constants
    private const string GroundTag = "Ground";
    private const string PassthroughPlatform = "PassthroughPlatform";
    private const string EnemyTag = "Enemy";

    bool canCollide = true;

    void Start()
    {
        LocatedColliderReferences();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (playerMovementScript.PlayerIsDead) { return; }
        if (!canCollide) {return; }
        if(collision.gameObject.tag == EnemyTag)
        {
            canCollide = false;
            Event_Manager.OnEndLevelTriggered(Event_Manager.LevelScenarioState.GameOver);
        }
        if (collision.gameObject.tag == GroundTag)
        {
           
            if (collision.otherCollider == playerGroundDetector && playerMovementScript.IsFalling) // I think other collider in this instance refers to the player's collider     
            {                                                    // and "Other", simply means, the collider that hasnt been detected in this collision instance ( The ground collider )
                playerMovementScript.IsGrounded = true;
          //      Debug.Log("Ground tag trigger activated");
             
            }
            else if(collision.otherCollider == headCollisionDetector)
            {
                playerMovementScript.EndJumpProcess();
            }
        }

        if(collision.gameObject.tag == PassthroughPlatform)
        {
            Physics2D.IgnoreCollision(collision.collider, headCollisionDetector);
            if(playerGroundDetector.transform.position.y < collision.gameObject.transform.position.y) 
            {

                Physics2D.IgnoreCollision(collision.collider, playerGroundDetector, true); ; 
                return; 
            }
            else { 
                Physics2D.IgnoreCollision(collision.collider, playerGroundDetector, false); ;
            }

            if (collision.otherCollider == playerGroundDetector && playerMovementScript.IsFalling) // I think other collider in this instance refers to the player's collider     
            {                                                    // and "Other", simply means, the collider that hasnt been detected in this collision instance ( The ground collider )
                playerMovementScript.IsGrounded = true;
      //          Debug.Log("Passthrough Platform trigger activated");

            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == GroundTag || collision.gameObject.tag == PassthroughPlatform)
        {
        
            if (collision.otherCollider == playerGroundDetector)
            {
                playerMovementScript.IsGrounded = false;
           //     Debug.Log("Player has left: " + collision.collider.name);
               
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Level_Select_Portal_Trigger>() != null)
        {
          //  gameObject.GetComponent<PlayerAnimationControl>().portalTriggerMethodExecutionTEST = collision.GetComponent<Level_Select_Portal_Trigger>();
     
        }
    }

    // Internal Script Logic
    private void LocatedColliderReferences()
    {
        playerMovementScript = GetComponent<Player_Movement>();
        playerBoxColliders = GetComponents<BoxCollider2D>();

        playerGroundDetector = playerBoxColliders[0];
        headCollisionDetector = playerBoxColliders[1];
    }
  

}