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
            
            //Play sound effect
            AudioManager.instance.PlaySfX(SoundEffects.CollectingEnemyPart);

            collision.GetComponent<CollectableItem>().PickUp();

            // Destroy the collected item object in the world
            Destroy(collision.gameObject);
            
        }

        //Drop off enemy part to the HUB collection point
        if(collision.GetComponent<EnemyPartDropOff>() != null){
            // Access the inventory instance
            InventoryManager InventoryManager = InventoryManager.Instance;

            if(InventoryManager.GetItemAmountForType(Item.ItemType.EnemyPart) > 0){
                
                //what item to remove
                Item enemyPartItem = new Item { itemType = Item.ItemType.EnemyPart, itemAmount = 1 };

                while (InventoryManager.GetItemAmountForType(Item.ItemType.EnemyPart) > 0){
                    //Add to Enemy part Drop Off
                    collision.GetComponent<EnemyPartDropOff>().DropOffEnemyPart();
                
                    //remove item from inventory
                    InventoryManager.RemoveItem(enemyPartItem);
                }

            }

            
        }
    }
}
