using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Goal_Trigger_Behaviour : MonoBehaviour
{
    protected bool canCollide = true;
   [SerializeField] protected GameObject playerPriorToPortalFall = null;
    [SerializeField] protected float speedOfPlayerFallingIntoPortal = 100f;
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && canCollide)
        {

            canCollide = false;
            playerPriorToPortalFall = collision.gameObject;
            PortalEnterLogic(playerPriorToPortalFall);
        }
    }

    protected void LevelGoalSpecificLogic(Event_Manager.LevelScenarioState stateWhenPortalEntered)
    {
        Event_Manager.OnEndLevelTriggered(stateWhenPortalEntered);
    }

    protected void PortalEnterLogic(GameObject player)
    {
        player.transform.parent = this.transform;
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody2D>().isKinematic = true;
        StartCoroutine(DragPlayerToPortalMiddle(player, speedOfPlayerFallingIntoPortal));
      //  player.transform.localPosition = Vector3.zero;
    }

    private IEnumerator DragPlayerToPortalMiddle(GameObject player, float moveSpeed)
    {
        Debug.Log("Coroutine for player portal movement fired");
        float timeToStartLerp = 0;
        while(timeToStartLerp < 1)
        {
            player.transform.localPosition = Vector3.Lerp(player.transform.localPosition, Vector3.zero, timeToStartLerp);
            timeToStartLerp += Time.deltaTime / moveSpeed;
            yield return null;
        }
    }
}
