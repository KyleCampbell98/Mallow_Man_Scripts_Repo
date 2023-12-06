using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;

using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoadManager : MonoBehaviour
{

    private void Start()
    {
        Event_Manager.LoadNewScene += LoadNextLevel;
    }

    public static void LoadNextLevel(string sceneToLoad)
    {
        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogError("Attempted to load scene with a null string reference. The same scene has been reloaded as a failsafe.");
            Event_Manager.UnsubscribeAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
           
            return;
        }

        Event_Manager.UnsubscribeAll();
        
        SceneManager.LoadScene(sceneToLoad);
        
    }

    private void OnDestroy()
    {
        Event_Manager.LoadNewScene -= LoadNextLevel;
    }
}
