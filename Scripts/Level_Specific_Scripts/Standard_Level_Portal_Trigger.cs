using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Standard_Level_Portal_Trigger : Level_Goal_Trigger_Behaviour
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        LevelGoalSpecificLogic(Event_Manager.LevelScenarioState.LevelComplete);
    }
}
