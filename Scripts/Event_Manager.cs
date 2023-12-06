using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public static class Event_Manager 
{

    // Static Enums
    public enum collectableType { Coin, Star }
    public enum LevelScenarioState { InPlay, LevelComplete, GameOver, Pause, LevelSelectLoad }

    // Events
    public static event UnityAction<collectableType> PickupCollected; // This event will be subscribed to by other script methods that have a void return type and need to react to a coin being collected.
    public static event UnityAction<LevelScenarioState> EndLevelTrigger;
    public static event UnityAction<string> LoadNewScene; 
    public static event UnityAction<Level_Management_SO, GameObject, bool> LevelSelectSignTrigger;
    public static event UnityAction<LevelScenarioState> UpdateUITrigger;

    public static void OnLevelSelectSignTriggered(Level_Management_SO specificLevelInfo, GameObject portalToOpenForLevelStart = null, bool shouldShowPanel = true) // When a sign is walked over, the sign's script will pass in a level SO to this method, which will pass info aboutr the level to the level select screen UI
    {
        LevelSelectSignTrigger?.Invoke(specificLevelInfo, portalToOpenForLevelStart, shouldShowPanel);
    }

    public static void OnPickupCollected(collectableType type)
    {

        PickupCollected?.Invoke(type);
    } // This method will be called from other classes as UnityActions can not be directly called from other scripts.

    public static void OnEndLevelTriggered(LevelScenarioState currentGameState)
    {
        EndLevelTrigger?.Invoke(currentGameState);
        UpdateUITrigger?.Invoke(currentGameState); // Event is used to update UI in separate event, so that UI is only updated once all Level SO values have been set.
        //This way, the updated UI can only work with the most recent value updates, and not have the chance of updating the UI before the values have been updated, thus using the old values.
    }

    public static void OnSceneLoadNeeded(string sceneToBeLoaded)
    {

        LoadNewScene?.Invoke(sceneToBeLoaded);
    } // This can then be called from a master scene loader class.

    public static void UnsubscribeAll()
    {
        Debug.Log("UNSUB ALL CALLED");
       PickupCollected = null;
       EndLevelTrigger = null;
       LoadNewScene = null;
       LevelSelectSignTrigger = null;
    } // To be used on the loading of a new scene to prevent memory leakage. Unsubscribes all subscribers from events. 
}
