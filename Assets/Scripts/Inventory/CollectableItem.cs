using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public int id;
    [SerializeField] Item.ItemType itemType;
    [SerializeField] int itemAmount;

    [HideInInspector] public bool dropped = false;

    private void Start()
    {
        if (!dropped && itemType == Item.ItemType.EnemyPart && GameManager.Instance.collectedEnemyParts.Contains(id))
        {
            Destroy(gameObject);
        }
    }

    public Item GetItem(){
        return new Item {itemType = itemType, itemAmount = itemAmount};
    }

    public void PickUp()
    {
        if (itemType != Item.ItemType.EnemyPart) return;

        if (dropped)
        {
            PlayerManager.Instance.enemyPartsOnBody.Remove(PlayerManager.Instance.enemyPartsOnBody.Where(part => part.id == id).FirstOrDefault());
        }
        else
        {
            GameManager.Instance.collectedEnemyParts.Add(id);
        }
    }
}
