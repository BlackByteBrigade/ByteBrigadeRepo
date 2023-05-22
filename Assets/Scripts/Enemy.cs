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

    [Header("Patrolling")]
    [SerializeField] float distanceToMove;
    [SerializeField] float speed;
    [SerializeField] float timeBtwMoves;
    private Vector2 movementVector;
    private float timeCounter;
    private bool isReached;


    // Start is called before the first frame update
    public void Start()
    {
        Player = GameObject.Find("Player");
        Debug.Log("Hello World");
        Debug.Log(Player);
        _rand = new Random(DateTime.Now.Millisecond);
        _numberofAlertStates = Enum.GetNames(typeof(Alertness)).Length;
    }

    // Update is called once per frame
    public void Update()
    {
        Patrolling();

        DistanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);
        HandleAlertness();
        if (BecameVulnerable != default && (DateTime.Now - BecameVulnerable).TotalSeconds >= DurationVulnerable)
        {
            BecameVulnerable = default;
            IsInVulnerableState = false;
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
        //Debug.Log($"Bump!");
        //we go though any because we don't want to dmg the player if the player is touching the enemy in a weakened state 
        if (collision.contacts.Any(contact => contact.collider == Weakspot || contact.otherCollider == Weakspot) && IsInVulnerableState)
        {
            TakeDamage(100); //todo get amount of dmg from player object 
        }
        else if (collision.contacts.Any(contact => contact.otherCollider == Player || contact.collider == Player))
        {
            Player.GetComponent<Player>().TakeDamage(DmgFromTouching);
        }
    }

    public enum Alertness
    {
        Idle = 0,
        Noticed,
        Engaged
    }
    private void Patrolling()
    {
        if (isReached && timeBtwMoves < timeCounter)
        {
            DirectionDecider();
            isReached = false;
            timeCounter = 0;
        }
        else if (!isReached)
        {
            transform.position = Vector2.MoveTowards(transform.position, movementVector, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, movementVector) < 0.1f)
            {
                isReached = true;
            }
        }
        else
        {
            timeCounter += Time.deltaTime;
        }
    }
    private void DirectionDecider()
    {
        var RaycastHitUp = Physics2D.Raycast(transform.position, Vector2.up, distanceToMove);
        var RaycastHitDown = Physics2D.Raycast(transform.position, Vector2.down, distanceToMove);
        var RaycastHitLeft = Physics2D.Raycast(transform.position, Vector2.left, distanceToMove);
        var RaycastHitRight = Physics2D.Raycast(transform.position, Vector2.right, distanceToMove);

        var listOfDirections = new List<Vector2>();

        if (RaycastHitUp.collider == null)
        {
            listOfDirections.Add(Vector2.up);
        }
        if (RaycastHitDown.collider == null)
        {
            listOfDirections.Add(Vector2.down);
        }
        if (RaycastHitLeft.collider == null)
        {
            listOfDirections.Add(Vector2.left);
        }
        if (RaycastHitRight.collider == null)
        {
            listOfDirections.Add(Vector2.right);
        }

        if (listOfDirections.Count == 0)
        {
            movementVector = transform.position;
        }
        else
        {
            movementVector = transform.position + (Vector3)listOfDirections[UnityEngine.Random.Range(0, listOfDirections.Count)] * distanceToMove;
        }
        listOfDirections.Clear();
    }
}