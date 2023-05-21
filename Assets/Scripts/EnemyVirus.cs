using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyVirus : EnemyRanged
{
    private bool HasProjectiles;

    private List<GameObject> _projectiles;
    // Start is called before the first frame update
    void Start()
    {
        ((EnemyRanged)this).Start();
        _projectiles = new List<GameObject>();
        InitProjectiles();
    }

    // Update is called once per frame
    void Update()
    {
        ((EnemyRanged)this).Update();

        if (AlertnessLevel == Alertness.Engaged && HasProjectiles)
        {
            Fire();
        }

        if (!HasProjectiles && !IsInVulnerableState)
        {
            InitProjectiles();
        }
    }

    public void InitProjectiles()
    {
        var num = 10;
        var point = GetComponent<Renderer>().bounds.center;
        var radius = 1;
        //todo generate projectiles around the outer ring of the enemy
        for (int i = 0; i < num; i++)
        {

            /* Distance around the circle */
            var radians = 2 * Mathf.PI / num * i;

            /* Get the vector direction */
            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians);

            var spawnDir = new Vector3(horizontal, 0, vertical);

            /* Get the spawn position */
            var spawnPos = point + spawnDir * radius; // Radius is just the distance away from the point

            /* Now spawn */
            var projectile = Instantiate(Projectile, spawnPos, Quaternion.identity) as GameObject;

            /* Rotate the enemy to face towards player */
            projectile.transform.LookAt(point);

            /* Adjust height */
            projectile.transform.Translate(new Vector3(0, projectile.transform.localScale.y / 2, 0));
            _projectiles.Add(projectile);
        }

        HasProjectiles = true;
    }

    public override void Fire()
    {
        //todo shoot out the projectiles in a straight line outwards

        foreach (var projectile in _projectiles)
        {
           projectile.GetComponent<EnermyProjectile>().FireStraightLine();
        }
        HasProjectiles = false;
        _projectiles.Clear();
        BecameVulnerableNow();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Bump top!");
        ((Enemy)this).OnCollisionEnter2D(collision);
    }
}
