using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Level_UI_Manager : MonoBehaviour
{
    [SerializeField] private Level_Management_SO levelSpecificDataSO;


    // UI Management
    [Header("In Play UI Elements")]
    [SerializeField] private Canvas inPlayCanvas;
    [SerializeField] private TextMeshProUGUI timerTextBox;
    [SerializeField] private TextMeshProUGUI coinsCollectedTextBox;
    [SerializeField] private Image starCollectedInPlayIndicator;

    [Header("Level Complete UI Elements")]
    [SerializeField] private Canvas levelCompleteCanvas;
    [SerializeField] private TextMeshProUGUI levelCompleteContinuePrompt;
    [SerializeField] private TextMeshProUGUI coinsCollectedTotal;
    [SerializeField] private TextMeshProUGUI levelCompletionTimeValue;
    [SerializeField] private TextMeshProUGUI totalLevelScoreValue;
    [SerializeField] private Image starCollectedImage;
    [SerializeField] private Canvas gameOverCanvas;

    [Header("Pause Menu Elements")]
    [SerializeField] private Canvas pauseMenuCanvas;
    [SerializeField] private Canvas menuReturnConfirmationScreen;
    [SerializeField] private TextMeshProUGUI pauseMenuLevelTitle;

    [SerializeField] private float timeBetweenEndUITextFades = 0.2f;
    [Range(1, 25)] [SerializeField] private float endUIFadeInTime;

    private List<TextMeshProUGUI> endLevelUIToFadeIn = new List<TextMeshProUGUI>();
    
    
    

    private void Start()
    {
        ShowSpecificCanvas(Event_Manager.LevelScenarioState.InPlay);
      
        Event_Manager.PickupCollected += UpdateUI;
        Event_Manager.UpdateUITrigger += ShowSpecificCanvas;
        pauseMenuLevelTitle.text = levelSpecificDataSO.LevelName;
        coinsCollectedTextBox.text = coinsCollectedTextBox.text + "0";
        
        PopulateLevelEndUIList();

    }

    private void PopulateLevelEndUIList()
    {
        endLevelUIToFadeIn.Add(coinsCollectedTotal);
        endLevelUIToFadeIn.Add(levelCompletionTimeValue);
        endLevelUIToFadeIn.Add(totalLevelScoreValue);
        endLevelUIToFadeIn.Add(levelCompleteContinuePrompt);
    }

    private void Update()
    {
        timerTextBox.text = "Time: " + levelSpecificDataSO.TimeRemainingInSeconds.ToString("0");
    }

    private void ShowSpecificCanvas(Event_Manager.LevelScenarioState typeToShow)
    {
        Debug.Log("Called Show specific canvas from Level UI Manager");
        
        switch (typeToShow)
        {
            case Event_Manager.LevelScenarioState.InPlay:
                           
                inPlayCanvas.gameObject.SetActive(true);
                levelCompleteCanvas.gameObject.SetActive(false);
                gameOverCanvas.gameObject.SetActive(false);
                ResetPauseScreen();

                break;

            case Event_Manager.LevelScenarioState.Pause:
              
                if(inPlayCanvas == null) { Debug.LogWarning("PAUSE CANVAS NULL SOMEHOW"); }
                inPlayCanvas.gameObject.SetActive(false);
                levelCompleteCanvas.gameObject.SetActive(false);
                gameOverCanvas.gameObject.SetActive(false);
                pauseMenuCanvas.gameObject.SetActive(true);

                break;

            case Event_Manager.LevelScenarioState.LevelComplete:

                UpdateLevelCompleteUI();
                levelSpecificDataSO.CountdownTimerShouldRun = false;
                inPlayCanvas.gameObject.SetActive(false);
                levelCompleteCanvas.gameObject.SetActive(true);
                gameOverCanvas.gameObject.SetActive(false);
                pauseMenuCanvas.gameObject.SetActive(false);
                levelSpecificDataSO.LevelHasBeenCompleted = true;
                FadeInLevelCompleteUI(timeBetweenEndUITextFades);
                break;

            case Event_Manager.LevelScenarioState.GameOver:
                levelSpecificDataSO.CountdownTimerShouldRun = false;
                inPlayCanvas.gameObject.SetActive(false);
                levelCompleteCanvas.gameObject.SetActive(false);
                gameOverCanvas.gameObject.SetActive(true);
                pauseMenuCanvas.gameObject.SetActive(false);
                break;

            case Event_Manager.LevelScenarioState.LevelSelectLoad:
                Debug.Log("LevelSelectLoad case chosen.");

                break;
        }
    }

    private void UpdateUI(Event_Manager.collectableType collectableType)
    {

        Debug.Log("Collectable collected of type: " + collectableType.ToString());
        switch (collectableType)
        {
            case Event_Manager.collectableType.Coin:
                {
                    levelSpecificDataSO.CoinsCollected++;
                    coinsCollectedTextBox.text = "Coins: " + levelSpecificDataSO.CoinsCollected.ToString("0");
                    break;
                }
            case Event_Manager.collectableType.Star:
                {
                    if (levelSpecificDataSO.LevelStarCollected == false)
                    {
                        levelSpecificDataSO.LevelStarCollected = true;
                        Debug.Log("TriggeredMeUponCollected");
                    }
                    levelSpecificDataSO.LevelStarCollectedThisSession = true;
                    starCollectedInPlayIndicator.gameObject.SetActive(true);
                    starCollectedImage.color = Color.white;
                    break;
                }
        }
    }
    private void UpdateLevelCompleteUI()
    {
        coinsCollectedTotal.text = levelSpecificDataSO.CoinsCollected.ToString();
        totalLevelScoreValue.text = levelSpecificDataSO.OverallScore.ToString();
        levelCompletionTimeValue.text = levelSpecificDataSO.LevelCompletionTimeInSeconds.ToString("0:00");
    }

    // UI Animating Methods
    private void FadeInLevelCompleteUI(float timeBetweenTextFadeIns)
    {
        StartCoroutine(TextFadeInLoop(timeBetweenTextFadeIns));   
    }
    private IEnumerator UITextFadeIn(TextMeshProUGUI textToFadeIn, float textFadeInTime)
    {
        float timeToStartLerp = 0;
        while (timeToStartLerp < 1)
        {
            textToFadeIn.color = Color.Lerp(textToFadeIn.color, new Color(textToFadeIn.color.r, textToFadeIn.color.g, textToFadeIn.color.b, 1f), timeToStartLerp);
            timeToStartLerp += Time.deltaTime / textFadeInTime;
            yield return null;
        }
        
        
    } 

    private IEnumerator TextFadeInLoop(float timeBetweenFades)
    {
        foreach(TextMeshProUGUI UIElement in endLevelUIToFadeIn)
        {
            yield return new WaitForSeconds(timeBetweenFades);
            StartCoroutine(UITextFadeIn(UIElement, endUIFadeInTime));
        }
        levelSpecificDataSO.LevelEndCanBeProgressed = true;
    }
    private void ResetPauseScreen()
    {
        pauseMenuCanvas.gameObject.SetActive(false);
        menuReturnConfirmationScreen.gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        Event_Manager.PickupCollected -= UpdateUI;
        Event_Manager.UpdateUITrigger -= ShowSpecificCanvas;
    }
}
