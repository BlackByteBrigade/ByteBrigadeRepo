using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyGroup[] enemyGroups;
    [SerializeField] private float spawnRadius;
    [SerializeField] private float timeBetweenRespawns;

    private void Start()
    {
        SpawnAllEnemies();
    }

    private void SpawnAllEnemies()
    {
        // goes through each group of enemies
        foreach (EnemyGroup group in enemyGroups)
        {
            for (int i = 0; i < group.numEnemies; i++)
            {
                // spawns the enemy for each of the enemies in the group
                SpawnEnemy(group);
            }
        }
    }

    private IEnumerator RespawnEnemy(EnemyGroup group)
    {
        // delays spawn enemy by timeBetweenRespawns
        yield return new WaitForSeconds(timeBetweenRespawns);

        SpawnEnemy(group);
    }

    private void SpawnEnemy(EnemyGroup group)
    {
        Enemy enemy = Instantiate(group.enemyPrefab, transform.position + (Vector3)Random.insideUnitCircle * spawnRadius, Quaternion.identity);
        enemy.OnDeath += OnEnemyDie; // we need to know when the cell dies so we can respawn it
        group.enemies.Add(enemy); // keeps track of the newly spawned enemy

        if (enemy is EnemyPathing) // configures the path for pathing enemies
        {
            (enemy as EnemyPathing).path = group.pathRoot.GetComponentsInChildren<Transform>().Where(t => t != group.pathRoot).ToArray();
        }
    }

    private void OnEnemyDie(Cell enemy) // parameter has to be of type cell since the event is coming from the cell class
    {
        foreach (EnemyGroup group in enemyGroups)
        {
            // checks each group to see if it contains the enemy that died
            if (group.enemies.Contains(enemy as Enemy))
            {
                group.enemies.Remove(enemy as Enemy);
                StartCoroutine(RespawnEnemy(group));
            }
        }
    }
}

[Serializable]
public struct EnemyGroup
{
    public int numEnemies;
    public Enemy enemyPrefab;
    public Transform pathRoot;

    [HideInInspector] public List<Enemy> enemies;
}
