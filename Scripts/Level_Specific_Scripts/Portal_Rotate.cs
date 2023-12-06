using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Portal_Rotate : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.transform.Rotate(0, 0, 1 * rotationSpeed * Time.deltaTime);
       
    }
}
