
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCollection : MonoBehaviour
{  

    //Collect item - add to inventory and destroy object in world
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<CollectableItem>() != null){
            
            // Access the inventory instance
            InventoryManager InventoryManager = InventoryManager.Instance;

            // Add the item to the inventory
            InventoryManager.AddItem(collision.GetComponent<CollectableItem>().GetItem());

            // Destroy the collected item object in the world
            Destroy(collision.gameObject);
            //Touching collectable item
            //inventory.AddItem(collision.GetComponent<CollectableItem>().GetItem());
        }
    }
}
