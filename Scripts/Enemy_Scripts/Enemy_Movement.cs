using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
   [Header("Enemy Movement Configs")]
   [SerializeField] protected float enemySpeed = 5f;
    [SerializeField] protected float secondsBeforeDirectionChange;


   [Header("References")]
   [SerializeField] protected Rigidbody2D enemyRigidBody;
   [SerializeField] protected BoxCollider2D detectionCollider;

    [Header("State Control")]
    [SerializeField] protected bool hasStartedCollisionBehaviour = false;


    protected virtual void LocateComponentReferences()
    {
        enemyRigidBody = GetComponent<Rigidbody2D>();
        if (transform.childCount > 0)
        {
            detectionCollider = transform.GetChild(0).GetComponent<BoxCollider2D>();
        }
        /*else { GameObject detectionCollider = Instantiate(gameObject, gameObject.transform.parent.position, transform.localRotation, gameObject.transform);
            Debug.Log("Child object created");
        }*/
    }

    public virtual void EnemyMover(float speedForMove)
    {      
        enemyRigidBody.velocity = Vector2.right * speedForMove;
    }

    protected IEnumerator EdgeReached() // x value used for flipping the enemy in the Y axis once they reach their patrol limit
    {
        float newXScaleValue = FlipEnemyForPatrolDirectionChange();
        gameObject.transform.localScale = new Vector3(newXScaleValue, transform.localScale.y, transform.localScale.z);

        enemySpeed = -enemySpeed; // To move in the opposite direction, the enemies speed is set to that of opposite what they were moving at prior to "Edge Reached" being called. 

        EnemyMover(0); // This makes the enemy pause whilst the "Seconds before direction change" plays out, allowing for a delay on enemy patrol. 
        yield return new WaitForSeconds(secondsBeforeDirectionChange);
        EnemyMover(enemySpeed);
        hasStartedCollisionBehaviour = false;
    }

    private float FlipEnemyForPatrolDirectionChange()
    {
        float oldXScaleValue = gameObject.transform.localScale.x;
        float newXScaleValue = -oldXScaleValue;
        return newXScaleValue;
    }
}
