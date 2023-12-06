using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


public class Standard_Preset_Patrol : Enemy_Movement
{

    [Header("Patrol Area Configs")]
    [SerializeField] private float patrolAreaWidth = 5f;
    [SerializeField] private float patrolAreaHeight = 1f;

    [SerializeField] private BoxCollider2D patrolAreaCollider;
  
    private void Start()
    {
        LocateComponentReferences(); // Overrides Base Class
        patrolAreaCollider.size = new Vector2(patrolAreaWidth, patrolAreaHeight);
        base.EnemyMover(enemySpeed);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!detectionCollider.IsTouching(patrolAreaCollider))
        {
            StartCoroutine(EdgeReached());
        }
    }
    protected override void LocateComponentReferences()
    {
        patrolAreaCollider = transform.parent.GetComponent<BoxCollider2D>();
        if (patrolAreaCollider == null)
        {
            Debug.LogError("Side_Mover Enemy is missing a box collider in its parent object. ");
        }
        base.LocateComponentReferences();
    }


}
