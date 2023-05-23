using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [SerializeField] Item.ItemType itemType;
    [SerializeField] int itemAmount;

    public Item GetItem(){
        return new Item {itemType = itemType, itemAmount = itemAmount};
    }
}
