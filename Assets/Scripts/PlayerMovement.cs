﻿using System.Collections;
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
    public bool isDashCancelable = true;
    public SpriteRenderer dashVFXSprite;

    // dont want to have too many things in the inspector
    public Rigidbody2D Body { get; set; }
    public Player Player { get; set; }
    public SpriteRenderer Sprite { get; set; }

    private Vector2 movementInput;
    private bool canDash = true;
    private Color TMP_originalColor;

    private void Awake()
    {
        InitializeComponents();
        EndDashVFX();
    }

    private void InitializeComponents()
    {
        Body = GetComponent<Rigidbody2D>();
        Player = GetComponent<Player>();
        Sprite = GetComponent<SpriteRenderer>();
        TMP_originalColor = Sprite.color;
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

        // Sprite.color = Color.red; // TODO make visual better for dash
        

        Vector2 dashDir = movementInput.magnitude > 0.5f ? movementInput : Body.velocity.normalized;
        Body.velocity = dashDir * dashSpeed;

        StartDashVFX();

        yield return new WaitForSeconds(dashTime);

        EndDash();

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    private void StartDashVFX() {
        dashVFXSprite.enabled = true;
        float angle = Mathf.Atan2(Body.velocity.y, Body.velocity.x) * Mathf.Rad2Deg;
        // float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle += (-60f); //TEMP sprite angle offset
        // dashVFXSprite.transform.Rotate(0, 0, angle);
        dashVFXSprite.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    private void EndDashVFX() {
        dashVFXSprite.enabled = false;
    }

    private void EndDash()
    {
        Player.State = PlayerState.Moving;
        // Sprite.color = TMP_originalColor;// TODO make better visual for when dash ends
        EndDashVFX();
    }

    public void CancelDash()
    {
        if (isDashCancelable)
        {
            EndDash();
        }
    }
}