using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class CollectableItem : MonoBehaviour
{
    public int id;
    [SerializeField] Item.ItemType itemType;
    [SerializeField] int itemAmount;

    [Tooltip("only applies for dropped enemy parts - defines how far they spread from the dead body")]
    [SerializeField] float dropSpeed;

    [HideInInspector] public bool dropped = false;
    [HideInInspector] public bool pickedUpAlready = false;

    private void Start()
    {
        if (!dropped && itemType == Item.ItemType.EnemyPart && GameManager.Instance.collectedEnemyParts.Contains(id))
        {
            // if this enemy part was already collected, destroy this
            Destroy(gameObject);
        }
        else if (dropped && itemType == Item.ItemType.EnemyPart)
        {
            // if we are a dropped enemy part, we need to spread out so we dont clump at the same position
            GetComponent<Rigidbody2D>().velocity = Random.insideUnitCircle * dropSpeed;
        }
    }

    public Item GetItem(){
        return new Item {itemType = itemType, itemAmount = itemAmount};
    }

    public void PickUp()
    {
        pickedUpAlready = true;

        if (itemType != Item.ItemType.EnemyPart) return;
        // only continue with this logic if this is an enemy part

        if (dropped)
        {
            // if this was dropped by the player, then now that we picked it up it needs to be removed from the list of parts dropped
            PlayerManager.Instance.enemyPartsOnBody.Remove(PlayerManager.Instance.enemyPartsOnBody.Where(part => part.id == id).FirstOrDefault());
        }
        else
        {
            // just make sure we know we collected this enemy part so we can keep it gone when we reload this scene
            GameManager.Instance.collectedEnemyParts.Add(id);
        }
    }
}
