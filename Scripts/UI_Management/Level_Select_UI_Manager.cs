using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Level_Select_Load_Manager))]
public class Level_Select_UI_Manager : MonoBehaviour
{
    [Header("UI References")]
    [Header("Canvases")]
    [SerializeField] private Canvas levelInfoCanvas;
    [SerializeField] private Canvas defaultCanvas;

    [Header("Panels")]
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject startLevelPanel;

    [Header("Text Fields: Headers")]
    [SerializeField] private TextMeshProUGUI chooseALevelHeader;
    [SerializeField] private TextMeshProUGUI levelTitle;
    [SerializeField] private TextMeshProUGUI startPanelLevelTitle;
    [SerializeField] private TextMeshProUGUI levelLockedLabel;

    [Header("Text Fields: Panel Elements")]
    [SerializeField] private TextMeshProUGUI fastestClearTimeText;
    [SerializeField] private TextMeshProUGUI canStartLevelWithJumpPrompt;
   
    [Header("Images")]
    [SerializeField] private Image levelClearedVisibleTick;
    [SerializeField] private Image levelClearedGreyedOutTick;
    [SerializeField] private Image starCollectedVisibleTick;
    [SerializeField] private Image starCollectedGreyedOutTick;

    [Header("UI Animation Configs")]
    [Header("Lerp: Zoom Configs")]
    [Range(0.1f, 1)][SerializeField] private float panelZoomTime;
    [Range(0.1f, 0.5f)][SerializeField] private float startLevelPanelZoomDelay = 0.38f;
    [SerializeField] private float lengthOfZoomLerp = 1f;
    [SerializeField] private float portalZoomTime;
    [SerializeField] private float lengthOfPortalZoomLerp;

    [Header("Lerp: Slide Configs")]
    [SerializeField] private float headerElementSlideSpeed = 1f;
    [SerializeField] private float lengthOfHeaderSlideLerp = 1f;
    [SerializeField] private float levelStartPromptDelay = 2f;

    [Header("Sizing Vectors")]
    [SerializeField] private Vector3 requiredInfoPanelSize; // This vector and the one below use values from 0 -1 for each axis, acting as a percentage of the elements overall default scale
    [SerializeField] private Vector3 requiredStartLevelPanelSize; 
    [SerializeField] private Vector3 chooseALevelHeader_OffscreenPos;
    [SerializeField] private Vector3 chooseALevelHeader_OnscreenPos;

    [Header("Level Start References")]
    private GameObject portalToOpenOnLevelPrime;
    [SerializeField] private GameObject PortalToOpenOnLevelPrime { get { return portalToOpenOnLevelPrime; } set {
            portalToOpenOnLevelPrime = value;
                Debug.Log("Portal To Open been set"); } }

    private bool levelIsPrimedForLoad = false;

    // Delegates for method parameters
    private delegate void UIMethodToPass();
    private UIMethodToPass m_levelStartPromptDelay;
    private UIMethodToPass m_readyStartPanel;
   

    // Start is called before the first frame update
    void Start()
    {
        Event_Manager.LevelSelectSignTrigger += UpdateLevelSelectInfoPanel;
       
    }

