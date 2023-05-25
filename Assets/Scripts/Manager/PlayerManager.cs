using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public DNAUpgrade currentDNAUpgrade { get; set; }

    [Header("Scenes")]
    public string hubScene;

    [Header("Respawning")]
    public float respawnTime;

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

    public void RespawnPlayer()
    {
        Invoke(nameof(LoadHubScene), respawnTime);
    }

    private void LoadHubScene()
    {
        SceneManager.LoadScene(hubScene);
    }
}
