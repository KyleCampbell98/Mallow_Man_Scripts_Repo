using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Select_Sign_Trigger : MonoBehaviour
{
    // Internal Collison Control Bool
    [SerializeField] private bool canBeCollidedWith = true;

    // Info needed for UI updating
    [SerializeField] private Level_Management_SO infoAssociatedWithThisLevel;
    [SerializeField] private GameObject portalAssociatedWithThisLevel;
    public Level_Management_SO InfoAssociatedWithThisLevel { get { return infoAssociatedWithThisLevel; } }

    private void Start()
    {
      //  portalAssociatedWithThisLevel = GetComponentInChildren<Level_Select_Portal_Trigger>().gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canBeCollidedWith)
        {
            canBeCollidedWith = false;
            Event_Manager.OnLevelSelectSignTriggered(infoAssociatedWithThisLevel, portalAssociatedWithThisLevel, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((!canBeCollidedWith))
        {
            canBeCollidedWith = true;
            Event_Manager.OnLevelSelectSignTriggered(infoAssociatedWithThisLevel, portalAssociatedWithThisLevel, false);
        }
    }
}
