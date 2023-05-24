
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerItemController : MonoBehaviour
{  

    private void OnTriggerEnter2D(Collider2D collision) {

        //Collect item - add to inventory and destroy object in world
        if (collision.GetComponent<CollectableItem>() != null){   //Touching collectable item
            
            // Access the inventory instance
            InventoryManager InventoryManager = InventoryManager.Instance;

            // Add the item to the inventory
            InventoryManager.AddItem(collision.GetComponent<CollectableItem>().GetItem());
            
            //Play sound effect
            AudioManager.instance.PlaySfX(SoundEffects.CollectingEnemyPart);

            // Destroy the collected item object in the world
            Destroy(collision.gameObject);
            
        }

        //Drop off enemy part - 
        if(collision.GetComponent<EnemyPartDropOff>() != null){
            
            // Access the inventory instance
            InventoryManager InventoryManager = InventoryManager.Instance;
        }
    }
}
