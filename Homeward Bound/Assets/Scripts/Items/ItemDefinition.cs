using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

/*
 * Represents any items that may live in the game world
 */
 [CreateAssetMenu(fileName = "New Item", menuName = "Create New Item", order = 3)]
public class ItemDefinition : SerializedScriptableObject
{

    public Sprite itemSprite;
    [HideIf("ShowSingleSprite")]
    public Sprite uiSprite;


    public Constants.InventorySlotType SlotType;
    
    [HideIf("SlotType", Constants.InventorySlotType.Single)]
    [InfoBox("For horizontal, the first sprite will be the left slot, for vertical the first sprite will be the top slot")]
    public Sprite[] uiSprites;

    public Dictionary<ItemDefinition, ItemDefinition> itemCombinationRecipes;

    /*
     * Helper function for odin inspector, for serializing the item sprite variable
     */ 
    private bool ShowSingleSprite()
    {
        return SlotType != Constants.InventorySlotType.Single;
    }
}