    private void UpdateLevelSelectInfoPanel(Level_Management_SO infoAboutLevel, GameObject portalToOpen, bool showInfo) // When the player leaves the trigger, this bool will be passed as false, taking the panel off of the screen and hiding it until the next trigger
    {
        PortalToOpenOnLevelPrime = portalToOpen;

        StopAllCoroutines(); 
        if (showInfo)
        {

            if(infoAboutLevel.LevelThisSORelatesTo == SceneNameCacheSO.Scenes._menu) {
                levelInfoCanvas.gameObject.SetActive(true);
                startPanelLevelTitle.text = infoAboutLevel.LevelName;
                StartCoroutine(UIElementSlideIntoPlace(
                elementToSlide: chooseALevelHeader,
                startPos: chooseALevelHeader_OnscreenPos,
                endPos: chooseALevelHeader_OffscreenPos,
                headerElementSlideSpeed,
                lengthOfHeaderSlideLerp));
                ReadyStartPanel();
                return;
            }
            if(m_levelStartPromptDelay == null) { m_levelStartPromptDelay += ActivateLevelStartPromptDelay; }
          
            // The below code and switch statement sets up the level info panel to be shown 

            switch (infoAboutLevel.LevelHasBeenCompleted)
            {
                case true:
                    levelClearedGreyedOutTick.gameObject.SetActive(false);
                    levelClearedVisibleTick.gameObject.SetActive(true);
                    fastestClearTimeText.text = infoAboutLevel.FastestLevelCompleteTime.ToString("00.00");

                    if (infoAboutLevel.LevelStarCollected)
                    {
                        Debug.Log("Called \"Level Star Collected UI Logic\"");
                        starCollectedVisibleTick.gameObject.SetActive(true);
                        starCollectedGreyedOutTick.gameObject.SetActive(false);
                    }
                    else
                    {
                       // Debug.Log("Called \"Level Star Collected UI Logic\"");
                        starCollectedVisibleTick.gameObject.SetActive(false);
                        starCollectedGreyedOutTick.gameObject.SetActive(true);
                    }
                    break;
                case false:
                    fastestClearTimeText.text = "00.00";
                    levelClearedGreyedOutTick.gameObject.SetActive(true);
                    levelClearedVisibleTick.gameObject.SetActive(false);
                    starCollectedVisibleTick.gameObject.SetActive(false);
                    starCollectedGreyedOutTick.gameObject.SetActive(true);
            
            break;
            }
            
            levelTitle.text = infoAboutLevel.LevelName;
            startPanelLevelTitle.text = levelTitle.text;
            infoPanel.transform.localScale = Vector3.zero;
            canStartLevelWithJumpPrompt.gameObject.SetActive(false);

            levelInfoCanvas.gameObject.SetActive(true);

            StartCoroutine(UIElementSlideIntoPlace(
                elementToSlide : chooseALevelHeader, 
                startPos: chooseALevelHeader_OnscreenPos, 
                endPos: chooseALevelHeader_OffscreenPos, 
                headerElementSlideSpeed, 
                lengthOfHeaderSlideLerp));


            StartCoroutine(UIElementZoomAnimation(infoPanel, Vector3.zero, requiredInfoPanelSize, panelZoomTime, lengthOfZoomLerp, showInfo));
            if (!infoAboutLevel.LevelCanBeLoadedFromLevelSelect) { levelLockedLabel.gameObject.SetActive(true); return; }
            else {
                levelLockedLabel.gameObject.SetActive(false);
                StartCoroutine(DelayUIActivation(m_levelStartPromptDelay, levelStartPromptDelay)); 
            }
          
        }
        else if (!showInfo)
        {
            ResetUI();
            PortalToOpenOnLevelPrime = null;
        }
    }

    private IEnumerator UIElementZoomAnimation(GameObject elementToZoom, Vector3 startVector, Vector3 targetVector, float PanelZoomTime, float lengthOfLerp, bool zoomIn)
    {

      //  Debug.Log("Zoom funtion called on: " + elementToZoom.name);
        float timeToStartLerp = 0;
        if (zoomIn)
        {
            elementToZoom.SetActive(true);
            elementToZoom.transform.localScale = Vector3.zero;
  
        }
        while (timeToStartLerp < lengthOfLerp)
        {
            elementToZoom.transform.localScale = Vector3.Lerp(startVector, targetVector, timeToStartLerp);
            timeToStartLerp += Time.deltaTime / PanelZoomTime;
            yield return null;
        }
       
        if (!zoomIn)
        {
            elementToZoom.SetActive(false);
            
        }

    }

