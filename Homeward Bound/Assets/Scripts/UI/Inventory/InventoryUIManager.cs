using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

/*
 * Class to handle events on the inventory UI
 */
public class InventoryUIManager : MonoBehaviour
{
    #region Singleton
    private static InventoryUIManager instance;

    public static InventoryUIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryUIManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(InventoryUIManager).Name;
                    instance = obj.AddComponent<InventoryUIManager>();
                }
            }
            return instance;
        }
    }
    #endregion

    private GameObject[,] inventoryGrid;

    [SerializeField]
    private Sprite emptySlotSprite;

    [Required][SerializeField]
    private GameObject inventoryButtons;

    public ItemDefinition inventorySelectedItem { get; private set; }


    public void Start()
    {
        

        //get a reference to the inventory grid
        GameObject inventoryPanel = GameObject.Find("InventoryHolder");
        GridLayoutGroup gridLayout = inventoryPanel.GetComponent<GridLayoutGroup>();
        inventoryGrid = new GameObject[inventoryPanel.transform.childCount / gridLayout.constraintCount , gridLayout.constraintCount];

        //deactivate the inventory buttons parent
        inventoryButtons.SetActive(false);

        //populate the inventory grid array
        int currChild = 0;
        for(int i = 0; i < inventoryPanel.transform.childCount / gridLayout.constraintCount; i++)
        {
            for(int j = 0; j < gridLayout.constraintCount; j++)
            {
                inventoryGrid[j, i] = inventoryPanel.transform.GetChild(currChild).gameObject;
                currChild++;
            }
        }
    }
    public void OnEnable()
    {
        //subscribe to the inventory ui update event
        InventoryManager.Instance.updateUIEvent += UpdateUI;
        //subscribe to the inventory slot clicked event
        InventorySlot.itemClicked += InventorySlotClicked;
    }
    public void OnDisable()
    {
        //subscribe to the inventory ui update event
        InventoryManager.Instance.updateUIEvent -= UpdateUI;
        //subscribe to the inventory slot clicked event
        InventorySlot.itemClicked -= InventorySlotClicked;
    }

    /*
     * Called when the Update ui event fires on the inventory manager. Loops through all items in the inventory and assigns the correct UI sprites
     * and an item to each slot
     */
    private void UpdateUI()
    {
        ItemDefinition[,] inventoryMatrix = InventoryManager.Instance.GetInventoryMatrix();

        for(int i = 0; i < inventoryMatrix.GetLength(0); i++)
        {
            for(int j = 0; j < inventoryMatrix.GetLength(1); j++)
            {
                InventorySlot slot = inventoryGrid[i, j].GetComponent<InventorySlot>();
                Image spriteRenderer = inventoryGrid[i, j].GetComponent<Image>();

                slot.containedItem = inventoryMatrix[i, j];
                spriteRenderer.sprite = ChooseUIImage(inventoryMatrix[i,j], inventoryMatrix, i, j);
            }
        }
    }
    /*
     * Returns the correct sprite for the specified item based on 
     */
    private Sprite ChooseUIImage(ItemDefinition item, ItemDefinition[,] inventoryMatrix, int xIndex, int yIndex)
    {

        if (item == null)
            return emptySlotSprite;

        if (item.SlotType == Constants.InventorySlotType.Single)
            return item.uiSprite;
        
        if(item.SlotType == Constants.InventorySlotType.Horizontal)
        {
            if (xIndex < inventoryMatrix.GetLength(0) - 1 && inventoryMatrix[xIndex + 1, yIndex] == item)
                return item.uiSprites[0];
            return item.uiSprites[1];
        }
        if (item.SlotType == Constants.InventorySlotType.Vertical)
        {
            if (yIndex < inventoryMatrix.GetLength(1) - 1 && inventoryMatrix[xIndex, yIndex + 1] == item)
                return item.uiSprites[0];
            return item.uiSprites[1];
        }

        return emptySlotSprite;
    }

    /*
     * Handles opening the inventory buttons when an inventory slot has been clicked. 
     */
    public void InventorySlotClicked(ItemDefinition selectedItem)
    {
        inventorySelectedItem = selectedItem;
        if (inventorySelectedItem == null)
        {
            CloseInventoryButtons();
        }
        else if (!inventoryButtons.activeInHierarchy)
        {
            inventoryButtons.SetActive(true);
        }
    }

    public void ItemUseSucessState(bool successState)
    {
        Debug.Log("Item use success state: " + successState);

        /*
         * In the event of an item use success, remove the item from the inventory, deselect it and deactivate the inventory buttons
         */
        if(successState)
        {
            InventoryManager.Instance.RemoveItemFromInventory(inventorySelectedItem);
            inventorySelectedItem = null;
            CloseInventoryButtons();
        }
    }
    public void CloseInventoryButtons()
    {
        inventoryButtons.SetActive(false);
    }
}
