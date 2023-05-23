using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic : Enemy
{
    [SerializeField] private float moveDistance;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Animator anim;

    [SerializeField] private Transform[] path;

    private int pathIndex = 0;

    private void Start()
    {
        base.Start();
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        Vector2 currentPos = transform.position;
        Vector2 targetPos = path[pathIndex].position;
        transform.position += (Vector3)(targetPos - currentPos).normalized * moveDistance;
        transform.up = -(targetPos - currentPos).normalized;

        anim.Play("move", -1, 0f);

        yield return new WaitForSeconds(moveSpeed);

        if (Vector2.Distance(transform.position, path[pathIndex].position) < moveDistance)
        {
            pathIndex = (pathIndex + 1) % path.Length;
        }

        StartCoroutine(Move());
    }
}
