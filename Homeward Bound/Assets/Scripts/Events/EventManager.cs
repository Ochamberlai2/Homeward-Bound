using UnityEngine;
using Sirenix.OdinInspector;
using System;

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
    
    public Action<ItemDefinition, Action<bool>> itemUsed;

    [Required]
    public Scenario CurrentScenario;

    public void Awake()
    {
        itemUsed += EventTriggered;
    }
    public void OnDisable()
    {
        itemUsed -= EventTriggered;
    }

    /*
     * Called when the itemUsed function is invoked. Handles 
     */
    private void EventTriggered(ItemDefinition itemDefinition, Action<bool> callback)
    {
        if (CurrentScenario == null)
            throw new Exception("CurrentScenario cannot be null");

        if(CurrentScenario.Outcomes.ContainsKey(itemDefinition))
        {
            CurrentScenario.OutcomeTriggered(itemDefinition);
            callback.Invoke(true);
        }
        else
        {
            callback.Invoke(false);
        }
    }

}
