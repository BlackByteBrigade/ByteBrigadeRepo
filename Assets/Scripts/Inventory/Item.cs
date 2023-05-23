using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType{
        EnemyPart
    }

    public ItemType itemType;
    
    public int itemAmount;

    public bool IsStackable(){
        switch (itemType) {
            default:
            case ItemType.EnemyPart:
                return true;
        }
    }
}
