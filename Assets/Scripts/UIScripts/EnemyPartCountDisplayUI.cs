using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyPartCountDisplayUI : MonoBehaviour
{

    private const string ENEMY_PART_TAG = "EnemyPart";
    private Item.ItemType itemBeingCollected = Item.ItemType.EnemyPart;

    [SerializeField] private TMP_Text enemyPartCounterText;

    private void Start() {
        // Count the number of enemy parts in the level
        CountNumberofEnemyPartsInLevel();

        // Refresh the enemy part counter text
        RefreshEnemyPartCounterText();

        // Subscribe to the OnPlayerInventoryListChanged event
        InventoryManager.Instance.OnPlayerInventoryListChanged += Inventory_OnPlayerInventoryListChanged;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the OnPlayerInventoryListChanged event to avoid memory leaks
        InventoryManager.Instance.OnPlayerInventoryListChanged -= Inventory_OnPlayerInventoryListChanged;
    }

    //Player inventry has changed. 
    private void Inventory_OnPlayerInventoryListChanged(object sender, System.EventArgs e){
        //Refresh Enemy part counter text in case an enemy part was picked up
        RefreshEnemyPartCounterText();
    }

    //Counts number of Enemy parts in level for enemy part counter text
    private void CountNumberofEnemyPartsInLevel(){
        GameObject[] enemyParts = GameObject.FindGameObjectsWithTag(ENEMY_PART_TAG);
        foreach (GameObject enemyPart in enemyParts){
            GameManager.Instance.totalEnemyPartsToBeCollected++;
        }
    }

    private void RefreshEnemyPartCounterText(){
        // Access the inventory instance
        InventoryManager InventoryManager = InventoryManager.Instance;

        //Update enemy part Counter text
        enemyPartCounterText.text = InventoryManager.GetItemAmountForType(itemBeingCollected) + "/" + GameManager.Instance.totalEnemyPartsToBeCollected;
    }
}
