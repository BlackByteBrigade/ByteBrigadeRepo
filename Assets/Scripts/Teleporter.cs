using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    public int teleporterId;
    public Transform exitPoint;

    [Header("Destination")] public string destinationScene;
    public int destinationTeleporterId;

    public static int currentDestinationId = -1;

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