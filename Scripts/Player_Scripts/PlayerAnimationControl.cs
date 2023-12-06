using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Player_Movement))]
public class PlayerAnimationControl : MonoBehaviour
{

    //References
   [SerializeField] private Animator playerAnimController;
   [SerializeField] private SpriteRenderer playerSpriteRenderer;

    public static event UnityAction portalFallFinished;


   // public Level_Select_Portal_Trigger portalTriggerMethodExecutionTEST;

    // Start is called before the first frame update
    void Start()
    {
        
        GetComponentReferences();
        DelegateSubscriptions();
    }

    private void SetAnimStateBool(string boolToSet, bool currentState)
    {
        playerAnimController.SetBool(boolToSet, currentState);
    }

    private void SetAnimTrigger(string triggerToSet)
    {
        playerAnimController.SetTrigger(triggerToSet);

    }
    private void FlipSprite(float currentHoriInput)
    {
        Debug.Log("Flip Sprite Called");
        if (currentHoriInput == 0) { return; }
        playerSpriteRenderer.flipX = currentHoriInput == -1 ? true : false;
    }

    // Internal Script Logic
    private void DelegateSubscriptions()
    {
        Player_Movement.m_AnimationNotification += SetAnimStateBool;
        Player_Movement.m_SetAnimTrigger += SetAnimTrigger;
        Player_Movement.m_CheckForSpriteFlip += FlipSprite;
    }
    private void GetComponentReferences()
    {
       // Debug.Log("Player Anim Controller Get Components called");
        if (playerAnimController == null)
        {
            playerAnimController = GetComponent<Animator>();
        }
        if (playerSpriteRenderer == null)
        {
            playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
    }

    public void AnimFinishedCallBack()
    {
       // portalTriggerMethodExecutionTEST.AnimCallBackLevelLoad();
       portalFallFinished?.Invoke();
       portalFallFinished = null;
    }

    public void DestroyPlayer()
    {
        Destroy(this.gameObject);
    }
  
}
