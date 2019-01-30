using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    #region Singleton
    private static EventManager instance;

    public static EventManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EventManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(EventManager).Name;
                    instance = obj.AddComponent<EventManager>();
                }
            }
            return instance;
        }
    }
    #endregion


    public Scenario CurrentScenario;

    public void Awake()
    {
        
    }

    public void EventTriggered(ItemDefinition itemDefinition)
    {
        if (CurrentScenario == null)
            throw new System.Exception("CurrentScenario cannot be null");

        if(CurrentScenario.Outcomes.ContainsKey(itemDefinition))
        {
            CurrentScenario.OutcomeTriggered(itemDefinition);
        }

    }

}
