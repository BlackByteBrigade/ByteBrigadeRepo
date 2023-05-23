using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class EnemyVirus : EnemyRanged
{
    private bool HasProjectiles;

    private List<GameObject> _projectiles;
    private int _numberOfprojectiles = 12;

    private Transform _sourrounderParentTransform;

    // Start is called before the first frame update
    void Start()
    {
        _sourrounderParentTransform = this.transform;
        ((EnemyRanged)this).Start();
        _projectiles = new List<GameObject>();
        InitProjectiles();
    }

    // Update is called once per frame
    void Update()
    {
        ((EnemyRanged)this).Update();

        if (AlertnessLevel == Alertness.Engaged && HasProjectiles && AttackCoolDowned)
        {
            Fire();
        }

        if (!HasProjectiles && !IsInVulnerableState)
        {
            InitProjectiles();
        }
    }
    public static double ToRadians(double val)
    {
        return (Math.PI / 180) * val;
    }
    IEnumerator SurroundDelayed()
    {
        var angleStep = 360f / _numberOfprojectiles;
        var angleSteps = new List<float>();
        for (var i = 0; i < _numberOfprojectiles; i++)
        {
            angleSteps.Add(angleStep * i);
        }

        var point = GetComponent<Renderer>().bounds.center;
        for (var i = 0; i < _numberOfprojectiles; i++)
        {
            var index = Random.Range(0, angleSteps.Count - 1);
            var angle = angleSteps[index];
            angleSteps.RemoveAt(index);

            var radius = 1.8f;
            var angle1 = Math.Cos(ToRadians(angle + i));
            var angle2 = Math.Sin(ToRadians(angle + i));
            var pointx = radius * angle1 + transform.position.x;
            var pointy = radius * angle2 + transform.position.y;
            var projposition = new Vector3((float)(pointx), (float)(pointy), 0);
            var rot = Quaternion.FromToRotation(Vector3.forward, point);
            //var projectile = Instantiate(Projectile, projposition, Quaternion.identity);
            var projectile = Instantiate(Projectile, projposition, rot);

            projectile.transform.SetParent(_sourrounderParentTransform);
            projectile.transform.up = -(transform.position - projectile.transform.position).normalized;
            _projectiles.Add(projectile);
            HasProjectiles = true;
        
            yield return new WaitForSeconds(0.12f);
        }
    }

    public void InitProjectiles()
    {
        StartCoroutine(SurroundDelayed());
    }
   

    public new void Fire()
    {
        foreach (var projectile in _projectiles)
        {
            projectile.GetComponent<EnermyProjectile>().FireStraightLine();
            projectile.transform.parent = null;
        }

        HasProjectiles = false;
        _projectiles.Clear();
        BecameVulnerableNow();
        base.Fire();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log($"Bump top!");
        ((Enemy)this).OnCollisionEnter2D(collision);
    }
}
