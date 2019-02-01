using System;
using UnityEngine.EventSystems;
using UnityEngine;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public static event Action<ItemDefinition> itemClicked;

    public ItemDefinition containedItem;

    public void OnPointerClick(PointerEventData eventData)
    { 
        itemClicked.Invoke(containedItem);
    }
}
