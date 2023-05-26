using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public DNAUpgrade currentDNAUpgrade { get; set; }

    [Header("Scenes")]
    public string hubScene;

    [Header("Respawning")]
    public float respawnTime;
    public CollectableItem enemyPartDropPrefab;

    private string bodyScene;
    [HideInInspector] public List<EnemyPartDrop> enemyPartsOnBody = new List<EnemyPartDrop>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        currentDNAUpgrade = null;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLoadScene;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLoadScene;
    }

    private void OnLoadScene(Scene arg0, LoadSceneMode arg1)
    {
        if (SceneManager.GetActiveScene().name == bodyScene)
        {
            SpawnDroppedEnemyParts();
        }
    }

    public void RespawnPlayer()
    {
        int count = InventoryManager.Instance.GetItemAmountForType(Item.ItemType.EnemyPart);
        InventoryManager.Instance.RemoveItem(new Item() { itemType = Item.ItemType.EnemyPart, itemAmount = count });

        bodyScene = SceneManager.GetActiveScene().name;
        int offset = enemyPartsOnBody.Count == 0 ? 0 : enemyPartsOnBody[enemyPartsOnBody.Count - 1].id; // make sure we dont have duplicate ids
        for (int i = 0; i < count; i++)
        {
            enemyPartsOnBody.Add(new EnemyPartDrop() { id = offset + i, location = Player.instance.transform.position });
        }

        SpawnDroppedEnemyParts();

        Invoke(nameof(LoadHubScene), respawnTime);
    }

    private void LoadHubScene()
    {
        SceneManager.LoadScene(hubScene);
    }

    private void SpawnDroppedEnemyParts()
    {
        foreach (EnemyPartDrop drop in enemyPartsOnBody)
        {
            CollectableItem item = Instantiate(enemyPartDropPrefab, drop.location, Quaternion.identity);
            item.id = drop.id;
            item.dropped = true;
        }
    }
}

public struct EnemyPartDrop
{
    public int id;
    public Vector2 location;
}
