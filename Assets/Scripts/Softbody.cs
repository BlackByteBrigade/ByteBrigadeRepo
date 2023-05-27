using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Softbody : MonoBehaviour
{
    public float radius = 0.5f;
    public LayerMask collisionLayer;

    [Header("Softbody Settings")]
    public float flex = 1;
    public float rigidness = 1;
    public float bounce = 0.2f;
    public float stretch = 1f;

    [SerializeField] private List<Rigidbody2D> bones;
    private Rigidbody2D body;

    private List<Vector3> bonePositions = new List<Vector3>();

    private const float TOO_FAR = 5f;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        foreach (Rigidbody2D bone in bones)
        {
            bonePositions.Add(bone.transform.localPosition);
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < bones.Count; i++)
        {
            if (Vector2.Distance(bones[i].transform.localPosition, bonePositions[i]) > TOO_FAR)
            {
                bones[i].transform.localPosition = bonePositions[i];
                bones[i].velocity = Vector2.zero;
            }

            Vector3 currentPos = bones[i].transform.localPosition;
            Vector3 currentVel = bones[i].velocity;
            Vector2 headToCenterForce = -currentPos * Mathf.Max(currentPos.magnitude * currentPos.magnitude - 5f, 0); // makes sure that if a bone gets stuck on a corner it goes to the center of the object
            Vector2 keepShapeForce = (bonePositions[i] - currentPos) * flex * Mathf.Max(Vector2.Dot(bonePositions[i], transform.InverseTransformVector(body.velocity)) * 0.5f * stretch + stretch + 1, 0.7f);
            Vector2 neighborForce = (bones[(i + 1) % bones.Count].transform.localPosition - currentPos + (bones[(i - 1 < 0 ? bones.Count : i) - 1].transform.localPosition - currentPos)) * rigidness;
            Vector2 rigidForce = -currentVel * rigidness;

            bones[i].velocity += headToCenterForce + (keepShapeForce + neighborForce + rigidForce) * Time.fixedDeltaTime;
        }

        Collider2D[] colliders = new Collider2D[4];
        int hits = Physics2D.OverlapCircleNonAlloc(transform.position, radius, colliders, collisionLayer);
        for (int i = 0; i < hits; i++)
        {
            if (colliders[i].isTrigger) continue;
            Vector2 hitOffset = (Vector2)transform.position - colliders[i].ClosestPoint(transform.position);
            body.velocity += hitOffset.normalized * (Time.fixedDeltaTime * (5 * radius) * bounce / Mathf.Max(hitOffset.magnitude * hitOffset.magnitude, 0.01f));
        }
    }
}
