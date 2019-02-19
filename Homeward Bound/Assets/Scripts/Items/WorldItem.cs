using UnityEngine;
using UnityEngine.EventSystems;

public class WorldItem : MonoBehaviour
{
    [SerializeField]
    public ItemDefinition itemDefinition;

    public void OnMouseDown()
    {
        /* 
        * Handle picking up the item
        */
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if(InventoryManager.Instance.AddItemToInventory(itemDefinition))
            {
                gameObject.SetActive(false);
            }
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

