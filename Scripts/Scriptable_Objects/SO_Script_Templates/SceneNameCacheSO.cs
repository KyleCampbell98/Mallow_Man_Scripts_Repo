using Mono.Collections.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


[CreateAssetMenu]
public class SceneNameCacheSO : ScriptableObject
{
    public enum Scenes { _menu, _level_Select, _level_01, _level_02, _level_03, _level_04, _level_05 };
    private const string main_Menu = "Main_Menu_Screen";
    private const string level_Select = "Level_Select_Scene";
    private const string level_01 = "Level_01";
    private const string level_02 = "Level_02";
    private const string level_03 = "Level_03";
    private const string level_04 = "Level_04";
    private const string level_05 = "Level_05";

    public string ReturnConstSceneName(Scenes scene)
    {
        string sceneToReturnForLoad;
        switch (scene)
        {
            
            case Scenes._level_01:
                sceneToReturnForLoad = level_01;
                break;

            case Scenes._menu:
                sceneToReturnForLoad = main_Menu;
                break;

            case Scenes._level_Select:
                sceneToReturnForLoad = level_Select;
                break;
            case Scenes._level_02:
                sceneToReturnForLoad = level_02;
                break;
            case Scenes._level_03:
                sceneToReturnForLoad = level_03;
                break;
            case Scenes._level_04:
                sceneToReturnForLoad = level_04;
                break;
            case Scenes._level_05:
                sceneToReturnForLoad = level_05;
                break;

            default: sceneToReturnForLoad = main_Menu;
                Debug.LogError($"Scene: \"{scene}\" doesn't exist in the build settings. Loading Menu by default."  ); ;
                break;
        }

        return sceneToReturnForLoad;
    }

    
}
