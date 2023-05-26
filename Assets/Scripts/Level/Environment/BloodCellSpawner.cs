using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//  Author: Pascal

public class BloodCellSpawner : MonoBehaviour
{
    [SerializeField] GameObject bloodCellPrefab;
    [SerializeField] float spawnInterval = 1f;
    [SerializeField] Vector2 spawnRectDimensions;
    [SerializeField] float despawnOffset = 2f;
    [SerializeField] float speed = 1f;

    Camera mainCamera;
    float spawnTimer;
    List<GameObject> bloodCells;

    private void Start() {
        bloodCells = new();
        mainCamera = Camera.main;
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
            bloodCell.transform.position += Vector3.up * speed * Time.deltaTime;
        }
    }

    void SpawnBloodCell() {
        Vector3 spawnPosition = new Vector3(
            Random.Range(-spawnRectDimensions.x/2f, spawnRectDimensions.x/2f),
            Random.Range(-spawnRectDimensions.y/2f, spawnRectDimensions.y/2f),
            0f
        );

        var obj = Instantiate(bloodCellPrefab, Vector3.zero , Quaternion.identity, transform);
        obj.transform.localPosition = spawnPosition;
        bloodCells.Add(obj);
    }

    void DespawnBloodCells() {
        List<GameObject> toBeRemoved = new();

        foreach (var bloodCell in bloodCells) {
            if (bloodCell.transform.position.y > mainCamera.orthographicSize + despawnOffset)
                toBeRemoved.Add(bloodCell);
            
        }

        toBeRemoved.ForEach( obj => {
            bloodCells.Remove(obj);
            Destroy(obj);
        });
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Vector3 size = new Vector3(spawnRectDimensions.x, spawnRectDimensions.y, 0);
        Gizmos.DrawWireCube(transform.position, size);
    }
}
