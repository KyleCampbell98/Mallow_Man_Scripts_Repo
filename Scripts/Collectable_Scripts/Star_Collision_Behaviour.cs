using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Star_Collision_Behaviour : Pickup_Collision_Base_Script
{

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (canCollide && collision.gameObject.CompareTag(playerTag))
        {
            Debug.Log("Collided with Star");
            base.EndCollision();
            Event_Manager.OnPickupCollected(Event_Manager.collectableType.Star);
            
        }
    }
}
