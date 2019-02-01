using UnityEngine;

/*
 * Class containing the button on click functions for the inventory system
 */
public class InventoryButtonEvents : MonoBehaviour
{
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
        
        inventoryUIManager.CloseInventoryButtons();
    }

}
