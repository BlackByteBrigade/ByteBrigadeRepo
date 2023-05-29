using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    public int teleporterId;
    public Transform exitPoint;
    public float distBeforeEnable = 5f;

    [Header("Destination")] public string destinationScene;
    public int destinationTeleporterId;

    public static int currentDestinationId = -1;
    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (!col.enabled && Player.instance != null && Vector2.Distance(Player.instance.transform.position, transform.position) > distBeforeEnable)
        {
            col.enabled = true;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (currentDestinationId == teleporterId && Player.instance != null)
        {
            col.enabled = false;
            Player.instance.transform.position = exitPoint.transform.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(destinationScene);
            currentDestinationId = destinationTeleporterId;
        }
    }
}