using DG.Tweening;
using System.Linq;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public int id;
    [SerializeField] Item.ItemType itemType;
    [SerializeField] int itemAmount;

    [Tooltip("only applies for dropped enemy parts - defines how far they spread from the dead body")]
    [SerializeField] float dropRadius;
    [SerializeField] float dropTime;
    [SerializeField] GameObject vfxPrefab;

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
            transform.DOMove(PickDropPosition(), dropTime);
        }
    }

    private Vector3 PickDropPosition()
    {
        Vector3 pos = transform.position;
        int tries = 30;
        for (int i = 0; i < tries; i++)
        {
            pos = transform.position + (Vector3)Random.insideUnitCircle * dropRadius;
            Collider2D[] results = new Collider2D[2];
            Physics2D.OverlapPointNonAlloc(pos, results);
            if (results.Length > 1 || (results.Length > 0 && !results[0].TryGetComponent<CollectableItem>(out _)))
            {
                return pos;
            }
        }
        return transform.position;
    }

    void FixedUpdate()
    {
        if (itemType != Item.ItemType.EnemyPart) return;
        // only continue with this logic if this is an enemy part

        if(GameManager.Instance.narrationHasSeenEnemyPart) return;
        // only continue with this logic if the player has not yet been close to any enemy part

        var playerPos = GameObject.Find("Player")?.transform.position;
        if(playerPos == null)
            return; //Player might be dead

        var distanceToPlayer = Vector2.Distance(transform.position, playerPos.Value);
        if (distanceToPlayer <= 5)
        {
            GameManager.Instance.PlayerCloseToEnemyPart();
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

        GameObject obj = Instantiate(vfxPrefab);
        obj.transform.position = transform.position;

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
