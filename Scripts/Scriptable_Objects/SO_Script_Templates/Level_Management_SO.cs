using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
// This scriptable object contains all data that pertains to a specific level.
/*
 * Each level will have its own scriptable object that will be configured with level specific parameters such as unique time limits,
 * and trackers on collected coins/stars
*/
public class Level_Management_SO : ScriptableObject
{
    [Header("Level Name")]
    [SerializeField] private string levelName;
    public string LevelName { get { return levelName; } }

    [Header("Level Timer Data")]
    [SerializeField] private float levelCountdownTimerStartingValue;
    [SerializeField] private float timeRemainingInSeconds;

    public float LevelCountdownTimerStartingValue { get { return levelCountdownTimerStartingValue; } private set { levelCountdownTimerStartingValue = value; } }
    public float TimeRemainingInSeconds
    {
        get { return timeRemainingInSeconds; }
        set
        {
            timeRemainingInSeconds = value;
            if (value <= 0 && CountdownTimerShouldRun)
            {
                OutOfTimeBehaviour();
            }
        }
    }

    [Header("Scene Progression Settings")]
    [SerializeField] private SceneNameCacheSO.Scenes levelThisSORelatesTo;

    [SerializeField] private SceneNameCacheSO.Scenes levelToComeNext;
    public SceneNameCacheSO.Scenes LevelThisSORelatesTo { get { return levelThisSORelatesTo; } }
    public SceneNameCacheSO.Scenes LevelToComeNext { get { return levelToComeNext; } }
    public Level_Management_SO levelToComeNextInfoObject;



    // State Bools
    [SerializeField] private bool levelEndCanBeProgressed;
    [SerializeField] private bool levelIsPaused = false;
    [SerializeField] private bool levelHasBeenCompleted = false;
    [SerializeField] private bool levelStarCollected = false;
    [SerializeField] private bool levelCanBeLoadedFromLevelSelect;

    public bool LevelEndCanBeProgressed { get { return levelEndCanBeProgressed; } set { levelEndCanBeProgressed = value; Debug.Log("Level can be progressed set to: " + value.ToString()); } } // Checks whether the prompt to "Hit any key to continue" has appeared
    public bool LevelIsPaused
    {
        get { return levelIsPaused; }
        set
        {
            levelIsPaused = value;
            if (value == true)
            {
                CountdownTimerShouldRun = false;
            }
            else if (value == false)
            {
                CountdownTimerShouldRun = true;
            }
            Debug.Log("Level Paused set to: " + value);
        }
    }
    public bool CountdownTimerShouldRun { get; set; }
    // Upon facing any issues with pausing via "Time.Timescale", go to https://gamedevbeginner.com/the-right-way-to-pause-the-game-in-unity/
    // for more information on what is and isn't affected by timescale pausing



    public bool LevelHasBeenCompleted
    {
        get { return levelHasBeenCompleted; }
        set
        {
            {
                levelHasBeenCompleted = value;

                if (value == true) { levelToComeNextInfoObject.levelCanBeLoadedFromLevelSelect = true; }
                else if (value == false) { levelToComeNextInfoObject.levelCanBeLoadedFromLevelSelect = false; }
            }
            Debug.Log("Level has been completed value set to: " + value);
        }
    }

    public bool LevelStarCollected { get { return levelStarCollected; } set { levelStarCollected = value; Debug.Log("Star Collected Value set to: " + value.ToString()); } } // Whether the star has EVER been collected whilst playing this level. Only reset by player restarting with a new game file.
    public bool LevelStarCollectedThisSession { get; set; }

    public bool LevelCanBeLoadedFromLevelSelect { get { return levelCanBeLoadedFromLevelSelect; } }

    // Scoring Info
    [SerializeField] private int coinsCollected;
    [SerializeField] private float levelCompletionTimeInSeconds = 0;
    [SerializeField] private float fastestLevelCompleteTime;
    [SerializeField] private int overallScore;
    public int CoinsCollected { get { return coinsCollected; } set { coinsCollected = value; } }

    public float LevelCompletionTimeInSeconds { get { return levelCompletionTimeInSeconds; } set { levelCompletionTimeInSeconds = value;  if (value<FastestLevelCompleteTime){ fastestLevelCompleteTime = value; } } }// Value restarts on Play 
    public float FastestLevelCompleteTime { get { return fastestLevelCompleteTime; } set { fastestLevelCompleteTime = value; } }
    public int OverallScore { get { return overallScore; } set { overallScore = value; } }

   

    
    private void OnEnable()
    {
        CoinsCollected = 0;
        LevelStarCollectedThisSession = false;     
    }

    public void LevelStartBehaviour()
    {
        TimeRemainingInSeconds = levelCountdownTimerStartingValue;
        CoinsCollected = 0;
        OverallScore = 0;
        CountdownTimerShouldRun = true;
        LevelIsPaused = false;
        LevelEndCanBeProgressed = false;
        if (!levelHasBeenCompleted)
        {
            fastestLevelCompleteTime = LevelCountdownTimerStartingValue;
        }
    }
    private void OutOfTimeBehaviour() // Exception to rule of SOs not containing methods, as having this method in the SO allows for
        // the method to be run once from within the property it is called from.
    {
      CountdownTimerShouldRun = false;
      TimeRemainingInSeconds = 0;
      Event_Manager.OnEndLevelTriggered(Event_Manager.LevelScenarioState.GameOver);
    }

    public void DEBUGRESETONLY()
    {
        Debug.Log("CALLED SO WIPE");
        LevelStarCollected = false;
        LevelHasBeenCompleted = false;
        LevelCompletionTimeInSeconds = 0;
    }
}
