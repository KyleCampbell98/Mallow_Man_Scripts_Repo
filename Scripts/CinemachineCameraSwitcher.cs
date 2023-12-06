using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineCameraSwitcher : MonoBehaviour
{
    private Animator cinemachineAnimator;
    [SerializeField] private CinemachineVirtualCamera playerCam;

    // Start is called before the first frame update
    void Start()
    {
        Event_Manager.EndLevelTrigger += SwitchCurrentCamera;
        cinemachineAnimator = GetComponent<Animator>();
        playerCam = GetComponentInChildren<CinemachineVirtualCamera>();
    }

  private void SwitchCurrentCamera(Event_Manager.LevelScenarioState currentLevelState)
    {
        if(currentLevelState == Event_Manager.LevelScenarioState.LevelComplete)
        {

            
            cinemachineAnimator.Play("ZoomedOut_PortalCamState");
            StartCoroutine(PortalCamZoomIn(2));
  
        }
    }

    private IEnumerator PortalCamZoomIn(float timeToWaitForZoom)
    {
        yield return new WaitForSeconds(timeToWaitForZoom);
        cinemachineAnimator.Play("PortalCamState");
    }

    private void OnDisable()
    {
        Event_Manager.EndLevelTrigger -= SwitchCurrentCamera;
    }
}
