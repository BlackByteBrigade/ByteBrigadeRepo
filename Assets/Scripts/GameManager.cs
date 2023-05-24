using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Scenes")]
    public string hubScene;

    [Header("Respawning")]
    public float respawnTime;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
