using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    private GameObject[,] inventoryGrid;

    [SerializeField]
    private Sprite emptySlotSprite;

    public void Start()
    {
        //subscribe to the inventory ui update event
        InventoryManager.Instance.updateUIEvent += UpdateUI;
        //get a reference to the inventory grid
        GameObject inventoryPanel = GameObject.Find("InventoryHolder");
        GridLayoutGroup gridLayout = inventoryPanel.GetComponent<GridLayoutGroup>();
        inventoryGrid = new GameObject[inventoryPanel.transform.childCount / gridLayout.constraintCount , gridLayout.constraintCount];

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

    /*
     * Called when the Update ui event fires on the inventory manager 
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
                if (inventoryMatrix[i, j] == null)
                {
                    spriteRenderer.sprite = emptySlotSprite;
                }
                else
                {
                    spriteRenderer.sprite = inventoryMatrix[i, j].uiSprite;
                }
            }
        }



    }
}
