using UnityEngine;
using UnityEngine.EventSystems;

public class WorldItem : MonoBehaviour
{
    public ItemDefinition itemDefinition;


    public static GameObject SpawnWorldItem(ItemDefinition item, GameObject itemPrefab)
    {

        GameObject worldItem = Instantiate(itemPrefab, PlayerCharacterController.PlayerCharacterTransform.position, Quaternion.identity) as GameObject;
        worldItem.GetComponent<WorldItem>().itemDefinition = item;
        worldItem.GetComponent<SpriteRenderer>().sprite = item.itemSprite;
        worldItem.name = item.itemName;
        Utils.ResetBoxCollider2DBoundsToSpriteBounds(worldItem.GetComponent<BoxCollider2D>(), item.itemSprite);

        return worldItem;
    }

    public void OnMouseDown()
    {
        /* 
        * Handle picking up the item
        */
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if(InventoryManager.Instance.AddItemToInventory(itemDefinition))
            {
                Destroy(gameObject);
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