    public void PrimeForLevelStart() // What is called when the player jumps. Initially sets up the conditions to enter the level
    {
        Debug.Log("PrimeForLevelStart Called");
        Player_Movement.PlayerHasJumped -= PrimeForLevelStart;
        defaultCanvas.gameObject.SetActive(false);
       
        m_readyStartPanel += ReadyStartPanel; // This is only passed as a delegate so that the method can be passed as a parameter into my delay coroutine.
        levelIsPrimedForLoad = true;
        
        StartCoroutine(UIElementZoomAnimation(infoPanel, requiredInfoPanelSize, Vector3.zero, panelZoomTime, lengthOfZoomLerp, false));
        StartCoroutine(DelayUIActivation(m_readyStartPanel, startLevelPanelZoomDelay));
    }

    private void ReadyStartPanel() 
    {
       Debug.Log("Ready Start Panel Called");
      
        startLevelPanel.transform.localScale = Vector3.zero;
        startLevelPanel.SetActive(true);
        StartCoroutine(UIElementZoomAnimation(startLevelPanel, Vector3.zero, requiredStartLevelPanelSize, panelZoomTime, lengthOfZoomLerp, true));
        StartCoroutine(UIElementZoomAnimation(PortalToOpenOnLevelPrime, Vector3.zero, new Vector3(0.5f, 0.5f, 0.5f), portalZoomTime, lengthOfPortalZoomLerp, true));
        m_readyStartPanel -= ReadyStartPanel; 
    }

    private void ResetUI()
    {
        Debug.Log("Reset UI Called");
        levelIsPrimedForLoad = false;
        StartCoroutine(UIElementZoomAnimation(infoPanel, requiredStartLevelPanelSize, Vector3.zero, panelZoomTime, lengthOfZoomLerp, false));
        StartCoroutine(UIElementZoomAnimation(startLevelPanel, requiredStartLevelPanelSize, Vector3.zero, panelZoomTime, lengthOfZoomLerp, false));
        StartCoroutine(UIElementZoomAnimation(PortalToOpenOnLevelPrime, PortalToOpenOnLevelPrime.transform.localScale, Vector3.zero, portalZoomTime, lengthOfPortalZoomLerp, false));

        defaultCanvas.gameObject.SetActive(true);
        StartCoroutine(UIElementSlideIntoPlace(chooseALevelHeader,
        chooseALevelHeader_OffscreenPos, chooseALevelHeader_OnscreenPos,
        headerElementSlideSpeed, lengthOfHeaderSlideLerp));
    }

    //UI Animation Functions
    private IEnumerator UIElementSlideIntoPlace(TextMeshProUGUI elementToSlide, Vector3 startPos, Vector3 endPos, float slideSpeed, float lengthOfLerp)
    {
        float timeToStartLerp = 0;

        while (timeToStartLerp < lengthOfLerp)
        {
            elementToSlide.rectTransform.anchoredPosition = Vector3.Lerp(startPos, endPos, timeToStartLerp);
            timeToStartLerp += Time.deltaTime / slideSpeed;
            yield return null;
        }
    }
    private IEnumerator UIElementSlideIntoPlace(GameObject elementToSlide, Vector3 startPos, Vector3 endPos, float slideSpeed, float lengthOfLerp)
    {
        float timeToStartLerp = 0;

        while (timeToStartLerp < lengthOfLerp)
        {
            elementToSlide.transform.localScale = Vector3.Lerp(startPos, endPos, timeToStartLerp);
            timeToStartLerp += Time.deltaTime / slideSpeed;
            yield return null;
        }
    }

    private IEnumerator DelayUIActivation(UIMethodToPass methodToDelay, float secondsToWait)
    {
        yield return new WaitForSeconds(secondsToWait);
        methodToDelay?.Invoke();
    }

    private void ActivateLevelStartPromptDelay()
    {
        canStartLevelWithJumpPrompt.gameObject.SetActive(true);
        m_levelStartPromptDelay = null;
    }
}

