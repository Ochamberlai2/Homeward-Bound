using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Collections;

[CreateAssetMenu(fileName ="New Scenario", menuName ="Create New Scenario", order = 1)]
public class Scenario : SerializedScriptableObject
{

    [InfoBox("List of actions to be fired when the scenario is triggered.")]
    public Action[] Actions;

    [InfoBox("Dictionary of outcomes for the scenario. If the key (the item) is used, then the value (the scenario) will be triggered")]
    public Dictionary<ItemDefinition, Scenario> Outcomes;


    /*
     * Called when one of the items specified in the oucomes dictionary has been used. Will call all actions and set the scenario attributed
     * to the item as the current scenario
     */
    public void OutcomeTriggered(ItemDefinition item)
    {

        for(int i = 0; i < Actions.Length; i++)
        {
            Actions[i].Fire();
            
        }

        EventManager.Instance.CurrentScenario =  Outcomes[item];
    }
}
