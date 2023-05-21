using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Main Movement")]
    public float movementSpeed;
    public float movementMaxSpeed;

    public float movementDrag;

    [Header("Dash")]
    public float dashSpeed;
    public float dashTime;
    public float dashCooldown;

    // dont want to have too many things in the inspector
    private Rigidbody2D body;

    private Vector2 movementInput;
    private bool isDashing = false;
    private bool canDash = true;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // we do main movement on fixed update because we are simply updating velocity, which is only updated on fixed updates anyways
        if (!isDashing) UpdateMovement();
    }

    private void Update()
    {
        movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // we check for dashes every frame so that we dont accidentally miss a frame when we are pressing space
        if (canDash && Input.GetKeyDown(KeyCode.Space)) StartCoroutine(Dash());
    }

    private void UpdateMovement()
    {
        // need to get speed before our changes for later use
        float currentSpeed = body.velocity.magnitude;

        body.velocity += movementInput * (movementSpeed * Time.fixedDeltaTime);

        if (movementInput.magnitude < 0.5f)
        {
            // only add drag if we aren't pressing anything
            body.velocity -= body.velocity.normalized * (movementDrag * Time.fixedDeltaTime);
        }
        else if (body.velocity.magnitude > movementMaxSpeed)
        {
            // this is the later use -- makes sure we dont accelerate further if we are above max speed
            body.velocity = body.velocity.normalized * Mathf.Min(currentSpeed, body.velocity.magnitude);
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;

        Vector2 dashDir = movementInput.magnitude > 0.5f ? movementInput : body.velocity.normalized;
        body.velocity = dashDir * dashSpeed;

        yield return new WaitForSeconds(dashTime);

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }
}