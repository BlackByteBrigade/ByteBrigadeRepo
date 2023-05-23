using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic : Enemy
{
    [SerializeField] private float moveDistance;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float driftSpeed;
    [SerializeField] private float targetRadius;
    [SerializeField] private Animator anim;

    [SerializeField] private Transform[] path;

    private Rigidbody2D body;
    private Vector2 targetPos;
    private int pathIndex = 0;

    private void Start()
    {
        base.Start();
        body = GetComponent<Rigidbody2D>();
        TargetNextPoint();
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        Vector2 currentPos = transform.position;
        Vector2 prevTargetPos = path[(pathIndex - 1 < 0 ? path.Length : pathIndex) - 1].position;
        Vector2 targetDir = (targetPos - currentPos).normalized;

        body.velocity = targetDir * driftSpeed;
        transform.position += (Vector3)targetDir * moveDistance;
        transform.up = -targetDir;

        anim.Play("move", -1, 0f);

        yield return new WaitForSeconds(moveSpeed);

        if (Vector2.Dot(targetPos - (Vector2)transform.position, targetPos - prevTargetPos) < 0)
        {
            TargetNextPoint();
        }

        StartCoroutine(Move());
    }

    private void TargetNextPoint()
    {
        pathIndex = (pathIndex + 1) % path.Length;
        targetPos = (Vector2)path[pathIndex].position + Random.insideUnitCircle * targetRadius;
    }
}
