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
    IEnumerator Sorround()
    {
        var angleStep = 360f / _numberOfprojectiles;
        var angleSteps = new List<float>();
        for (var i = 0; i < _numberOfprojectiles; i++)
        {
            angleSteps.Add(angleStep * i);
        }

        //var rand = new Random(DateTime.Now.Millisecond);
        var point = GetComponent<Renderer>().bounds.center;
        //Projectile.transform.SetParent(SourrounderParentTransform);
        for (var i = 0; i < _numberOfprojectiles; i++)
        {
          

            //var projectile = Instantiate(Projectile);
            var index = Random.Range(0, angleSteps.Count - 1);
            var angle = angleSteps[index];
            angleSteps.RemoveAt(index);


            ///
            /// another attmpt....
            var radius = 2.2f;
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
            /////


            //projectile.transform.RotateAround(transform.position, Vector3.up, angle);
            //projectile.transform.SetParent(_sourrounderParentTransform);

            //projectile.transform.LookAt(projectile.transform.position - _sourrounderParentTransform.position);
            //projectile.transform.LookAt(transform.position);

            //_projectiles.Add(projectile);
            //HasProjectiles = true;
            //projectile.transform.RotateAround(transform.position, Vector2.zero, angle);


            yield return new WaitForSeconds(0.12f);
        }
    }

    public void InitProjectiles()
    {
        StartCoroutine(Sorround());
        return;
        var num = 15;
        var point = GetComponent<Renderer>().bounds.center;
        var radius = 1.2f;

        Vector3 center = transform.position;
       
        //todo generate projectiles around the outer ring of the enemy
        for (int i = 0; i < num; i++)
        {

            /* Distance around the circle */
            var radians = 2 * Mathf.PI / num * i;

            /* Get the vector direction */
            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians);

            var spawnDir = new Vector3(horizontal, 0, vertical);
            int ang = i * 30;
            spawnDir.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
            spawnDir.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
            spawnDir.z = center.z;
           
            /* Get the spawn position */
            var spawnPos = point + spawnDir * radius; // Radius is just the distance away from the point

            /* Now spawn */
            var projectile = Instantiate(Projectile, spawnPos, Quaternion.identity) as GameObject;
            projectile.transform.SetParent(_sourrounderParentTransform);
            //projectile.transform.rotation= projectile.transform.position - _sourrounderParentTransform.position;
            /* Rotate the enemy to face towards player */
            Vector3 awayDirection = this.transform.position - center;
            awayDirection.z = 0;
            projectile.transform.right = awayDirection.normalized;

            //projectile.transform.LookAt(center);

            /* Adjust height */
            projectile.transform.Translate(new Vector3(0, projectile.transform.localScale.y / 2, 0));
            _projectiles.Add(projectile);
        }

        HasProjectiles = true;
    }
   

    public new void Fire()
    {
        //todo shoot out the projectiles in a straight line outwards

        //foreach (var projectile in _projectiles)
        //{
        //   projectile.GetComponent<EnermyProjectile>().FireStraightLine();
        //   projectile.transform.parent = null;
        //}

        foreach (var projectile in gameObject.GetComponentsInChildren<EnermyProjectile>())
        {
            projectile.transform.parent = null;
            projectile.FireStraightLine();
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
