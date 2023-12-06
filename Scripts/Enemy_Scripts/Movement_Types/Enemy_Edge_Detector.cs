using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Edge_Detector : Enemy_Movement
{
    [Header("References")]
    [SerializeField] private BoxCollider2D groundCollider;

    [Header("Collision Checks")]
    [SerializeField] private bool detectedInitialGroundCol = false;

    [Header("Collision Check Type")]
    [SerializeField] private bool checkingForWalls;
    [SerializeField] private bool checkingForGround;

    void Start()
    {
        LocateComponentReferences();
        EnemyMover(enemySpeed);
    }


    // Collision Methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (checkingForWalls)
        {
            if (collision.gameObject.tag == "Wall" && collision.IsTouching(detectionCollider) && !hasStartedCollisionBehaviour)
            {
                hasStartedCollisionBehaviour = true;
                StartCoroutine(EdgeReached());
            }
        }

        else if (checkingForGround)
        {
            if ((collision.gameObject.tag == "Ground") && !detectedInitialGroundCol)
            {
                //detectedInitialGroundCol = true;
                Debug.Log("Ground Col detected");
                groundCollider = collision.gameObject.GetComponent<BoxCollider2D>();
            }
        }

        else
        {
            return;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (checkingForGround)
        {
            if (!detectionCollider.IsTouching(groundCollider))
            {
                Debug.Log("Ground Col lost");
                StartCoroutine(EdgeReached());

            }
        }
    }

}
