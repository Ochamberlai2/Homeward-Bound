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
}
