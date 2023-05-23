using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Main Movement")]
    public float movementSpeed;
    public float movementMaxSpeed;

    public float movementDrag;
    public float idleSpeed; 

    [Header("Dash")]
    public float dashSpeed;
    public float dashTime;
    public float dashCooldown;

    // dont want to have too many things in the inspector
    public Rigidbody2D Body { get; set; }
    public Player Player { get; set; }

    private Vector2 movementInput;
    private bool isDashing = false;
    private bool canDash = true;

    private void Awake()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        Body = GetComponent<Rigidbody2D>();
        Player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        // we do main movement on fixed update because we are simply updating velocity, which is only updated on fixed updates anyways
        UpdateMovement();
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
        float currentSpeed = Body.velocity.magnitude;

        if (Player.State != PlayerState.Dashing)
        {
            // only change moving and idling states if we are not currently dashing
            Player.State = Body.velocity.magnitude < idleSpeed ? PlayerState.Idling : PlayerState.Moving;
        }

        Body.velocity += movementInput * (movementSpeed * Time.fixedDeltaTime);

        if (movementInput.magnitude < 0.5f)
        {
            // only add drag if we aren't pressing anything
            Body.velocity -= Body.velocity.normalized * (movementDrag * Time.fixedDeltaTime);
        }
        else if (Body.velocity.magnitude > movementMaxSpeed)
        {
            // this is the later use -- makes sure we dont accelerate further if we are above max speed
            Body.velocity = Body.velocity.normalized * Mathf.Min(currentSpeed, Body.velocity.magnitude);
        }
    }

    private IEnumerator Dash()
    {
        Player.State = PlayerState.Dashing;
        canDash = false;

        AudioManager.instance.PlaySfX(SoundEffects.PlayerDash);

        Vector2 dashDir = movementInput.magnitude > 0.5f ? movementInput : Body.velocity.normalized;
        Body.velocity = dashDir * dashSpeed;

        yield return new WaitForSeconds(dashTime);

        Player.State = PlayerState.Moving;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }
}