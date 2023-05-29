using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerItemController : MonoBehaviour
{  



    private void OnTriggerEnter2D(Collider2D collision) {

        //Collect item - add to inventory and destroy object in world
        if (collision.GetComponent<CollectableItem>() != null && !collision.GetComponent<CollectableItem>().pickedUpAlready)
        {   //Touching collectable item

            // Access the inventory instance
            InventoryManager InventoryManager = InventoryManager.Instance;

            // Add the item to the inventory
            InventoryManager.AddItem(collision.GetComponent<CollectableItem>().GetItem());
            
            GameManager.Instance.PlayerPicksUpEnemyPart();

            collision.GetComponent<CollectableItem>().PickUp();

            // Destroy the collected item object in the world
            Destroy(collision.gameObject);
            
        }

        //Drop off enemy part to the HUB collection point
        if(collision.GetComponent<EnemyPartDropOff>() != null){
            // Access the inventory instance
            InventoryManager InventoryManager = InventoryManager.Instance;

            if(InventoryManager.GetItemAmountForType(Item.ItemType.EnemyPart) > 0)
            {
                
                //what item to remove
                Item enemyPartItem = new Item { itemType = Item.ItemType.EnemyPart, itemAmount = 1 };

                var ammountOfEnemyPartsToBeDelivered = InventoryManager.GetItemAmountForType(Item.ItemType.EnemyPart);
                //Handle enemy part drop off
                GameManager.Instance.PlayerDropsOffEnemyParts(ammountOfEnemyPartsToBeDelivered);
                //remove enemy parts from inventory
                InventoryManager.RemoveItemsOfType(Item.ItemType.EnemyPart);

            }
        }
    }
}
