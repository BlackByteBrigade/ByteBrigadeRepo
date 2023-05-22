using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class Enemy : Cell
{
    public GameObject Player { get; set; }
    public Collider2D Weakspot;

    public bool IsInVulnerableState;
    public int DmgFromTouching;
    public float DistanceToPlayer { get; set; }
    public float ReactsToPlayerDistance;
    public float AlarmedByPlayerDistance;
    public Alertness AlertnessLevel { get; set; }
    public float CountdownTillRestAlertness;
    public float CountdownTillRestAlertnessRemaining { get; set; }

    private Random _rand;
    private int _numberofAlertStates;
    private DateTime _lastAlertRise { get; set; }

    private bool Has3SecondsPassedSinceLastAlertRise => (_lastAlertRise == default(DateTime) || (DateTime.Now - _lastAlertRise).TotalSeconds > 3);


    private DateTime BecameVulnerable;
    public int DurationVulnerable;

    //movement
    public Rigidbody2D MyRigidbody { get; set; }
    public bool HandleBasicMovement;
    public float MoveRadius;
    public float MoveSpeed;
    private Vector3 SpawnPos;
    private DateTime startedMoveing;
    private DateTime stoppedMoving;

    // Start is called before the first frame update
    public void Start()
    {
        MyRigidbody = gameObject.GetComponent<Rigidbody2D>();
        SpawnPos = transform.position;
        Player = GameObject.Find("Player");
        Debug.Log("Hello World");
        Debug.Log(Player);
        _rand = new Random(DateTime.Now.Millisecond);
        _numberofAlertStates = Enum.GetNames(typeof(Alertness)).Length;
    }

    // Update is called once per frame
    public void Update()
    {
        DistanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);
        HandleAlertness();
        if (BecameVulnerable != default && (DateTime.Now - BecameVulnerable).TotalSeconds >= DurationVulnerable)
        {
            BecameVulnerable = default;
            IsInVulnerableState = false;
        }

        HandleMovement();
    }

    public void HandleMovement()
    {
        if (HandleBasicMovement)
        {
            MyRigidbody.velocity = transform.right * MoveSpeed;
            //MyRigidbody.velocity = transform.right * MoveSpeed * -1;
            //MyRigidbody.velocity = transform.up * MoveSpeed;
            //MyRigidbody.velocity = transform.up * MoveSpeed * -1;
        }
    }

    public void BecameVulnerableNow()
    {
        BecameVulnerable = DateTime.Now;
        IsInVulnerableState = true;
    }

    public void HandleAlertness()
    {
        if (DistanceToPlayer <= AlarmedByPlayerDistance)
        {
            AlertnessLevel = Alertness.Engaged;
            _lastAlertRise = DateTime.Now;
            //Debug.Log($"AlertnessLevel increased to {AlertnessLevel:G}");
        }
        else if (DistanceToPlayer <= ReactsToPlayerDistance && Has3SecondsPassedSinceLastAlertRise && _rand.Next(0, 10) % 4 == 0)
        {
            CountdownTillRestAlertnessRemaining = CountdownTillRestAlertness;
            _lastAlertRise = DateTime.Now;
            if ((int)AlertnessLevel < _numberofAlertStates)
            {
                AlertnessLevel++;
                //Debug.Log($"AlertnessLevel increased to {AlertnessLevel:G}");
            }
        }
        else if (AlertnessLevel > 0 && CountdownTillRestAlertnessRemaining <= 0)
        {
            AlertnessLevel--;
            //Debug.Log($"AlertnessLevel decreased to {AlertnessLevel:G}");

        }

        if (CountdownTillRestAlertnessRemaining > 0)
        {
            CountdownTillRestAlertnessRemaining -= Time.deltaTime;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        var playerScript = Player.GetComponent<Player>();
        //Debug.Log($"Bump!");
        //we go though any because we don't want to dmg the player if the player is touching the enemy in a weakened state 
        if (collision.contacts.Any(contact => contact.collider == Weakspot || contact.otherCollider == Weakspot) && IsInVulnerableState && playerScript.State == PlayerState.Dashing)
        {
            TakeDamage(100); //todo get amount of dmg from player object 
        }
        else if (collision.contacts.Any(contact => contact.otherCollider == Player || contact.collider == Player))
        {
            playerScript.TakeDamage(DmgFromTouching);
        }
    }

    public enum Alertness
    {
        Idle = 0,
        Noticed,
        Engaged
    }
}