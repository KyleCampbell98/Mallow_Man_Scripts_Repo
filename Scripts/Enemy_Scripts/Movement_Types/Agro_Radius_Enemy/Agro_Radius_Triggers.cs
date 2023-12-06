using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]

public class Agro_Radius_Triggers : MonoBehaviour
{
    [Header("Reference Cache")]
    [SerializeField] private Agro_Radius_Seeking agroSeekingScriptRef;

    // String Constants
    private const string Player = "Player";
  
    void Start()
    {
        agroSeekingScriptRef = gameObject.transform.GetComponentInChildren<Agro_Radius_Seeking>(); // Hard refertence as this script has a direct dependency on another. They should be tightly coupled. 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Player))
        {
            StartCoroutine(agroSeekingScriptRef.TargetLostBehaviour());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Player))
        {
            agroSeekingScriptRef.ActiveAgroSpeed = Mathf.Abs( collision.gameObject.GetComponent<Rigidbody2D>().velocity.x); 
            // Makes the enemy chase the player at half of the speed that the player entered the agro radius at 
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Player))
        {
            agroSeekingScriptRef.TargetDestination = collision.gameObject.transform.position;
        }
    }
}
