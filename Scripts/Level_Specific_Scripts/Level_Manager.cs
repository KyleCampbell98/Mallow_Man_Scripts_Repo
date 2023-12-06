using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;

public class Level_Manager : MonoBehaviour
{
    [SerializeField] private Level_Management_SO levelSpecificDataSO;
   
    // Scriptable object references
    [SerializeField] private SceneNameCacheSO sceneNameCacheSO;
    private bool canBePaused;
    void Start()
    {
        canBePaused = true;
        Time.timeScale = 1.0f;
        levelSpecificDataSO.LevelStartBehaviour();
        Event_Manager.EndLevelTrigger += LevelEndScenario;
        Debug.LogWarning("Current Assigned level data object is: " + levelSpecificDataSO.name);
      
    }

      
    void Update()
    {
        CountdownTimer();
        CheckForPause();
        CheckForLevelEndInteraction();
       
    }    
    
    // End Of Level Funtions

    private void CheckForLevelEndInteraction()
    {
        if (!levelSpecificDataSO.LevelEndCanBeProgressed) { return; }
        if (Input.anyKeyDown)
        {
           Event_Manager.OnSceneLoadNeeded(sceneNameCacheSO.ReturnConstSceneName(levelSpecificDataSO.LevelToComeNext));
        }
    }
    private void LevelEndScenario(Event_Manager.LevelScenarioState currentGameState)
    {
        switch (currentGameState)
        {
            case Event_Manager.LevelScenarioState.Pause:
                HandlePause(currentGameState);
                break;

            case Event_Manager.LevelScenarioState.InPlay:
                HandlePause(currentGameState);
                break;

            case Event_Manager.LevelScenarioState.GameOver:
                canBePaused = false;
                break;

            case Event_Manager.LevelScenarioState.LevelComplete:
                canBePaused = false;
                levelSpecificDataSO.OverallScore = CalculateLevelScore();
                if (levelSpecificDataSO.LevelHasBeenCompleted == false)
                {
                    levelSpecificDataSO.LevelHasBeenCompleted = true;
                }
                break;

            case Event_Manager.LevelScenarioState.LevelSelectLoad:
                canBePaused = false;
                break;
        }
        
    }
    

    // Internal Script Logic Functions
   

    private int CalculateLevelScore()
    {
        int levelScore = 0;
        if (levelSpecificDataSO.LevelHasBeenCompleted) { levelScore = 100; }
        levelSpecificDataSO.LevelCompletionTimeInSeconds = levelSpecificDataSO.LevelCountdownTimerStartingValue - Mathf.Floor(levelSpecificDataSO.TimeRemainingInSeconds);
       
        switch (levelSpecificDataSO.LevelStarCollectedThisSession)
        {
            case true:
                levelScore += Mathf.RoundToInt((levelSpecificDataSO.CoinsCollected * 0.75f) * levelSpecificDataSO.LevelCompletionTimeInSeconds * 10);
               
                break;

            case false:
                levelScore += Mathf.RoundToInt((levelSpecificDataSO.CoinsCollected * 0.5f) * levelSpecificDataSO.LevelCompletionTimeInSeconds * 10);

                break;
        }
        return levelScore;
    }
    private void CountdownTimer()
    {
        if (levelSpecificDataSO.CountdownTimerShouldRun)
        {
            levelSpecificDataSO.TimeRemainingInSeconds -= Time.deltaTime * 1;
           
        }
    }

    private void CheckForPause()
    {
        if (Input.GetButtonDown("Start") &&  canBePaused)// Corresponds to "Pause" on controller, and "Esc" key on pc.
        {
            if (levelSpecificDataSO.LevelIsPaused)
            {
                
                Event_Manager.OnEndLevelTriggered(Event_Manager.LevelScenarioState.InPlay);

            }
            else if (!levelSpecificDataSO.LevelIsPaused)
            {
               
                Event_Manager.OnEndLevelTriggered(Event_Manager.LevelScenarioState.Pause);
            }
        }
    }

    private void HandlePause(Event_Manager.LevelScenarioState currentGameState)
    {
        switch (currentGameState)
        {
          
            case Event_Manager.LevelScenarioState.Pause:
                levelSpecificDataSO.LevelIsPaused = true;
                Time.timeScale = 0f;
                break;
            default:
                levelSpecificDataSO.LevelIsPaused = false;
                Time.timeScale = 1f;
                break;
        }
    }

    private void OnDisable()
    {
        Event_Manager.EndLevelTrigger -= LevelEndScenario;
    }



}
