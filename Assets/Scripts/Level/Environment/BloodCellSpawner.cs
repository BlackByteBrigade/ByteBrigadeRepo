using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


//  Author: Pascal

public class BloodCellSpawner : MonoBehaviour
{
    [SerializeField] int dmgFromTouching = 30;
    [SerializeField] GameObject bloodCellPrefab;
    [SerializeField] float spawnInterval = 1f;
    [SerializeField] Vector2 spawnRectDimensions;
    [SerializeField] float speed = 1f;
    [Tooltip("How far should a blood cell travel from spawn")]
    [SerializeField] float maxDistanceTravelled = 100f;
    [SerializeField] Sound ambientSound;

    
    float spawnTimer;
    List<GameObject> bloodCells;
    AudioPlayer audioPlayer;

    void Start() {

        if (TryGetComponent<AudioPlayer>(out audioPlayer)) {
            audioPlayer.AddSound(ambientSound);
            audioPlayer.Play(ambientSound);
        }
        bloodCells = new();
        spawnTimer = spawnInterval;

    }

    void Update() {
        UpdateBloodCells();
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f) {
            SpawnBloodCell();
            spawnTimer = spawnInterval;
        }
        DespawnBloodCells();
    }


    void UpdateBloodCells() {
        foreach (var bloodCell in bloodCells) {
            bloodCell.transform.position += transform.up * speed * Time.deltaTime;
        }
    }

    void SpawnBloodCell() {
        Vector3 spawnPosition = new Vector3(
            Random.Range(-spawnRectDimensions.x/2f, spawnRectDimensions.x/2f),
            Random.Range(-spawnRectDimensions.y/2f, spawnRectDimensions.y/2f),
            0f
        );

        var obj = Instantiate(bloodCellPrefab, Vector3.zero , Quaternion.identity, transform);
        obj.transform.up = transform.up;
        obj.transform.localPosition = spawnPosition;
        Enemy enemy = obj.GetComponent<Enemy>();
        enemy.DmgFromTouching = dmgFromTouching;
        bloodCells.Add(obj);
    }

    void DespawnBloodCells() {
        List<GameObject> toBeRemoved = new();

        foreach (var bloodCell in bloodCells) {
            float distance = Vector3.Distance(bloodCell.transform.position,transform.position);
            if ( distance > maxDistanceTravelled)
                toBeRemoved.Add(bloodCell);
            
        }

        toBeRemoved.ForEach( obj => {
            bloodCells.Remove(obj);
            StartCoroutine(DespawnRoutine(obj));
        });
    }

    private IEnumerator DespawnRoutine(GameObject obj)
    {
        Vector3 originalScale = obj.transform.localScale;
        float timer = 1;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            obj.transform.position += transform.up * speed * Time.deltaTime;
            obj.transform.localScale = originalScale * timer;
            yield return null;
        }
        Destroy(obj);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Vector3 size = new Vector3(spawnRectDimensions.x, spawnRectDimensions.y, 0);
        Gizmos.DrawWireCube(transform.position, size);
    }
}
