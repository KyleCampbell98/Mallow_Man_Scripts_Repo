using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Pickup_Collision_Base_Script : MonoBehaviour
{
    protected bool canCollide = true;
    protected string playerTag = "Player";
    protected abstract void OnTriggerEnter2D(Collider2D collision);
    
       
    

    protected void EndCollision()
    {
        canCollide = false;
        Destroy(this.gameObject);
    }
}
