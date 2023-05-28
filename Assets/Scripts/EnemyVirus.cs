using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class EnemyVirus : EnemyRanged
{
    [Header("Projectiles")] [SerializeField]
    int _numberOfprojectiles = 12;

    [SerializeField] float radiusOffset = 1.8f;
    [SerializeField] float projectileSize = 1f;

    [Header("Visuals")] [SerializeField] SpriteRenderer eyeRenderer;
    [SerializeField] Sprite exposedCoreSprite;


    private bool _hasProjectiles;
    private bool _hasAllProjectiles;
    private List<GameObject> _projectiles;
    private Transform _sourrounderParentTransform;
    private Sprite coreSprite;
    private SpriteRenderer renderer;


    void Start()
    {
        _sourrounderParentTransform = this.transform;
        renderer = GetComponent<SpriteRenderer>();
        coreSprite = renderer.sprite;

        base.Start();
        _projectiles = new List<GameObject>();
    }


    void Update()
    {
        ((EnemyRanged)this).Update();

        if (AlertnessLevel == Alertness.Engaged && _hasProjectiles && _hasAllProjectiles && AttackCoolDowned)
        {
            Fire();
        }

    }

    void FixedUpdate()
    {
        if (!_hasProjectiles && !IsInVulnerableState)
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
        if (!_hasProjectiles && !_projectiles.Any())
        {
            _hasProjectiles = true;
            var angleStep = 360f / _numberOfprojectiles;
            var angleSteps = new List<float>();
            for (var i = 0; i < _numberOfprojectiles; i++)
            {
                angleSteps.Add(angleStep * i);
            }

            float scaleX = transform.localScale.x;
            float scaleY = transform.localScale.y;

            // var point = GetComponent<Renderer>().bounds.center;
            var point = renderer.bounds.center;
            UpdateSprite();

            for (var i = 0; i < _numberOfprojectiles; i++)
            {
                var index = Random.Range(0, angleSteps.Count);
                var angle = angleSteps[index];
                angleSteps.RemoveAt(index);

                var angle1 = Math.Cos(ToRadians(angle + i));
                var angle2 = Math.Sin(ToRadians(angle + i));
                var pointx = radiusOffset * angle1 * scaleX + transform.position.x;
                var pointy = radiusOffset * angle2 * scaleY + transform.position.y;
                var projposition = new Vector3((float)(pointx), (float)(pointy), 0);
                var rot = Quaternion.FromToRotation(Vector3.forward, point);
                var projectile = Instantiate(Projectile, projposition, rot);


                projectile.transform.SetParent(_sourrounderParentTransform);
                projectile.transform.localScale = Vector3.one * projectileSize;
                projectile.transform.up = -(transform.position - projectile.transform.position).normalized;
                _projectiles.Add(projectile);


                yield return new WaitForSeconds(0.12f);
            }

            _hasAllProjectiles = true;
        }
    }

    public void InitProjectiles()
    {
        //Debug.Log($"InitProjectiles {_hasProjectiles}");
        //Debug.Log($"number of {_projectiles.Count}");

        if (_hasProjectiles) 
            return;

        StartCoroutine(SurroundDelayed());
    }


    public new void Fire()
    {
        //Debug.Log($"Fire: {_projectiles.Count}");
        var firedProjectiles = new List<GameObject>();
        foreach (var projectile in _projectiles)
        {
            projectile.GetComponent<EnermyProjectile>().FireStraightLine();
            projectile.transform.parent = null;
            firedProjectiles.Add(projectile);
        }
        foreach (var firedProjectile in firedProjectiles)
        {
            _projectiles.Remove(firedProjectile);
        }

        BecameVulnerableNow();

        _hasAllProjectiles = false;
        _hasProjectiles = false;
        //_projectiles.Clear();
        base.Fire();
        UpdateSprite();
    }

    void UpdateSprite()
    {
        renderer.sprite = _hasProjectiles ? coreSprite : exposedCoreSprite;
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log($"Bump top!");
        ((Enemy)this).OnCollisionEnter2D(collision);
    }
}