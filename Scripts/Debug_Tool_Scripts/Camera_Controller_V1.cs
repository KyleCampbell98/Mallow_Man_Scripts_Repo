using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject followTarget;

    [Header("Camera Configs")]
    [SerializeField] private float camOrthographicSize;
    [SerializeField] private int camMinSize = 2;
    [SerializeField] private int camMaxSize = 10;
    private const float defaultCamSize = 5;

    void Start()
    {
        camOrthographicSize = defaultCamSize;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeCameraSize();
        Camera.main.transform.position = new Vector3(followTarget.transform.position.x, 0, -10);
    }

    private void ChangeCameraSize()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && camOrthographicSize < camMaxSize)
        {
            camOrthographicSize++;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && camOrthographicSize > camMinSize)
        {
            camOrthographicSize--;
        }
        Camera.main.orthographicSize = camOrthographicSize;
    }
}
