using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickSceneFunctions : MonoBehaviour
{
    [SerializeField] private SceneNameCacheSO sceneNameCache;
    [SerializeField] private Level_Management_SO[] level_Management_SOs;

    // Play Canvas Functions
   public void OnResumeClicked()
    {
        Debug.Log("Registering input");
        Event_Manager.OnEndLevelTriggered(Event_Manager.LevelScenarioState.InPlay);
           
    }

    public void QuitGame()
    {
        Debug.Log("Quititing Game....");
        Application.Quit();
    }

    //Main Menu Functions
    public void StartMenuNewGameButton()
    {
        int timesResetCalled = 0;
        foreach(Level_Management_SO level_Management_SO in level_Management_SOs)
        {
            level_Management_SO.DEBUGRESETONLY();
            Debug.Log($"Reset Level data called: {timesResetCalled} times");
            timesResetCalled++;
        }
        SceneManager.LoadScene(sceneNameCache.ReturnConstSceneName(SceneNameCacheSO.Scenes._level_01));    
    }

    public void LevelSelectButton()
    {
        SceneLoadManager.LoadNextLevel(sceneNameCache.ReturnConstSceneName(SceneNameCacheSO.Scenes._level_Select));
    }

    public void ReturnToMenuButton()
    {
        Event_Manager.OnEndLevelTriggered(Event_Manager.LevelScenarioState.InPlay);
        SceneLoadManager.LoadNextLevel(sceneNameCache.ReturnConstSceneName(SceneNameCacheSO.Scenes._menu));
       
    }

    public void RetryLevel()
    {
        SceneLoadManager.LoadNextLevel(SceneManager.GetActiveScene().name);
        
    }




}
