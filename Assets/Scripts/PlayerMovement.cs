using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Main Movement")]
    public float movementSpeed;
    public float movementMaxSpeed;

    public float movementDrag;

    private Rigidbody2D body;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float currentSpeed = body.velocity.magnitude; // need to get speed before our changes for later use
        body.velocity -= body.velocity.normalized * (movementDrag * Time.fixedDeltaTime);

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        body.velocity += input * (movementSpeed * Time.fixedDeltaTime);

        if (body.velocity.magnitude > movementMaxSpeed)
        {
            body.velocity = body.velocity.normalized * currentSpeed; // this is the later use -- makes sure we dont accelerate further if we are above max speed
        }
    }
}