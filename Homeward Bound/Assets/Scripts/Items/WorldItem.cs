using UnityEngine;
using UnityEngine.EventSystems;

public class WorldItem : MonoBehaviour
{
    [SerializeField]
    private ItemDefinition itemDefinition;

    public void OnMouseDown()
    {
        /* 
            * Handle picking up the item
            */
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            InventoryManager.Instance.AddItemToInventory(itemDefinition);
            gameObject.SetActive(false);
        }
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

