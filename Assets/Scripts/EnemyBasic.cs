using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBasic : EnemyPathing
{
    [Header("Basic Enemy")]
    [SerializeField] private float moveDistance;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float driftSpeed;
    [SerializeField] private Animator anim;


    private Rigidbody2D body;

    public void Start()
    {
        base.Start();
        body = GetComponent<Rigidbody2D>();

        // starts the movement cycle
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        Vector2 currentPos = transform.position; // easier to deal with the position as a vector2
        Vector2 prevTargetPos = path[(PathIndex - 1 < 0 ? path.Length : PathIndex) - 1].position;
        Vector2 targetDir = (TargetPos - currentPos).normalized;

        // face and move towards target
        body.velocity = targetDir * driftSpeed;
        transform.position += (Vector3)targetDir * moveDistance;
        transform.up = -targetDir;

        anim.Play("move", -1, 0f); // plays move animation from the start

        yield return new WaitForSeconds(moveSpeed);

        if (Vector2.Dot(TargetPos - (Vector2)transform.position, TargetPos - prevTargetPos) < 0)
        {
            // only move on to the next point on the path if the previous point is behind us
            TargetNextPoint();
        }

        // resets the cycle
        StartCoroutine(Move());
    }
}
