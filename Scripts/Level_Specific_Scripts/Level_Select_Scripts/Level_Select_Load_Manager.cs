using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Level_Select_UI_Manager))]
[RequireComponent(typeof(SceneLoadManager))]
public class Level_Select_Load_Manager : MonoBehaviour
{
    [Header("Level Select Confirmation Cache")]
    [SerializeField] private Level_Management_SO highlightedLevelInfo;

    [Header("Script References")]
    [SerializeField] private Level_Select_UI_Manager levelSelectUIManagerScript;


    private void Start()
    {
        Event_Manager.LevelSelectSignTrigger += PrepForLevelLoad;
        levelSelectUIManagerScript = GetComponent<Level_Select_UI_Manager>();
    }

    private void PrepForLevelLoad(Level_Management_SO currentLevelView, GameObject TEMP, bool shouldCacheLevel)
    {
        if (shouldCacheLevel)
        {
            highlightedLevelInfo = currentLevelView;
            if(currentLevelView.LevelThisSORelatesTo == SceneNameCacheSO.Scenes._menu ) { return; }
            else if(currentLevelView.LevelCanBeLoadedFromLevelSelect == false) { Debug.Log("Level not yet unlocked"); return; }
            Player_Movement.PlayerHasJumped += levelSelectUIManagerScript.PrimeForLevelStart;
        }
        else if (!shouldCacheLevel)
        {
            Player_Movement.PlayerHasJumped -= levelSelectUIManagerScript.PrimeForLevelStart;
            highlightedLevelInfo = null;
        }
    }

    private void OnDestroy()
    {
        Event_Manager.LevelSelectSignTrigger -= PrepForLevelLoad;
    }

}
