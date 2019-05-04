using System.Collections.Generic;
using UnityEngine;

/*
 * Class containing the button on click functions for the inventory system
 */
public class InventoryButtonEvents : MonoBehaviour
{
    [SerializeField]
    private GameObject itemPrefab;

    private ItemDefinition combineItemFirstSelectedItem = null;


    public void Start()
    {
        InventoryUIManager.Instance.SecondInventoryItemClicked += SecondInventoryItemClickedHandler;
    }

    public void OnDestroy()
    {
        InventoryUIManager.Instance.SecondInventoryItemClicked -= SecondInventoryItemClickedHandler;

    }

    /*
     * Handle the use item button click event
     */
    public void UseItem()
    {
        InventoryUIManager inventoryUIManager = InventoryUIManager.Instance;
        ItemDefinition item = inventoryUIManager.inventorySelectedItem;
        //invoke the itemUsed event
        EventManager.Instance.itemUsed.Invoke(item, inventoryUIManager.ItemUseSucessState);
    }

    /*
     * Discard item from inventory (removes the item from the inventory
     */
    public void DiscardItem()
    {
        InventoryUIManager inventoryUIManager = InventoryUIManager.Instance;
        ItemDefinition item = inventoryUIManager.inventorySelectedItem;
        //remove item from inventory
        InventoryManager.Instance.RemoveItemFromInventory(item);
        Debug.Log("Didn't need that " + item.name + " anymore anyway!");

        WorldItem.SpawnWorldItem(item, itemPrefab);

        inventoryUIManager.CloseInventoryButtons();
    }

    /*
     *Checks the inventory's currently selected item's combination list against the newly clicked on item to see if it can combine. If it can, combine, if not return an error 
     */
    public void CombineItem()
    {
        InventoryUIManager inventoryUIManager = InventoryUIManager.Instance;
        combineItemFirstSelectedItem = inventoryUIManager.inventorySelectedItem;

        InventoryManager.Instance.WaitingForCombinedItemClick = true;
    }

    /*
     * Called when the InventoryUIManager event SecondInventoryItemClicked is invoked. 
     */
    private void SecondInventoryItemClickedHandler(ItemDefinition secondItem)
    {
        if (combineItemFirstSelectedItem == null)
        {
            throw new System.Exception("Item to combine with is missing");
        }
        if (secondItem != null)
        {


            if (combineItemFirstSelectedItem.itemCombinationRecipes.ContainsKey(secondItem))
            {
                ItemDefinition combinationResult = combineItemFirstSelectedItem.itemCombinationRecipes[secondItem];

                if (InventoryManager.Instance.TestCombineResult(new List<ItemDefinition> { combineItemFirstSelectedItem, secondItem }, combinationResult))
                {
                    //remove both items from the inventory
                    InventoryManager.Instance.RemoveItemFromInventory(combineItemFirstSelectedItem);
                    InventoryManager.Instance.RemoveItemFromInventory(secondItem);

                    //add the new one
                    InventoryManager.Instance.AddItemToInventory(combinationResult);
                }
                else
                {
                    Debug.Log("Could not combine " + combineItemFirstSelectedItem.itemName + " and " + secondItem.itemName + " due to insufficient inventory space for the resultant item");
                }

                

            }
            else
            {
                Debug.Log(secondItem.itemName + " not a valid combination candidate for " + combineItemFirstSelectedItem.itemName);
            }
        }
        combineItemFirstSelectedItem = null;
        InventoryManager.Instance.WaitingForCombinedItemClick = false;
    }
}
