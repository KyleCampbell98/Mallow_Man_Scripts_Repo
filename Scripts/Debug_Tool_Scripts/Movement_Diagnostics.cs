using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Movement_Diagnostics : MonoBehaviour
{
    private Rigidbody2D playerRb;
    [SerializeField] private TextMeshProUGUI velocityText;
    [SerializeField] private TextMeshProUGUI currentJumpHeight;
    [SerializeField] private TextMeshProUGUI highestJumpReached;

    private float highestJumpPointReached;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        highestJumpPointReached = playerRb.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDiagnosticsDisplay();

    }

    private void UpdateDiagnosticsDisplay()
    {
        velocityText.text = "Horizontal Velocity: " + Mathf.Abs(playerRb.velocity.x).ToString("0.0");
        currentJumpHeight.text = "Vertical Velocity: " + playerRb.velocity.y.ToString("0.0");
        UpdateHighestJumpReachedText();
    }

    private void UpdateHighestJumpReachedText()
    {
        if(playerRb.position.y > highestJumpPointReached)
        {
            highestJumpPointReached = playerRb.position.y;
            highestJumpReached.text = "Max Heighet Jumped: " + highestJumpPointReached.ToString("0.0");
        }
    }
}
