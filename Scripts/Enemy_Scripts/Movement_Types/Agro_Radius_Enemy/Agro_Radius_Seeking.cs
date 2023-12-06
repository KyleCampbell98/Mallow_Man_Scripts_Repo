using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Agro_Radius_Seeking : Enemy_Movement
{
    [Header("Agro Movement Parameters")]
    [SerializeField] private float returnToDefaultPosSpeedDivider = 2;
    [SerializeField] private float chaseSpeedDivider = 2;
    [SerializeField] private float activeAgroSpeed;

    [Header("Agro Radius Cache/Parameters")]
    [Range(3, 10)][SerializeField] private float agroRadius;
    [SerializeField] private GameObject agroRadiusGameObject;

    [Header("Agro Seeking Vectors")]
    [SerializeField] private Vector2 targetDestination;
    [SerializeField] private Vector2 defaultPos;

    // Properties
    public Vector2 TargetDestination { get { return targetDestination; } set { targetDestination = new Vector2(value.x, transform.position.y); } }
    public float ActiveAgroSpeed { 
        get 
        {
            return activeAgroSpeed; 
        } 
        set 
        { 
            activeAgroSpeed = value / chaseSpeedDivider;
            Debug.Log($"Agro Speed is currently = {ActiveAgroSpeed}");
        } 
    }

    void Start()
    {
        LocateComponentReferences();
        SetupAgroArea();
    }

    private void Update()
    {
       EnemyMover(ActiveAgroSpeed);
    }

    public override void EnemyMover(float speedToMove)
    { 
        if(transform.position.x == defaultPos.x && TargetDestination == defaultPos) 
        { 
            Debug.Log("Side  moving enemy is already home at default, doesn't need to run script logic for move"); 
            return; 
        } // Could cause issues as this logic will in theory only run if teh enemy is at a specific floating point value for the X pos of the default position. 

        if (TargetDestination == defaultPos)
        {   
            transform.position = Vector2.MoveTowards(transform.position, TargetDestination, (speedToMove / returnToDefaultPosSpeedDivider) * Time.deltaTime);
            // If the enemy is returning to default, this logic will run to simulate a slow moving return, like it is skulking back to position.
        }
        else 
        {
            transform.position = Vector2.MoveTowards(transform.position, TargetDestination, speedToMove * Time.deltaTime);
        }
    }

    public IEnumerator TargetLostBehaviour()
    {
        TargetDestination = transform.position; // Makes the enemy briefly stop moving.
        yield return new WaitForSeconds(secondsBeforeDirectionChange);
        TargetDestination = defaultPos;

    }

    // Internal Script Logic Methods
    protected override void LocateComponentReferences()
    {
        base.LocateComponentReferences();
        agroRadiusGameObject = transform.parent.gameObject;

    }
    private void SetupAgroArea() // !!! Need to add functionality for the setup of a rectangular agro area based on a box colider !!! //
    {
        CircleCollider2D agroRadiusCollider;
        agroRadiusCollider = agroRadiusGameObject.GetComponent<CircleCollider2D>();
        if (agroRadiusCollider != null)
        {
            agroRadiusCollider.radius = agroRadius;
        }
        else { Debug.LogError("Could not find an agro radius collider. Enemy's parent object is likely missing a collider component"); } 
        defaultPos = agroRadiusGameObject.transform.position;
    }

    //Debug Methods
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (agroRadiusGameObject != null)
        {
            Gizmos.DrawWireSphere(agroRadiusGameObject.transform.position, agroRadius);
        }
    }




}
