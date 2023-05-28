using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{   
    public static InventoryManager Instance {get; private set;}

    public event EventHandler OnPlayerInventoryListChanged;

    private List<Item> playerInventoryList;

    private void Awake() {
        //Create player inventory list
        playerInventoryList = new List<Item>();;

        //Instance
        if (Instance == null){  
            Instance = this;        
        } else {
            Destroy(gameObject);    
            return;                 
        }

        // do not destroy me when a new scene loads
        DontDestroyOnLoad(gameObject); 
    }

    //Add item(s) to inventory
    public void AddItem(Item item){
        //Check if item is stackable
        if (item.IsStackable()){
            bool itemAlreadyInInventory = false;
            foreach(Item inventoryItem in playerInventoryList){
                if (inventoryItem.itemType == item.itemType){
                    inventoryItem.itemAmount += item.itemAmount;
                    itemAlreadyInInventory = true;
                }
            }
            if(!itemAlreadyInInventory){
                playerInventoryList.Add(item);
            }
        
        }
        else
        {   
            //If not stackable then just add item to inventory
            playerInventoryList.Add(item);
        }
        OnPlayerInventoryListChanged?.Invoke(this, EventArgs.Empty);
    }

    //Remove item(s) to inventory
    public void RemoveItem(Item item){
        //Check if item is stackable
        if (item.IsStackable()){
            Item itemInInventrory = null;
            foreach(Item inventoryItem in playerInventoryList){
                if (inventoryItem.itemType == item.itemType){
                    inventoryItem.itemAmount -= item.itemAmount;
                    itemInInventrory = inventoryItem;
                }
            }
            if(itemInInventrory != null && itemInInventrory.itemAmount <= 0){
                playerInventoryList.Remove(itemInInventrory);
            }
        
        }
        else
        {
            //If not stackable then just remove item from inventory
            playerInventoryList.Remove(item);
        }
        OnPlayerInventoryListChanged?.Invoke(this, EventArgs.Empty);
    }

    //Get player inventory list
    public List<Item> GetPlayerInventoryList(){
        return playerInventoryList;
    }
    /// <summary>
    /// returns how many items the player has of a given type 
    /// </summary>
    /// <param name="itemType">The amount to query</param>
    /// <returns></returns>
    public int GetItemAmountForType(Item.ItemType itemType)
    {
        var itemOfType = playerInventoryList.FirstOrDefault(o => o.itemType.Equals(itemType));
        return itemOfType?.itemAmount ?? 0;
    }

    /// <summary>
    /// Removes all Items of type <see cref="itemType"/>
    /// </summary>
    /// <param name="itemType">the item type to be removed</param>
    public void RemoveItemsOfType(Item.ItemType itemType)
    {
        Item itemToDelete;
        do
        {
            itemToDelete = playerInventoryList.FirstOrDefault(o => o.itemType.Equals(itemType));
            if (itemToDelete != null)
            {
                playerInventoryList.Remove(itemToDelete);
            }
        } while (itemToDelete != null);
    }
}
