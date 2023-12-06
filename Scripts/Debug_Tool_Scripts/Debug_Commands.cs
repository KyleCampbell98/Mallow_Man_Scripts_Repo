using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
using System;


public class Debug_Commands : MonoBehaviour
{

    [Range(0f, 1f)][SerializeField] private float gameSpeed = 1f;

    [SerializeField] private bool debugEnabled;
   

 //   [SerializeField] private GameObject debugUI;
    [SerializeField] private Level_Management_SO debugPurposeLevelSO;

    // Update is called once per frame
    void Update()
    {
        if (debugEnabled) { 
        
          
            ReloadScene();
            Time.timeScale = gameSpeed;
            EnableDebugUI();
            if (Input.GetKeyDown(KeyCode.V))
            {
                debugPurposeLevelSO.DEBUGRESETONLY();
            }
          
        }
    }

    private void ReloadScene()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneLoadManager.LoadNextLevel(null);
        }
    }
    private void EnableDebugUI()
    {
        if (Input.GetKeyDown(KeyCode.F5) && debugEnabled)
        {
        //   bool debugState = debugUI.activeSelf ? false : transform;
          //  debugUI.SetActive(debugState);
        }
    }

    private void Start()
    {
       // debugUI.SetActive(false);
    }
    private void Awake()
    {
        LocateReferences();
    }
    private void LocateReferences()
    {
   //     debugUI = GameObject.FindGameObjectWithTag("DebugUI");
    }
}
