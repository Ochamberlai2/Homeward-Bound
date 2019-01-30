using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    [SerializeField]
    private ItemDefinition itemDefinition;

    private void OnMouseUp()
    {
        InventoryManager.Instance.AddItemToInventory(itemDefinition);
        gameObject.SetActive(false);
    }
    //called when a variable is changed
    private void OnValidate()
    {
        if (itemDefinition != null)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = itemDefinition.itemSprite;
            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            collider.size = spriteRenderer.sprite.bounds.size;
        }
    }
}

