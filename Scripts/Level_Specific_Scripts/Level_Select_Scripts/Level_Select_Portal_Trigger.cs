using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Level_Select_Portal_Trigger : Level_Goal_Trigger_Behaviour
{
    private SceneNameCacheSO sceneNameCacheTemp;
    private Level_Select_Sign_Trigger parent_Sign_Trigger_Script;
    [SerializeField] private BoxCollider2D portalCollider;
    [SerializeField] private float secondsToWaitAfterResizeForColliderEnable = 1.5f;
  
    private void Start()
    {
        canCollide = true;
        parent_Sign_Trigger_Script = GetComponentInParent<Level_Select_Sign_Trigger>();
        sceneNameCacheTemp = ScriptableObject.CreateInstance("SceneNameCacheSO") as SceneNameCacheSO;
        if(portalCollider == null)
        {
            Debug.LogError("Portal collider reference hasn't " +
                "been set in the inspector. " +
                "Enabling and disabling of collider upon zooming in and out on portal can not function without a reference.");
        }
       
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (canCollide)
        {
            if (collision.gameObject.tag == "Player")
            {

               base.OnTriggerEnter2D(collision);
                PlayerAnimationControl.portalFallFinished += AnimCallBackLevelLoad;
                LevelGoalSpecificLogic(Event_Manager.LevelScenarioState.LevelSelectLoad);
            }
              
        }
    }
   

    public void AnimCallBackLevelLoad()
    {
        Event_Manager.OnSceneLoadNeeded(sceneNameCacheTemp.ReturnConstSceneName(parent_Sign_Trigger_Script.InfoAssociatedWithThisLevel.LevelThisSORelatesTo));
    }

    private void OnEnable()
    {
        StartCoroutine(ColliderEnableDelay(secondsToWaitAfterResizeForColliderEnable));
    }
    private void OnDisable()
    {
        portalCollider.enabled = false;
    }

    private IEnumerator ColliderEnableDelay(float secondsToWait)
    {
        yield return new WaitUntil(() => gameObject.transform.localScale == new Vector3(0.5f, 0.5f, 0.5f));
        yield return new WaitForSeconds(secondsToWait);
        
        portalCollider.enabled = true;
    }
}
