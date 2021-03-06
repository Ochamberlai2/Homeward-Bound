using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class to handle the back-end (non UI) side of the inventory system
 */
public class InventoryManager : SerializedMonoBehaviour
{

    #region Singleton
    private static InventoryManager instance;

    public static InventoryManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(InventoryManager).Name;
                    instance = obj.AddComponent<InventoryManager>();
                }
            }
            return instance;
        }
    }
    #endregion

    //Public fields
    [ReadOnly]
    public bool WaitingForCombinedItemClick;

    //Private fields
    [SerializeField]
    private int maxInventorySize = 4;
    [SerializeField]
    [ReadOnly]
    private int currentInventorySlotsOccupied = 0;
    [ReadOnly]
    [SerializeField]
    private List<ItemDefinition> inventoryItems;
    [SerializeField]
    [TableMatrix]
    [ReadOnly]
    private ItemDefinition[,] inventoryItemArray = new ItemDefinition[2, 2];

    //events
    public delegate void UpdateUI();
    public event UpdateUI updateUIEvent = delegate { };


    /*
     * This function will return true if the item slot type can be added to the inventory
     */
    private bool CanAddToInventory(Constants.InventorySlotType slotType)
    {
        switch (slotType)
        {
            case Constants.InventorySlotType.Single:
                return currentInventorySlotsOccupied < maxInventorySize;

            case Constants.InventorySlotType.Horizontal:
                return maxInventorySize - currentInventorySlotsOccupied >= 2;

            case Constants.InventorySlotType.Vertical:
                return maxInventorySize - currentInventorySlotsOccupied >= 2;

            default:
                return false;
        }
    }

    /*
     *  This function will add an item to the inventory. It will return false if it cannot add an item to the inventory or true if it has added the item
     */
    [Button]
    public bool AddItemToInventory(ItemDefinition itemToAdd)
    {
        if (itemToAdd == null || !CanAddToInventory(itemToAdd.SlotType))
        {
            return false;
        }

        return PlaceItemInSlot(itemToAdd);

        /*
        * If there is an open slot (e.g the slot type is horizontal and it can slot the horizontal item into the inventory without moving any items) return true. 
        */
        List<ValueTuple<int, int>> IsOpenSlot(Constants.InventorySlotType slotType)
        {

            for (int i = 0; i < inventoryItemArray.GetLength(0); i++)
            {
                for (int j = 0; j < inventoryItemArray.GetLength(1); j++)
                {
                    if (slotType == Constants.InventorySlotType.Single && inventoryItemArray[i, j] == null)
                    {
                        return new List<ValueTuple<int, int>>() { new ValueTuple<int, int>(i, j) };
                    }
                    else if (slotType == Constants.InventorySlotType.Horizontal)
                    {
                        if (i < inventoryItemArray.GetLength(0) - 1 && inventoryItemArray[i, j] == null && inventoryItemArray[i + 1, j] == null)
                        {
                            return new List<ValueTuple<int, int>>() { new ValueTuple<int, int>(i, j), new ValueTuple<int, int>(i + 1, j) };
                        }
                    }
                    else if (slotType == Constants.InventorySlotType.Vertical)
                    {
                        if (j < inventoryItemArray.GetLength(1) - 1 && inventoryItemArray[i, j] == null && inventoryItemArray[i, j + 1] == null)
                        {
                            return new List<ValueTuple<int, int>>() { new ValueTuple<int, int>(i, j), new ValueTuple<int, int>(i, j + 1) };
                        }
                    }

                }
            }
            return new List<ValueTuple<int, int>>();
        }
        /*
            * Used in the event that items need to be moved around in order to slot an item into the inventory, this will recalculate item positions
            */
        void RecalculateSlotPositions(Constants.InventorySlotType slotType)
        {
            switch (slotType)
            {
                case Constants.InventorySlotType.Horizontal:
                    for (int i = 0; i < inventoryItemArray.GetLength(0); i++)
                    {
                        if (inventoryItemArray[1, i] != null && inventoryItemArray[0, i] == null)
                        {
                            inventoryItemArray[0, i] = inventoryItemArray[1, i];
                            inventoryItemArray[1, i] = null;
                        }
                    }
                    break;

                case Constants.InventorySlotType.Vertical:
                    for (int i = 0; i < inventoryItemArray.GetLength(1); i++)
                    {
                        if (inventoryItemArray[i, 1] != null && inventoryItemArray[i, 0] == null)
                        {
                            inventoryItemArray[i, 0] = inventoryItemArray[i, 1];
                            inventoryItemArray[i, 1] = null;
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        /*
        * Will find the empty slots and will place an item into them.
        */
        bool PlaceItemInSlot(ItemDefinition item)
        {

            if (item.SlotType != Constants.InventorySlotType.Single)
            {
                RecalculateSlotPositions(item.SlotType);
            }

            List<ValueTuple<int, int>> openPositions = IsOpenSlot(item.SlotType);

            if (openPositions.Count == 0)
            {
                return false;
            }

            if (item.SlotType == Constants.InventorySlotType.Single)
            {
                inventoryItemArray[openPositions[0].Item1, openPositions[0].Item2] = item;
                currentInventorySlotsOccupied++;
                inventoryItems.Add(item);
            }
            else
            {
                inventoryItemArray[openPositions[0].Item1, openPositions[0].Item2] = item;
                inventoryItemArray[openPositions[1].Item1, openPositions[1].Item2] = item;
                currentInventorySlotsOccupied += 2;
                inventoryItems.Add(item);
            }
            updateUIEvent.Invoke();
            return true;
        }
    }

    /*
     * This function will remove an item from the inventory. It will return false if the item does not exist in the inventory, or true if it has been removed
     */
    [Button]
    public bool RemoveItemFromInventory(ItemDefinition itemToRemove)
    {
        if (!inventoryItems.Contains(itemToRemove))
            return false;

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i] == itemToRemove)
            {
                inventoryItems.RemoveAt(i);
                break;
            }
        }

        //number of slots occupied
        if (itemToRemove.SlotType == Constants.InventorySlotType.Horizontal || itemToRemove.SlotType == Constants.InventorySlotType.Vertical)
        {
            currentInventorySlotsOccupied -= 2;
        }
        else
        {
            currentInventorySlotsOccupied--;
        }

        for (int i = 0; i < inventoryItemArray.GetLength(0); i++)
        {
            for (int j = 0; j < inventoryItemArray.GetLength(1); j++)
            {
                if (inventoryItemArray[i, j] == itemToRemove)
                {
                    inventoryItemArray[i, j] = null;
                }
            }
        }
        updateUIEvent.Invoke();
        return true;
    }
    public bool TestCombineResult(List<ItemDefinition> itemsToRemove, ItemDefinition itemToAdd)
    {
        ItemDefinition[,] mockInventory = inventoryItemArray;
        List<ItemDefinition> mockInventoryItems = inventoryItems;

        /*
         * Remove the items from the inventory 
         */
        for(int item = 0; item < itemsToRemove.Count; item++)
        {
            ItemDefinition itemToRemove = itemsToRemove[item];

            for (int i = 0; i < mockInventoryItems.Count; i++)
            {
                if (mockInventoryItems[i] == itemToRemove)
                {
                    mockInventoryItems.RemoveAt(i);
                    break;
                }
            }

            //number of slots occupied
            if (itemToRemove.SlotType == Constants.InventorySlotType.Horizontal || itemToRemove.SlotType == Constants.InventorySlotType.Vertical)
            {
                currentInventorySlotsOccupied -= 2;
            }
            else
            {
                currentInventorySlotsOccupied--;
            }

            for (int i = 0; i < mockInventory.GetLength(0); i++)
            {
                for (int j = 0; j < mockInventory.GetLength(1); j++)
                {
                    if (mockInventory[i, j] == itemToRemove)
                    {
                        mockInventory[i, j] = null;
                    }
                }
            }
        }

        /*
         * Attempt to add the item to the slot
         */
        return MockAddItemToInventory(itemToAdd, mockInventoryItems, mockInventory);


    }

    private bool MockAddItemToInventory(ItemDefinition itemToAdd, List<ItemDefinition> mockInventoryItems, ItemDefinition[,] mockInventoryItemArray)
    {
        if (itemToAdd == null || !CanAddToInventory(itemToAdd.SlotType))
        {
            return false;
        }

        return PlaceItemInSlot(itemToAdd);

        /*
        * If there is an open slot (e.g the slot type is horizontal and it can slot the horizontal item into the inventory without moving any items) return true. 
        */
        List<ValueTuple<int, int>> IsOpenSlot(Constants.InventorySlotType slotType)
        {

            for (int i = 0; i < mockInventoryItemArray.GetLength(0); i++)
            {
                for (int j = 0; j < mockInventoryItemArray.GetLength(1); j++)
                {
                    if (slotType == Constants.InventorySlotType.Single && mockInventoryItemArray[i, j] == null)
                    {
                        return new List<ValueTuple<int, int>>() { new ValueTuple<int, int>(i, j) };
                    }
                    else if (slotType == Constants.InventorySlotType.Horizontal)
                    {
                        if (i < mockInventoryItemArray.GetLength(0) - 1 && mockInventoryItemArray[i, j] == null && mockInventoryItemArray[i + 1, j] == null)
                        {
                            return new List<ValueTuple<int, int>>() { new ValueTuple<int, int>(i, j), new ValueTuple<int, int>(i + 1, j) };
                        }
                    }
                    else if (slotType == Constants.InventorySlotType.Vertical)
                    {
                        if (j < mockInventoryItemArray.GetLength(1) - 1 && mockInventoryItemArray[i, j] == null && mockInventoryItemArray[i, j + 1] == null)
                        {
                            return new List<ValueTuple<int, int>>() { new ValueTuple<int, int>(i, j), new ValueTuple<int, int>(i, j + 1) };
                        }
                    }

                }
            }
            return new List<ValueTuple<int, int>>();
        }
        /*
            * Used in the event that items need to be moved around in order to slot an item into the inventory, this will recalculate item positions
            */
        void RecalculateSlotPositions(Constants.InventorySlotType slotType)
        {
            switch (slotType)
            {
                case Constants.InventorySlotType.Horizontal:
                    for (int i = 0; i < mockInventoryItemArray.GetLength(0); i++)
                    {
                        if (mockInventoryItemArray[1, i] != null && mockInventoryItemArray[0, i] == null)
                        {
                            mockInventoryItemArray[0, i] = mockInventoryItemArray[1, i];
                            mockInventoryItemArray[1, i] = null;
                        }
                    }
                    break;

                case Constants.InventorySlotType.Vertical:
                    for (int i = 0; i < mockInventoryItemArray.GetLength(1); i++)
                    {
                        if (mockInventoryItemArray[i, 1] != null && mockInventoryItemArray[i, 0] == null)
                        {
                            mockInventoryItemArray[i, 0] = mockInventoryItemArray[i, 1];
                            mockInventoryItemArray[i, 1] = null;
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        /*
        * Will find the empty slots and will place an item into them.
        */
        bool PlaceItemInSlot(ItemDefinition item)
        {

            if (item.SlotType != Constants.InventorySlotType.Single)
            {
                RecalculateSlotPositions(item.SlotType);
            }

            List<ValueTuple<int, int>> openPositions = IsOpenSlot(item.SlotType);

            if (openPositions.Count == 0)
            {
                return false;
            }

            if (item.SlotType == Constants.InventorySlotType.Single)
            {
                mockInventoryItemArray[openPositions[0].Item1, openPositions[0].Item2] = item;
                currentInventorySlotsOccupied++;
                mockInventoryItems.Add(item);
            }
            else
            {
                mockInventoryItemArray[openPositions[0].Item1, openPositions[0].Item2] = item;
                mockInventoryItemArray[openPositions[1].Item1, openPositions[1].Item2] = item;
                currentInventorySlotsOccupied += 2;
                mockInventoryItems.Add(item);
            }
            return true;
        }
    }

    public ItemDefinition[,] GetInventoryMatrix()
    {
        return inventoryItemArray;
    }

    /*
     * For debug purposes, clears out the inventory
     */
    [Button]
    void DEBUG_resetInventory()
    {
        for (int i = 0; i < inventoryItemArray.GetLength(0); i++)
        {
            for (int j = 0; j < inventoryItemArray.GetLength(1); j++)
            {
                inventoryItemArray[i, j] = null;
            }
        }
        currentInventorySlotsOccupied = 0;
        inventoryItems.Clear();
    }
}
