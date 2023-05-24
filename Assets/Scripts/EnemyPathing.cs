using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : Enemy
{
    [Header("Pathing")]
    public Transform[] path;
    [SerializeField] private float targetRadius;

    public Vector2 TargetPos { get; set; } // the next position we are moving to
    public int PathIndex { get; set; }

    public void Start()
    {
        base.Start();
        PathIndex = -1;
        TargetNextPoint();
    }

    public void TargetNextPoint()
    {
        PathIndex = (PathIndex + 1) % path.Length;
        TargetPos = (Vector2)path[PathIndex].position + Random.insideUnitCircle * targetRadius;
    }
}
