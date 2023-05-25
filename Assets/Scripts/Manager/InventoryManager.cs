using System;
using System.Collections;
using System.Collections.Generic;
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

    // How many items do you have of a type?
    public int GetItemAmountForType(Item.ItemType itemType){
        foreach(Item inventoryItem in playerInventoryList){
            if (inventoryItem.itemType == itemType){
                return inventoryItem.itemAmount;
            }
        }
        
        int NoitemFound = 0;
        return NoitemFound;
    }
}
